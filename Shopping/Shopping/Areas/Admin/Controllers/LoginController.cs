using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Constants;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Shopping.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        
        // GET: Admin/Login
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
            var userLogin = _userService.GetUserLogin(model);
            if (userLogin != null)
            {
                Session.Add(Values.USER_SESSION, userLogin);
                return RedirectToAction("Index", "Home");
            }

            TempData["AlertMessage"] = Messages.LOGIN_FAILED;
            return Login();
        }

        public ActionResult Logout()
        {
            if (Session[Values.USER_SESSION] != null)
            {
                Session.Remove(Values.USER_SESSION);
            }

            return RedirectToAction("Login");
        }
    }
}