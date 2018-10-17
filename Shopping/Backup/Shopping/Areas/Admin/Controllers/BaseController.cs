using System.Collections.Generic;
using Common;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Constants;
using DataAccess.Models;

namespace Shopping.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = Session[Values.USER_SESSION] as UserModel;
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }

            base.OnActionExecuted(filterContext);
        }

        protected void SetAlert(string message, NotificationType type)
        {
            TempData["AlertMessage"] = message;
            if (type == NotificationType.Success)
            {
                TempData["AlertType"] = "alert-success";
            }
            else if(type == NotificationType.Error)
            {
                TempData["AlertType"] = "alert-danger";
            }
        }

        /// <summary>
        /// DisplayName success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotificationType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// DisplayName error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotificationType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// DisplayName notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotificationType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = $"notifications.{type}";
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }
    }
}