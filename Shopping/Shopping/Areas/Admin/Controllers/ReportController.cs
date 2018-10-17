using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.SearchConditions;

namespace Shopping.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        public ActionResult ProductInventoryReport()
        {
            var model = new InventorySearchConditionReport
            {
                CategoryId = 1
            };
            return View(model);
        }
    }
}