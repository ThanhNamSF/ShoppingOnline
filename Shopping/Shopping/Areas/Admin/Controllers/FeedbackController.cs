using Common;
using Common.Constants;
using Common.SearchConditions;
using DataAccess.Interfaces;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        // GET: Admin/Feedback
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new FeedbackSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, FeedbackSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var feedbacks = _feedbackService.SearchFeedbacks(condition);
            var gridModel = new DataSourceResult()
            {
                Data = feedbacks.DataSource,
                Total = feedbacks.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Edit(int id)
        {
            var feedback = _feedbackService.GetFeedbackById(id);
            if (feedback == null)
                return RedirectToAction("List");
            return View(feedback);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var feedback = _feedbackService.GetFeedbackById(id);
                if (feedback == null)
                    return RedirectToAction("List");
                _feedbackService.DeleteFeedback(id);
                SuccessNotification("Delete successfully.");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Deleted failed");
                return RedirectToAction("List");
            }
        }

        public ActionResult GetAllHistoryReplies(int feedbackId)
        {
            var model = _feedbackService.GetAllFeedbackByFeedbackId(feedbackId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Reply(FeedbackModel model)
        {
            try
            {
                var feedback = _feedbackService.GetFeedbackById(model.Id);
                if (feedback == null)
                    return RedirectToAction("List");
                if (string.IsNullOrEmpty(model.ReplyContent))
                {
                    ErrorNotification("Reply content is required!");
                    return RedirectToAction("Edit", new { id = feedback.Id });
                }
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                feedback.ReplierId = currentUser.Id;
                feedback.RepliedDateTime = DateTime.Now;
                feedback.ReplyContent = model.ReplyContent;
                SendEmailHelper.SendEmailToCustomer(feedback.Email, feedback.FullName, "Feedback information", feedback.ReplyContent);
                _feedbackService.AddReply(feedback);
                SuccessNotification("Reply feedback successfully!");
                return RedirectToAction("Edit", new { id = feedback.Id });
            }
            catch (Exception e)
            {
                ErrorNotification("Reply feedback failed!");
                return RedirectToAction("Edit", new { id = model.Id });
            }
        }
    }
}