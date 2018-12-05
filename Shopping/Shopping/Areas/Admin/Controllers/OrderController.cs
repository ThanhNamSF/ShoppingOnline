using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Constants;
using Common.SearchConditions;
using DataAccess.Interfaces;
using DataAccess.Models;
using PayPal.Api;
using Shopping.Helpers;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Shopping.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;

        public OrderController(IOrderService orderService, IUserService userService, ICustomerService customerService, IInvoiceService invoiceService)
        {
            _orderService = orderService;
            _userService = userService;
            _customerService = customerService;
            _invoiceService = invoiceService;
        }

        #region CRUD Order

        // GET: Admin/order
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new OrderSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, OrderSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var orders = _orderService.SearchOrders(condition);
            var gridModel = new DataSourceResult()
            {
                Data = orders.DataSource,
                Total = orders.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Edit(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return RedirectToAction("List");
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(OrderModel model)
        {
            try
            {
                var order = _orderService.GetOrderById(model.Id);
                if (order == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.UpdatedBy = currentUser.Id;
                model.UpdatedDateTime = DateTime.Now;
                _orderService.UpdateOrder(model);
                SuccessNotification("Update order information successfully!");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Update order information failed!");
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                if (order == null)
                    return RedirectToAction("List");
                if (order.IsHasInvoice)
                {
                    ErrorNotification("Delete order failed. This order have been invoiced ");
                    return RedirectToAction("List");
                }
                _orderService.DeleteOrder(id);
                SuccessNotification("Delete order successfully.");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Delete order failed.");
                return RedirectToAction("List");
            }
        }
        #endregion

        #region Order Detail

        [HttpPost]
        public ActionResult OrderDetailList(DataSourceRequest command, int orderId)
        {
            var orderDetails = _orderService.SearchOrderDetails(new OrderDetailSearchCondition()
            {
                OrderId = orderId,
                PageSize = command.PageSize,
                PageNumber = command.Page - 1
            });
            orderDetails.DataSource.ForEach(x => x.SubAmount = x.Quantity * x.UnitPrice);
            var gridModel = new DataSourceResult
            {
                Data = orderDetails.DataSource,
                Total = orderDetails.TotalItems
            };
            return Json(gridModel);
        }

        #endregion

        #region Approved/Open/Cancel/Create Invoice
        [HttpPost]
        public ActionResult Approve(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return RedirectToAction("List");
            }

            var customer = _customerService.GetCustomerById(order.CustomerId);
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            if (currentUser != null)
            {
                order.ApproverId = currentUser.Id;
                order.Status = true;
            }
            _orderService.Approved(order);
            if (customer != null)
            {
                string emailContent = GetBodyOfOrderInformation(order, customer);
                SendEmailHelper.SendEmailToCustomer(customer.Email, customer.LastName + " " + customer.FirstName, "Order information", emailContent);
                string smsContent =
                    "Your order have been approved. Please check your email for more information of order. Thanks!!!";
                SendSmsToCustomer(customer.Phone, smsContent);
            }
            SuccessNotification("Approve order successfully.");
            return RedirectToAction("Edit", new { order.Id });
        }

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return RedirectToAction("List");
            }

            var customer = _customerService.GetCustomerById(order.CustomerId);
            if (RefundMoneyFromPaymentId(order.PaymentId))
            {
                _orderService.Cancel(id);
                SuccessNotification("Cancel order successfully.");
                if (customer != null)
                {
                    string emailContent = GetBodyCancelOrder(customer);
                    SendEmailHelper.SendEmailToCustomer(customer.Email, customer.LastName + " " + customer.FirstName, "Order information", emailContent);
                    string smsContent =
                        "Your order have been cancelled. Please check your email for more information of order. Thanks!!!";
                    SendSmsToCustomer(customer.Phone, smsContent);
                }
            }
            else
            {
                ErrorNotification("Cancel order failed");
            }
            return RedirectToAction("Edit", new { order.Id });
        }

        [HttpPost]
        public ActionResult CreateInvoice(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return RedirectToAction("List");
            }
            var customer = _customerService.GetCustomerById(order.CustomerId);
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            if (currentUser != null)
            {
                var invoice = _invoiceService.CreateNewInvoice(currentUser.Id, order);
                _invoiceService.InsertInvoice(invoice);
                _orderService.CreateInvoice(order.Id);
                SuccessNotification("Make invoice successfully. Please check it in invoice list.");
                return RedirectToAction("Edit", new { order.Id });
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        private bool RefundMoneyFromPaymentId(string paymentId)
        {
            try
            {
                var apiContext = Configuration.GetAPIContext();
                var payment = Payment.Get(apiContext, paymentId);
                var paymentAmount = payment.transactions[0].amount;
                var paymentInvoiceNumber = payment.transactions[0].invoice_number;
                var paymentSaleId = payment.transactions[0].related_resources[0].sale.id;
                var refundAmount = new Amount()
                {
                    total = paymentAmount.total,
                    currency = paymentAmount.currency
                };
                var refundRequest = new RefundRequest()
                {
                    amount = refundAmount,
                    invoice_number = paymentInvoiceNumber
                };
                Sale.Refund(apiContext, paymentSaleId, refundRequest);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GetBodyCancelOrder(CustomerModel customer)
        {
            string body = "Xin chào " + customer.UserName + ",";
            body += "<br /><br /><strong>Đã xảy ra sự cố khi đặt hàng nên đơn hàng của quý khách đã bị hủy.</strong>";
            body += "<br /><strong>Số tiền của quý khách đã được hoàn lại!</strong>";
            body += "<br />Chúng tôi chân thành xin lỗi vì sự cố trên. Mong quý khách thông cảm ạ.";
            return body;
        }

        private string GetBodyOfOrderInformation(OrderModel order, CustomerModel customer)
        {
            var orderDetails = _orderService.GetOrderDetailsByOrderId(order.Id);
            string body = "<p>Kính chào quý khách,</p>";
            body += "<br />Cảm ơn quý khách đã mua hàng tại Downy Shoes";
            body += "<br />Thông tin đơn hàng của quý khách bao gồm:";
            body +=
                "<table style='ORDER-TOP:rgb(222, 226, 227) 1px solid; BORDER-RIGHT:rgb(222, 226, 227) 1px solid; WHITE-SPACE:normal; WORD-SPACING:0px; BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; TEXT-TRANSFORM:none; COLOR:rgb(34, 34, 34); PADDING-BOTTOM:6px; PADDING-TOP:6px; FONT:12px arial, sans-serif; PADDING-LEFT:6px; BORDER-LEFT:rgb(222, 226, 227) 1px solid; LETTER-SPACING:normal; PADDING-RIGHT:6px; BACKGROUND-COLOR:rgb(255, 255, 255); TEXT-INDENT:0px' cellspacing='0' cellpadding='0'></table>";
            body += "<tbody>";
            body += "<tr>";

            body += "<td style='FONT-FAMILY:arial,sans-serif;TEXT-ALIGN:left;MARGIN:0px'>";
            body += "<p> Mã đơn hàng: ";
            body += "<b>" + order.Code + "</b>";
            body += "</p>";
            body += "<p>Ngày: " + order.CreatedDateTime.ToString("dd/MM/yyyy HH:mm:ss") + "</p>";
            body += "<p>Địa chỉ: ";
            body += "<b>" + order.ReceiverAddress + "</b>";
            body += "</p>";
            body += "<br/>";
            body += "</td>";

            body += "<td style='FONT-FAMILY:arial,sans-serif;MARGIN:0px'>";
            body += "<p> Khách hàng: ";
            body += "<b>" + customer.LastName + " " + customer.FirstName + "</b>";
            body += "</p>";
            body += "<p> Người nhận: ";
            body += "<b>" + order.ReceiverName + "</b>";
            body += "</p>";
            body += "<p>Điện thoại: ";
            body += "<b>" + order.ReceiverPhone + "</b>";
            body += "</p>";
            body += "<br/>";
            body += "</td>";

            body += "</tr>";

            body += "<tr>";
            body += "<td style='FONT-FAMILY:arial,sans-serif; MARGIN: 0px' colspan='2'>";
            body += "<table style='BORDER-TOP:rgb(222, 226, 227) 1px solid; HEIGHT: 94px; BORDER-RIGHT:rgb(222, 226, 227) 1px solid; WIDTH: 512px; BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; BORDER-LEFT:rgb(222, 226, 227) 1px solid' cellspacing='0' cellpadding='6'>";
            body += "<thead style='BACKGROUND:rgb(220, 239, 245); FONT-WEIGHT:bold; TEXT-ALIGN:center'>";
            body += "<tr>";
            body += "<th>STT</th>";
            body += "<th>Sản phẩm</th>";
            body += "<th>SL</th>";
            body += "<th>Đơn giá</th>";
            body += "<th>Thành tiền</th>";
            body += "<th></th>";
            body += "</tr>";
            body += "</thead>";

            body += "<tbody>";
            int stt = 1;
            foreach (var item in orderDetails)
            {
                body += "<tr style='BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; TEXT-ALIGN:center'>";
                body += "<td style='FONT-FAMILY:arial,sans-serif; BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; MARGIN: 0px; TEXT-ALIGN:center''>" + (stt++) + "</td>";
                body += "<td style='FONT-FAMILY:arial,sans-serif; BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; MARGIN: 0px; TEXT-ALIGN:center''>" + item.ProductName + "</td>";
                body += "<td style='FONT-FAMILY:arial,sans-serif; BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; MARGIN: 0px; TEXT-ALIGN:center''>" + item.Quantity + "</td>";
                body += "<td style='FONT-FAMILY:arial,sans-serif; BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; MARGIN: 0px; TEXT-ALIGN:center''>" + string.Format("{0:$#,#}", item.UnitPrice) + "</td>";
                body += "<td style='FONT-FAMILY:arial,sans-serif; BORDER-BOTTOM:rgb(222, 226, 227) 1px solid; MARGIN: 0px; TEXT-ALIGN:center''>" + string.Format("{0:$#,#}", item.Quantity * item.UnitPrice) + "</td>";
                body += "</tr>";
            }
            body += "</tbody>";

            body += "</table>";
            body += "<p>Tổng cộng: <strong>" + string.Format("{0:$#,#}", order.Amount) + "</strong></p>";

            body += "</td>";
            body += "</tr>";
            body += "</tbody>";
            body += "</table>";

            body += "<br /><br /> Đơn hàng của quý khách sẽ được giao trong vòng 3 ngày kể từ ngày nhận được thông báo này.";
            body += "<br />Xin chân thành cảm ơn quý khách đã đặt hàng tại cửa hàng chúng tôi.";
            return body;
        }

        private void SendSmsToCustomer(string phoneNumber, string body)
        {
            try
            {
                var to = new PhoneNumber(phoneNumber);
                var from = new PhoneNumber("+19257054026");
                var message = MessageResource.Create(
                    to: to,
                    from: from,
                    body: body
                );
            }
            catch (Exception e)
            {
                
            }
            
        }
        #endregion
    }
}