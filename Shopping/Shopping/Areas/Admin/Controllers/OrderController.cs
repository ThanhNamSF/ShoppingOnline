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

        public OrderController(IOrderService orderService, IUserService userService, ICustomerService customerService)
        {
            _orderService = orderService;
            _userService = userService;
            _customerService = customerService;
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
                SuccessNotification("Cập nhật thông tin đơn hàng thành công!");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Cập nhật sản phẩm thất bại");
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
                _orderService.DeleteOrder(id);
                SuccessNotification("Xóa sản phẩm thành công.");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Xóa sản phẩm thất bại");
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

        #region Approved/Open/Cancel
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
                SendEmailHelper.SendEmailToCustomer(customer.Email, customer.FirstName + " " + customer.LastName, "Thông tin về đơn hàng", emailContent);
                string smsContent =
                    "Đơn hàng của bạn đã được xác nhận. Mời bạn kiểm tra email để biết thêm thông tin chi tiết. Xin cảm ơn!!!";
                SendSmsToCustomer(customer.Phone, smsContent);
            }
            SuccessNotification("Duyệt đơn hàng thành công.");
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
                SuccessNotification("Hủy hóa đơn thành công.");
                if (customer != null)
                {
                    string emailContent = GetBodyCancelOrder(customer);
                    SendEmailHelper.SendEmailToCustomer(customer.Email, customer.FirstName + " " + customer.LastName, "Thông tin về đơn hàng", emailContent);
                    string smsContent =
                        "Đơn hàng của bạn đã bị hủy. Mời bạn kiểm trả email để biết thêm thông tin chi tiết. Xin cảm ơn!!!";
                    SendSmsToCustomer(customer.Phone, smsContent);
                }
            }
            else
            {
                ErrorNotification("Hủy hóa đơn thất bại");
            }
            return RedirectToAction("Edit", new { order.Id });
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
            string body = "Xin chào " + customer.UserName + ",";
            body += "<br /><br />Shop đã nhận được đơn đặt hàng của quý khách.";
            body += "<br />Thông tin đơn hàng của quý khách bao gồm:";
            body += "<br /><strong>Ngày đặt hàng: " + order.CreatedDateTime.ToString("dd/MM/yyyy HH:mm:ss") + "</strong>";
            body += "<br /><strong>Địa chỉ giao hàng: " + order.ReceiverAddress + "</strong>";
            body += "<br /><strong>Tên người nhận: " + order.ReceiverName + "</strong>";
            body += "<br /><strong>Số điện thoại: " + order.ReceiverPhone + "</strong>";
            body += "<br /><strong>Các sản phẩm bao gồm:</strong>";
            foreach (var item in orderDetails)
            {
                body += "<br /><strong>" + item.ProductName + "(Số lượng: " + item.Quantity + ").</strong>";
            }

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