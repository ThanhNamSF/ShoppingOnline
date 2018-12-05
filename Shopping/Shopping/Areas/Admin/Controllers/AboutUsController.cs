using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Shopping.Areas.Admin.Controllers
{
    public class AboutUsController : BaseController
    {
        private readonly IAboutService _aboutService;

        public AboutUsController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }
        // GET: Admin/AboutUs
        public ActionResult Index()
        {
            return RedirectToAction("Edit");
        }

        public ActionResult Edit()
        {
            var about = _aboutService.GetAboutUs();
            return View(about);
        }

        [HttpPost]
        public ActionResult Edit(AboutModel model)
        {
            try
            {
                var about = _aboutService.GetAboutUs();
                if (!ModelState.IsValid)
                    return View(model);
                _aboutService.UpdateAbout(model);
                SuccessNotification("Update information successfully!");
                return RedirectToAction("Edit");
            }
            catch (Exception e)
            {
                ErrorNotification("Update information failed!");
                return View(model);
            }
        }
    }
}