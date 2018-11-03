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
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }


        // GET: Login
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userLogin = _userService.GetUserLogin(model, Values.CustomerRole);
            if (userLogin != null)
            {
                Session.Add(Values.CUSTOMER_SESSION, userLogin);
                return RedirectToAction("Index", "Home");
            }
            return Login();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Login");
            model.CreatedDateTime = DateTime.Now;
            model.Role = Values.CustomerRole;
            _userService.CreateUser(model);
            return RedirectToAction("Login");
        }
    }
}