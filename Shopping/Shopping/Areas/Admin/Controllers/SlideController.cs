using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Constants;
using Common.SearchConditions;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Shopping.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        private readonly ISlideService _slideService;

        public SlideController(ISlideService slideService)
        {
            _slideService = slideService;
        }
        // GET: Admin/Slide
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new SlideSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, SlideSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var slides = _slideService.SearchSlides(condition);
            var gridModel = new DataSourceResult()
            {
                Data = slides.DataSource,
                Total = slides.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            var model = new SlideModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(SlideModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.CreatedBy = currentUser.Id;
                model.CreatedDateTime = DateTime.Now;
                _slideService.InsertSlide(model);
                SuccessNotification("Add new slider successfully.");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Add new slider failed");
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var slide = _slideService.GetSlideById(id);
            if (slide == null)
                return RedirectToAction("List");
            return View(slide);
        }

        [HttpPost]
        public ActionResult Edit(SlideModel model)
        {
            try
            {
                var slide = _slideService.GetSlideById(model.Id);
                if (slide == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.UpdatedBy = currentUser.Id;
                model.UpdatedDateTime = DateTime.Now;
                _slideService.UpdateSlide(model);
                SuccessNotification("Update slider successfully!");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Update slider failed!");
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var slide = _slideService.GetSlideById(id);
                if (slide == null)
                    return RedirectToAction("List");
                _slideService.DeleteSlide(id);
                SuccessNotification("Delete slider successfully.");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Delete slider failed");
                return RedirectToAction("List");
            }
        }
    }
}