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
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IGroupUserService _groupUserService;

        public UserController(IUserService userService, IGroupUserService groupUserService)
        {
            _userService = userService;
            _groupUserService = groupUserService;
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new UserSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, UserSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var users = _userService.SearchUsers(condition);
            var gridModel = new DataSourceResult()
            {
                Data = users.DataSource,
                Total = users.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                if (_userService.CheckUserNameIsExisted(model.UserName))
                {
                    model.UserNameIsExisted = true;
                    return View(model);
                }
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.CreatedBy = currentUser.Id;
                model.CreatedDateTime = DateTime.Now;
                _userService.InsertUser(model);
                SuccessNotification("Thêm user mới thành công.");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Thêm user mới thất bại");
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return RedirectToAction("List");
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            try
            {
                var user = _userService.GetUserById(model.Id);
                if (user == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.UpdatedBy = currentUser.Id;
                model.UpdatedDateTime = DateTime.Now;
                _userService.UpdateUser(model);
                SuccessNotification("Cập nhật thông tin user thành công!");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Cập nhật user thất bại");
                return View(model);
            }
        }

        public JsonResult GetAllAdminUser()
        {
            var users = _userService.GetAllUser();
            return this.Json(users, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllGroupUsers()
        {
            var groupUsers = _groupUserService.GetAllGroupUsers();
            return Json(groupUsers, JsonRequestBehavior.AllowGet);
        }
    }
}