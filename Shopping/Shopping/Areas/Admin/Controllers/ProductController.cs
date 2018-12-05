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
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: Admin/Product
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new ProductSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, ProductSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var products = _productService.SearchProducts(condition);
            var gridModel = new DataSourceResult()
            {
                Data = products.DataSource,
                Total = products.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            var model = new ProductModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProductModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                if (_productService.IsProductCodeExisted(model.Code))
                {
                    ErrorNotification("Add new product failed.Product code is existed.");
                    return View(model);
                }
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.CreatedBy = currentUser.Id;
                model.CreatedDateTime = DateTime.Now;
                _productService.InsertProduct(model);
                SuccessNotification("Add new product successfully.");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Add new product failed");
                return View(model);
            }  
        }

        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return RedirectToAction("List");
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(ProductModel model)
        {
            try
            {
                var product = _productService.GetProductById(model.Id);
                if (product == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.UpdatedBy = currentUser.Id;
                model.UpdatedDateTime = DateTime.Now;
                _productService.UpdateProduct(model);
                SuccessNotification("Update product information successfully!");
                return model.ContinueEditing ? RedirectToAction("Edit", new {id = model.Id}) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Update product failed");
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                if (product == null)
                    return RedirectToAction("List");
                if (_productService.IsProductExistedInOrderInvoiceReceive(product.Id))
                {
                    ErrorNotification("Delete product failed. This product already exists in order or invoice or receive.");
                    return RedirectToAction("List");
                }
                _productService.DeleteProduct(id);
                SuccessNotification("Delete product successfully.");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Delete product failed");
                return RedirectToAction("List");
            }
        }
    }
}