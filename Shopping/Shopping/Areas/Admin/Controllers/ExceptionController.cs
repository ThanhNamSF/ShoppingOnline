using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Areas.Admin.Controllers
{
    public class ExceptionController : BaseController
    {
        public ActionResult UnAuthorize()
        {
            return View();
        }
    }
}