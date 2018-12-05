using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common.Constants;
using DataAccess.Interfaces;
using DataAccess.Models;
using PayPal.Api;
using Shopping.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio;
using Twilio.AspNet.Mvc;
using Configuration = Shopping.Helpers.Configuration;
using Common;

namespace Shopping.Controllers
{
    public class PaypalController : BaseController
    {
        private readonly IOrderService _orderService;
        private static ReceiverInformation _receiverInformation;
        private readonly IProductService _productService;
        private readonly ICodeGeneratingService _codeGeneratingService;

        public PaypalController(IOrderService orderService, IProductService productService, ICodeGeneratingService codeGeneratingService)
        {
            _orderService = orderService;
            _productService = productService;
            _codeGeneratingService = codeGeneratingService;
        }
        // GET: Paypal
        public ActionResult Index()
        {
            return View();
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var cart = Session[Values.CartSession] as List<CartItem>;
            if (cart == null || cart.Count == 0)
                return null;
            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            double subAmount = 0; //USD
            double taxAmount = 1; //USD
            double shippingAmount = 1; //USD

            foreach (var item in cart)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Product.Name,
                    currency = "USD",
                    price = string.Format("{0:n}" , item.Product.Price),
                    quantity = item.Quantity.ToString(),
                    sku = item.Product.Code
                });
                subAmount += item.Product.Price * item.Quantity;
            }
            //itemList.items.Add(new Item()
            //{
            //    name = "Item Name",
            //    currency = "USD",
            //    price = "5",
            //    quantity = "1",
            //    sku = "sku"
            //});

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = string.Format("{0:n}", taxAmount),
                shipping = string.Format("{0:n}", shippingAmount),
                subtotal = string.Format("{0:n}", subAmount)
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = string.Format("{0:n}", taxAmount + shippingAmount + subAmount), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        public ActionResult PaymentWithCreditCard()
        {
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "USD";
            item.price = "5";
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = "NewYork";
            billingAddress.country_code = "US";
            billingAddress.line1 = "23rd street kew gardens";
            billingAddress.postal_code = "43210";
            billingAddress.state = "NY";


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = "874";  //card cvv2 number
            crdtCard.expire_month = 1; //card expire date
            crdtCard.expire_year = 2020; //card expire year
            crdtCard.first_name = "Aman";
            crdtCard.last_name = "Thakur";
            crdtCard.number = "1234567890123456"; //enter your credit card number here
            crdtCard.type = "visa"; //credit card type here paypal allows 4 types

            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "1";
            details.subtotal = "5";
            details.tax = "1";

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = "7";
            amnt.details = details;

            // Now make a transaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = "your invoice number which you are generating";

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {
                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string

                APIContext apiContext = Configuration.GetAPIContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.state is "approved" it means the payment was successful else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            }
            catch (PayPal.PayPalException ex)
            {
                return View("FailureView");
            }

            return View("SuccessView");
        }

        public ActionResult PaymentWithPaypal(ReceiverInformation receiverInformation)
        {
            var cart = Session[Values.CartSession] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                TempData[Values.PaypalNotification] = NotificationType.Empty;
                return RedirectToAction("Index", "Cart");
            }
            if (TryValidateModel(receiverInformation))
            {
                _receiverInformation = new ReceiverInformation
                {
                    Address = receiverInformation.Address,
                    FullName = receiverInformation.FullName,
                    Street = receiverInformation.Street,
                    MobileNumber = receiverInformation.MobileNumber
                };
            }
                
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        TempData[Values.PaypalNotification] = NotificationType.Error;
                        return RedirectToAction("Index", "Cart");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData[Values.PaypalNotification] = NotificationType.Error;
                return RedirectToAction("Index", "Cart");
            }

            InsertOrder();
            TempData[Values.PaypalNotification] = NotificationType.Success;
            return RedirectToAction("Index", "Cart");
        }

        private void InsertOrder()
        {
            var customer = Session[Values.CUSTOMER_SESSION] as CustomerModel;
            var cart = Session[Values.CartSession] as List<CartItem>;
            if (cart != null && cart.Count > 0)
            {
                var order = new OrderModel()
                {
                    ReceiverName = _receiverInformation.FullName,
                    ReceiverPhone = _receiverInformation.MobileNumber,
                    ReceiverAddress = _receiverInformation.Address + ", " + _receiverInformation.Street,
                    CreatedDateTime = DateTime.Now,
                    Amount = cart.Sum(item => item.Product.Price * item.Quantity),
                    Status = false,
                    CustomerId = customer.Id, //Edit later
                    Code = _codeGeneratingService.GenerateCode(Values.OrderPrefix),
                    PaymentId = payment.id
                };
                _orderService.InsertOrder(order);

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetailModel()
                    {
                        OrderId = order.Id,
                        ProductId = item.Product.Id,
                        UnitPrice = item.Product.Price,
                        Quantity = item.Quantity
                    };
                    _orderService.InsertOrderDetail(orderDetail);
                    _productService.DescreaseProduct(item.Product.Id, item.Quantity);
                }
                Session.Remove(Values.CartSession);
            }
        }

        public ActionResult SendSmsToCustomer(string phoneNumber, string body)
        {
            var to = new PhoneNumber("+84967148162");
            var from = new PhoneNumber("+19257054026");
            var message = MessageResource.Create(
                to: to,
                from: from,
                body: "Đơn hàng của bạn đã được xác nhận"
            );
            return View("SuccessView");
        }
    }
}