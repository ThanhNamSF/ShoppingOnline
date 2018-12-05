using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Constants;
using Common.Models;
using Common.SearchConditions;
using DataAccess.Interfaces;
using DataAccess.Models;
using Shopping.Helpers;

namespace Shopping.Areas.Admin.Controllers
{
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IProductService _productService;
        private readonly ICodeGeneratingService _codeGeneratingService;

        public InvoiceController(IInvoiceService invoiceService, IProductService productService, ICodeGeneratingService codeGeneratingService)
        {
            _invoiceService = invoiceService;
            _productService = productService;
            _codeGeneratingService = codeGeneratingService;
        }
        // GET: Admin/invoice
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        #region invoice CRUD

        public ActionResult List()
        {
            var model = new InvoiceSearchCondition();
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult List(DataSourceRequest command, InvoiceSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var invoices = _invoiceService.SearchInvoices(condition);
            var gridModel = new DataSourceResult()
            {
                Data = invoices.DataSource,
                Total = invoices.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            var model = _invoiceService.CreateNewInvoice(currentUser.Id);
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create(InvoiceModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                _invoiceService.InsertInvoice(model);
                SuccessNotification("Add new invoice successfully");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Add new invoice failed");
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var invoice = _invoiceService.GetInvoiceById(id);
            if (invoice == null)
                return RedirectToAction("List");
            return View(invoice);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(InvoiceModel model)
        {
            try
            {
                var invoice = _invoiceService.GetInvoiceById(model.Id);
                if (invoice == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.UpdatedBy = currentUser.Id;
                model.UpdatedDateTime = DateTime.Now;
                _invoiceService.UpdateInvoice(model);
                SuccessNotification("Update invoice information successfully");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Update invoice information failed");
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var invoice = _invoiceService.GetInvoiceById(id);
                if (invoice == null)
                    return RedirectToAction("List");
                _invoiceService.DeleteInvoice(id);
                SuccessNotification("Delete invoice successfully");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Delete invoice failed");
                return RedirectToAction("List");
            }
        }
        #endregion

        #region Popup Add Product

        [HttpPost]
        public ActionResult SearchProducts(DataSourceRequest command, ProductSearchCondition condition)
        {
            condition.PageNumber = command.Page - 1;
            condition.PageSize = command.PageSize;
            condition.DateFrom = DateTime.MinValue;
            condition.DateTo = DateTime.Now;
            var products = _productService.SearchProducts(condition);
            var gridModel = new DataSourceResult()
            {
                Data = products.DataSource,
                Total = products.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult ProductAddPopup(int invoiceId)
        {
            var model = new ProductSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAddPopup(string btnId, string formId, int invoiceId, ProductListSelected request)
        {
            if (request.ProductIds != null)
            {
                if (!string.IsNullOrEmpty(request.ProductIds))
                {
                    var productIds = request.ProductIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var productId in productIds)
                    {
                        var id = Int32.Parse(productId);
                        var isExisted = _invoiceService.CheckProductExistedInInvoice(invoiceId, id);
                        if (isExisted)
                            continue;
                        var product = _productService.GetProductById(id);
                        var invoiceDetailModel = new InvoiceDetailModel()
                        {
                            Quantity = 1,
                            ProductId = id,
                            InvoiceId = invoiceId,
                            UnitPrice = product.Price
                        };
                        _invoiceService.InsertInvoiceDetail(invoiceDetailModel);
                    }
                }

            }

            return RedirectToAction("Edit", new { id = invoiceId });
        }
        #endregion

        #region invoiceDetail

        [HttpPost]
        public ActionResult invoiceDetailList(DataSourceRequest command, InvoiceDetailSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;
            var invoice = _invoiceService.GetInvoiceById(condition.InvoiceId);
            PageList<InvoiceDetailModel> invoiceDetails;
            if (invoice.OrderId.HasValue)
            {
                condition.OrderId = invoice.OrderId.Value;
                invoiceDetails = _invoiceService.GetInvoiceDetailsByOrderId(condition);
            }
            else
            {
                invoiceDetails = _invoiceService.SearchInvoiceDetails(condition);
            }
            //invoiceDetails = _invoiceService.SearchInvoiceDetails(condition);
            invoiceDetails.DataSource.ForEach(x => x.Amount = x.UnitPrice * x.Quantity);
            var gridModel = new DataSourceResult
            {
                Data = invoiceDetails.DataSource,
                Total = invoiceDetails.TotalItems
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult AddinvoiceDetail(InvoiceDetailModel model)
        {
            var productModel = _productService.GetProductByCode(model.ProductCode);
            if (productModel == null)
            {
                return Json(new
                {
                    IsProductSkusNotFound = true
                });
            }

            if (_invoiceService.CheckProductExistedInInvoice(model.InvoiceId, productModel.Id))
            {
                return Json(new
                {
                    IsDupp = true
                });
            }

            model.ProductId = productModel.Id;
            model.Quantity = model.Quantity;
            _invoiceService.InsertInvoiceDetail(model);
            return Json(new { });
        }

        [HttpPost]
        public ActionResult invoiceDetailEdit(InvoiceDetailModel model)
        {
            var detail = _invoiceService.GetInvoiceDetailById(model.Id);
            if (detail == null)
                return Json(new DataSourceResult { Errors = "Product is not exist." });
            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            _invoiceService.UpdateInvoiceDetail(model);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult invoiceDetailDelete(int id)
        {
            var invoiceDetail = _invoiceService.GetInvoiceDetailById(id);
            if (invoiceDetail == null)
                return Json(null);

            _invoiceService.DeleteInvoiceDetail(id);
            return Json(null);
        }


        #endregion

        #region Approved/Open

        [HttpPost]
        public ActionResult Approve(int id)
        {
            var invoice = _invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                return RedirectToAction("Create");
            }

            if (!_invoiceService.HasInvoiceDetail(invoice.Id))
            {
                ErrorNotification("Invoice don't have any product. Please add product into invoice before approve!");
                return RedirectToAction("Edit", new { invoice.Id });
            }

            if (_invoiceService.CheckQuantityInInvoiceDetail(invoice.Id))
            {
                ErrorNotification("Product detail is invalid. Please check again!");
                return RedirectToAction("Edit", new { invoice.Id });
            }
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            invoice.ApprovedBy = currentUser.Id;
            invoice.Status = true;
            if (_invoiceService.Approved(invoice))
            {
                SuccessNotification("Approve invoice successfully.");
            }
            else
            {
                ErrorNotification("Approve invoice failed!");
            }
            
            return RedirectToAction("Edit", new { invoice.Id });
        }

        [HttpPost]
        public ActionResult Open(int id)
        {
            var invoice = _invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                return RedirectToAction("Create");
            }
            if (!_invoiceService.HasInvoiceDetail(invoice.Id))
            {
                ErrorNotification("Invoice don't have any product. Please add product into invoice before approve!");
                return RedirectToAction("Edit", new { invoice.Id });
            }
            if (_invoiceService.CheckQuantityInInvoiceDetail(invoice.Id))
            {
                ErrorNotification("Product detail is invalid. Please check again!");
                return RedirectToAction("Edit", new { invoice.Id });
            }

            invoice.ApprovedBy = null;
            invoice.ApprovedDateTime = null;
            invoice.Status = false;
            _invoiceService.Open(invoice);
            SuccessNotification("Open invoice successfully.");
            return RedirectToAction("Edit", new { invoice.Id });
        }

        #endregion
    }
}