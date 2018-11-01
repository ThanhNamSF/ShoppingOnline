using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Constants;
using Common.SearchConditions;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Shopping.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        // GET: Admin/Contact
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new ContactSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, ContactSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var contacts = _contactService.SearchContacts(condition);
            var gridModel = new DataSourceResult()
            {
                Data = contacts.DataSource,
                Total = contacts.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Edit(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null)
                return RedirectToAction("List");
            return View(contact);
        }

        [HttpPost]
        public ActionResult Edit(ContactModel model)
        {
            try
            {
                var contact = _contactService.GetContactById(model.Id);
                if (contact == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                _contactService.UpdateContact(model);
                SuccessNotification("Cập nhật thành công!");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Cập nhật thất bại");
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var contact = _contactService.GetContactById(id);
                if (contact == null)
                    return RedirectToAction("List");
                _contactService.DeleteContact(id);
                SuccessNotification("Xóa thành công.");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Xóa thất bại");
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Reply(ContactModel model)
        {
            try
            {
                var contact = _contactService.GetContactById(model.Id);
                if (contact == null)
                    return RedirectToAction("List");
                if (string.IsNullOrEmpty(model.ReplyContent))
                {
                    ErrorNotification("Nội dung trả lời không được để trống!");
                    return RedirectToAction("Edit", new { id = contact.Id });
                }
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                contact.ReplierId = currentUser.Id;
                contact.CreatedDateTime = DateTime.Now;
                contact.Content = model.ReplyContent;
                SendEmailHelper.SendEmailToCustomer(contact.Email, contact.FullName, "Thông tin phản hồi", contact.ReplyContent);
                _contactService.UpdateContact(contact);
                SuccessNotification("Trả lời phản hồi thành công!");
                return RedirectToAction("Edit", new { id = contact.Id });
            }
            catch (Exception e)
            {
                ErrorNotification("Trả lời phản hồi thất bại!");
                return RedirectToAction("Edit", new { id = model.Id });
            }
        }
    }
}