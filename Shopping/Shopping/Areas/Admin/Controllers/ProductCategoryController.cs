using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System.Net;
using System.Web.Http;
using DataAccess.Interfaces;

namespace Shopping.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public JsonResult GetAllProductCategories()
        {
            var productCategories = _productCategoryService.GetAllProductCategories();
            return this.Json(productCategories, JsonRequestBehavior.AllowGet);
        }
    }
}