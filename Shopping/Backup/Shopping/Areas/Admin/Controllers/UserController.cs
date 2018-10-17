using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using DataAccess.Interfaces;

namespace Shopping.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public JsonResult GetAllAdminUser()
        {
            var users = _userService.GetAllUserByRole((int)UserRole.Admin);
            return this.Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}