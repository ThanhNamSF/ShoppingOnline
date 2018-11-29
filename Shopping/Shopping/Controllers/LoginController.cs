using Common;
using Common.Constants;
using DataAccess.Interfaces;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers
{
    public class LoginController : Controller
    {
        private readonly ICustomerService _customerService;

        public LoginController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        // GET: Login
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            var model = new CustomerModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(CustomerModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var customerLogin = _customerService.GetCustomerLogin(model);
            if (customerLogin != null)
            {
                Session.Add(Values.CUSTOMER_SESSION, customerLogin);
                return RedirectToAction("Index", "Home");
            }
            return Login();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Login");
            model.CreatedDateTime = DateTime.Now;
            _customerService.CreateCustomer(model);
            return RedirectToAction("Login");
        }
    }
}