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
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        // GET: Feedback
        public ActionResult Index()
        {
            return RedirectToAction("Create");
        }

        public ActionResult Create()
        {
            var model = new FeedbackModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FeedbackModel model)
        {
            try
            {
                ModelState.Remove("ReplyContent");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.CUSTOMER_SESSION] as CustomerModel;
                model.CustomerId = currentUser.Id;
                model.CreatedDateTime = DateTime.Now;
                _feedbackService.InsertFeedback(model);
                return View(new FeedbackModel());
            }
            catch (Exception e)
            {
                return View(model);
            }
        }
    }
}