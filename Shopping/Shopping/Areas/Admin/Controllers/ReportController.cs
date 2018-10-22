using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Constants;
using Common.SearchConditions;
using DataAccess.Models;

namespace Shopping.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        public ActionResult ProductInventoryReport()
        {
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            var model = new InventorySearchConditionReport
            {
                CategoryId = 1,
                CreatedBy = currentUser != null ? currentUser.FirstName + " " + currentUser.LastName : string.Empty
            };
            return View(model);
        }

        public ActionResult RevenueReport()
        {
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            var model = new RevenueSearchConditionReport()
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                CreatedBy = currentUser != null ? currentUser.FirstName + " " + currentUser.LastName : string.Empty
            };
            return View(model);
        }
    }
}