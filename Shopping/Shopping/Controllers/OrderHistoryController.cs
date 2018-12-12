using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Constants;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Shopping.Controllers
{
    public class OrderHistoryController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderHistoryController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: OrderHistory
        public ActionResult Index()
        {
            return RedirectToAction("GetAllOrderHistory");
        }

        public ActionResult GetAllOrderHistory()
        {
            var currentCustomer = Session[Values.CUSTOMER_SESSION] as CustomerModel;
            var model = _orderService.GetAllOrderByCustomerId(currentCustomer.Id);
            return View(model);
        }


    }
}