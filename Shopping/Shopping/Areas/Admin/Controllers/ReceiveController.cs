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
    public class ReceiveController : BaseController
    {
        private readonly IReceiveService _receiveService;
        private readonly IProductService _productService;

        public ReceiveController(IReceiveService receiveService, IProductService productService)
        {
            _receiveService = receiveService;
            _productService = productService;
        }
        // GET: Admin/Receive
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        #region Receive CRUD

        public ActionResult List()
        {
            var model = new ReceiveSearchCondition();
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult List(DataSourceRequest command, ReceiveSearchCondition condition)
        {
            condition.PageSize = command.PageSize;
            condition.PageNumber = command.Page - 1;

            var receives = _receiveService.SearchReceives(condition);
            var gridModel = new DataSourceResult()
            {
                Data = receives.DataSource,
                Total = receives.TotalItems
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            var model = _receiveService.CreateNewReceive(currentUser.Id);
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create(ReceiveModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                if (_receiveService.IsReceiveCodeExisted(model.Code))
                {
                    ErrorNotification("Thêm mới phiếu nhập thất bại. Mã phiếu đã tồn tại");
                    return View(model);
                }
                _receiveService.InsertReceive(model);
                SuccessNotification("Thêm mới phiếu nhập thành công");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Thêm mới phiếu nhập thất bại");
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var receive = _receiveService.GetReceiveById(id);
            if (receive == null)
                return RedirectToAction("List");
            return View(receive);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(ReceiveModel model)
        {
            try
            {
                var receive = _receiveService.GetReceiveById(model.Id);
                if (receive == null)
                    return RedirectToAction("List");
                if (!ModelState.IsValid)
                    return View(model);
                var currentUser = Session[Values.USER_SESSION] as UserModel;
                model.UpdatedBy = currentUser.Id;
                model.UpdatedDateTime = DateTime.Now;
                _receiveService.UpdateReceive(model);
                SuccessNotification("Chỉnh sửa thông tin phiếu nhập thành công");
                return model.ContinueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Chỉnh sửa phiếu nhập thất bại");
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var receive = _receiveService.GetReceiveById(id);
                if (receive == null)
                    return RedirectToAction("List");
                _receiveService.DeleteReceive(id);
                SuccessNotification("Xóa phiếu nhập thành công");
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                ErrorNotification("Xóa phiếu nhập thất bại");
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

        public ActionResult ProductAddPopup(int receiveId)
        {
            var model = new ProductSearchCondition();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAddPopup(string btnId, string formId, int receiveId, ProductListSelected request)
        {
            if (request.ProductIds != null)
            {
                if (!string.IsNullOrEmpty(request.ProductIds))
                {
                    var productIds = request.ProductIds.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var productId in productIds)
                    {
                        var id = Int32.Parse(productId);
                        var isExisted = _receiveService.CheckProductExistedInReceive(receiveId, id);
                        if(isExisted)
                            continue;
                        var receiveDetailModel = new ReceiveDetailModel()
                        {
                            Quantity = 1,
                            VatRate = 0,
                            DiscountRate = 0,
                            ProductId = id,
                            ReceiveId = receiveId,
                            UnitPrice = 0
                        };
                        _receiveService.InsertReceiveDetail(receiveDetailModel);
                    }
                }
                
            }

            return RedirectToAction("Edit", new {id = receiveId});
        }
        #endregion

        #region ReceiveDetail

        [HttpPost]
        public ActionResult ReceiveDetailList(DataSourceRequest command, ReceiveDetailSearchCondition condition)
        {
            condition.PageNumber = command.Page - 1;
            condition.PageSize = command.PageSize;
            var receiveDetails = _receiveService.SearchReceiveDetails(condition);
            receiveDetails.DataSource.ForEach(x => CalculateAmount(x));
            var gridModel = new DataSourceResult
            {
                Data = receiveDetails.DataSource,
                Total = receiveDetails.TotalItems
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult AddReceiveDetail(ReceiveDetailModel model)
        {
            var productModel = _productService.GetProductByCode(model.ProductCode);
            if (productModel == null)
            {
                return Json(new
                {
                    IsProductSkusNotFound = true
                });
            }

            if (_receiveService.CheckProductExistedInReceive(model.ReceiveId, productModel.Id))
            {
                return Json(new
                {
                    IsDupp = true
                });
            }

            model.ProductId = productModel.Id;
            model.Quantity = model.Quantity;
            _receiveService.InsertReceiveDetail(model);
            return Json(new { });
        }

        private void CalculateAmount(ReceiveDetailModel receiveDetailModel)
        {
            receiveDetailModel.SubAmount = receiveDetailModel.UnitPrice * receiveDetailModel.Quantity;
            receiveDetailModel.DiscountAmount = receiveDetailModel.SubAmount * receiveDetailModel.DiscountRate / 100;
            receiveDetailModel.VatAmount = (receiveDetailModel.SubAmount - receiveDetailModel.DiscountAmount) *
                                           receiveDetailModel.VatRate / 100;
            receiveDetailModel.Amount = receiveDetailModel.SubAmount - receiveDetailModel.DiscountAmount +
                                        receiveDetailModel.VatAmount;
        }

        [HttpPost]
        public ActionResult ReceiveDetailEdit(ReceiveDetailModel model)
        {
            var detail = _receiveService.GetReceiveDetailById(model.Id);
            if (detail == null)
                return Json(new DataSourceResult { Errors = "Sản phẩm không tồn tại" });
            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            _receiveService.UpdateReceiveDetail(model);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ReceiveDetailDelete(int id)
        {
            var receiveDetail = _receiveService.GetReceiveDetailById(id);
            if (receiveDetail == null)
                return Json(null);

            _receiveService.DeleteReceiveDetail(id);
            return Json(null);
        }


        #endregion

        #region Approved/Open

        [HttpPost]
        public ActionResult Approve(int id)
        {
            var receive = _receiveService.GetReceiveById(id);
            if (receive == null)
            {
                return RedirectToAction("Create");
            }

            if (!_receiveService.HasReceiveDetail(receive.Id))
            {
                ErrorNotification("Phiếu nhập chưa có chi tiết. Vui lòng thêm sản phẩm vào chi tiết phiếu nhập trước khi duyệt !");
                return RedirectToAction("Edit", new { receive.Id });
            }

            if (_receiveService.CheckQuantityInReceiveDetail(receive.Id))
            {
                ErrorNotification("Chi tiết sản phẩm không hợp lệ. Vui lòng kiểm tra lại!");
                return RedirectToAction("Edit", new { receive.Id });
            }
            var currentUser = Session[Values.USER_SESSION] as UserModel;
            receive.ApprovedBy = currentUser.Id;
            receive.Status = true;
            _receiveService.Approved(receive);
            SuccessNotification("Duyệt phiếu nhập thành công.");
            return RedirectToAction("Edit", new { receive.Id });
        }

        [HttpPost]
        public ActionResult Open(int id)
        {
            var receive = _receiveService.GetReceiveById(id);
            if (receive == null)
            {
                return RedirectToAction("Create");
            }
            if (!_receiveService.HasReceiveDetail(receive.Id))
            {
                ErrorNotification("Phiếu nhập chưa có chi tiết. Vui lòng thêm sản phẩm vào chi tiết phiếu nhập trước khi duyệt !");
                return RedirectToAction("Edit", new { receive.Id });
            }
            if (_receiveService.CheckQuantityInReceiveDetail(receive.Id))
            {
                ErrorNotification("Chi tiết sản phẩm không hợp lệ. Vui lòng kiểm tra lại!");
                return RedirectToAction("Edit", new { receive.Id });
            }

            receive.ApprovedBy = null;
            receive.ApprovedDateTime = null;
            receive.Status = false;
            if (_receiveService.Open(receive))
            {
                SuccessNotification("Mở phiếu nhập thành công.");
            }
            else
            {
                ErrorNotification("Mở phiếu nhập thất bại!");
            }
            
            return RedirectToAction("Edit", new { receive.Id });
        }

        #endregion


    }
}