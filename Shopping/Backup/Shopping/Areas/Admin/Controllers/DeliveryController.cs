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
    public class DeliveryController : BaseController
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IProductService _productService;

        public DeliveryController(IDeliveryService deliveryService, IProductService productService)
        {
            _deliveryService = deliveryService;
            _productService = productService;
        }
        // GET: Admin/delivery
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        #region delivery CRUD

        public ActionResult List()
        {
            var model = new DeliverySearchCondition();
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult List(DataSourceRequest command, DeliverySearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var deliveries = _deliveryService.SearchDeliveries(condition);
            var gridModel = new DataSourceResult()
            {
                Data = deliveries.DataSource,
                Total = deliveries.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            var model = new DeliveryModel();
            model.DocumentDateTime = DateTime.Now;
            model.CreatedDateTime = DateTime.Now;
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create(DeliveryModel model)
        {
            try
            {
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.CreatedBy = currentUser.Id;
                model.CreatedDateTime = DateTime.Now;
                if (!ModelState.IsValid)
                    return View(model);
                _deliveryService.InsertDelivery(model);
                SuccessNotification("Thêm mới phiếu xuất thành công");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Thêm mới phiếu xuất thất bại");
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var delivery = _deliveryService.GetDeliveryById(id);
            if (delivery == null)
                return RedirectToAction("List");
            return View(delivery);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(DeliveryModel model)
        {
            try
            {
                var delivery = _deliveryService.GetDeliveryById(model.Id);
                if (delivery == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.UpdatedBy = currentUser.Id;
                model.UpdatedDateTime = DateTime.Now;
                _deliveryService.UpdateDelivery(model);
                SuccessNotification("Chỉnh sửa thông tin phiếu xuất thành công");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Chỉnh sửa phiếu xuất thất bại");
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var delivery = _deliveryService.GetDeliveryById(id);
                if (delivery == null)
                    return RedirectToAction("List");
                _deliveryService.DeleteDelivery(id);
                SuccessNotification("Xóa phiếu xuất thành công");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Xóa phiếu xuất thất bại");
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

        public ActionResult ProductAddPopup(int deliveryId)
        {
            var model = new ProductSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAddPopup(string btnId, string formId, int deliveryId, ProductListSelected request)
        {
            if (request.ProductIds != null)
            {
                if (!string.IsNullOrEmpty(request.ProductIds))
                {
                    var productIds = request.ProductIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var productId in productIds)
                    {
                        var id = Int32.Parse(productId);
                        var isExisted = _deliveryService.CheckProductExistedInDelivery(deliveryId, id);
                        if (isExisted)
                            continue;
                        var deliveryDetailModel = new DeliveryDetailModel()
                        {
                            Quantity = 1,
                            VatRate = 0,
                            DiscountRate = 0,
                            ProductId = id,
                            DeliveryId = deliveryId,
                            UnitPrice = 0
                        };
                        _deliveryService.InsertDeliveryDetail(deliveryDetailModel);
                    }
                }

            }

            return RedirectToAction("Edit", new { id = deliveryId });
        }
        #endregion

        #region deliveryDetail

        [HttpPost]
        public ActionResult DeliveryDetailList(DataSourceRequest command, int deliveryId)
        {
            var deliveryDetails = _deliveryService.SearchDeliveryDetails(new DeliveryDetailSearchCondition()
            {
                DeliveryId = deliveryId,
                PageSize = command.PageSize,
                PageNumber = command.Page - 1
            });
            deliveryDetails.DataSource.ForEach(x => CalculateAmount(x));
            var gridModel = new DataSourceResult
            {
                Data = deliveryDetails.DataSource,
                Total = deliveryDetails.TotalItems
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult AddDeliveryDetail(DeliveryDetailModel model)
        {
            var productModel = _productService.GetProductByCode(model.ProductCode);
            if (productModel == null)
            {
                return Json(new
                {
                    IsProductSkusNotFound = true
                });
            }

            if (_deliveryService.CheckProductExistedInDelivery(model.DeliveryId, productModel.Id))
            {
                return Json(new
                {
                    IsDupp = true
                });
            }

            model.ProductId = productModel.Id;
            model.Quantity = model.Quantity;
            _deliveryService.InsertDeliveryDetail(model);
            return Json(new { });
        }

        private void CalculateAmount(DeliveryDetailModel deliveryDetailModel)
        {
            deliveryDetailModel.SubAmount = deliveryDetailModel.UnitPrice * deliveryDetailModel.Quantity;
            deliveryDetailModel.DiscountAmount = deliveryDetailModel.SubAmount * deliveryDetailModel.DiscountRate / 100;
            deliveryDetailModel.VatAmount = (deliveryDetailModel.SubAmount - deliveryDetailModel.DiscountAmount) *
                                           deliveryDetailModel.VatRate / 100;
            deliveryDetailModel.Amount = deliveryDetailModel.SubAmount - deliveryDetailModel.DiscountAmount +
                                        deliveryDetailModel.VatAmount;
        }

        [HttpPost]
        public ActionResult DeliveryDetailEdit(DeliveryDetailModel model)
        {
            var detail = _deliveryService.GetDeliveryDetailById(model.Id);
            if (detail == null)
                return Json(new DataSourceResult { Errors = "Sản phẩm không tồn tại" });
            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            _deliveryService.UpdateDeliveryDetail(model);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult DeliveryDetailDelete(int id)
        {
            var deliveryDetail = _deliveryService.GetDeliveryDetailById(id);
            if (deliveryDetail == null)
                return Json(null);

            _deliveryService.DeleteDeliveryDetail(id);
            return Json(null);
        }


        #endregion

        #region Approved/Open

        [HttpPost]
        public ActionResult Approve(int id)
        {
            var delivery = _deliveryService.GetDeliveryById(id);
            if (delivery == null)
            {
                return RedirectToAction("Create");
            }

            if (!_deliveryService.HasDeliveryDetail(delivery.Id))
            {
                ErrorNotification("Phiếu xuất chưa có chi tiết. Vui lòng thêm sản phẩm vào chi tiết phiếu xuất trước khi duyệt !");
                return RedirectToAction("Edit", new { delivery.Id });
            }

            if (_deliveryService.CheckQuantityInDeliveryDetail(delivery.Id))
            {
                ErrorNotification("Chi tiết sản phẩm không hợp lệ. Vui lòng kiểm tra lại!");
                return RedirectToAction("Edit", new { delivery.Id });
            }
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            delivery.ApprovedBy = currentUser.Id;
            delivery.Status = true;
            if (_deliveryService.Approved(delivery))
            {
                SuccessNotification("Duyệt phiếu xuất thành công.");
            }
            else
            {
                ErrorNotification("Duyệt phiếu xuất thất bại!");
            }
            
            return RedirectToAction("Edit", new { delivery.Id });
        }

        [HttpPost]
        public ActionResult Open(int id)
        {
            var delivery = _deliveryService.GetDeliveryById(id);
            if (delivery == null)
            {
                return RedirectToAction("Create");
            }
            if (!_deliveryService.HasDeliveryDetail(delivery.Id))
            {
                ErrorNotification("Phiếu xuất chưa có chi tiết. Vui lòng thêm sản phẩm vào chi tiết phiếu xuất trước khi duyệt !");
                return RedirectToAction("Edit", new { delivery.Id });
            }
            if (_deliveryService.CheckQuantityInDeliveryDetail(delivery.Id))
            {
                ErrorNotification("Chi tiết sản phẩm không hợp lệ. Vui lòng kiểm tra lại!");
                return RedirectToAction("Edit", new { delivery.Id });
            }

            delivery.ApprovedBy = null;
            delivery.ApprovedDateTime = null;
            delivery.Status = false;
            _deliveryService.Open(delivery);
            SuccessNotification("Mở phiếu xuất thành công.");
            return RedirectToAction("Edit", new { delivery.Id });
        }

        #endregion
    }
}