using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.SearchConditions;
using DataAccess.Interfaces;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: Product
        public ActionResult Index()
        {
            return RedirectToAction("List", new {categoryId = 1});
        }

        public ActionResult List(int categoryId)
        {
            var products = _productService.SearchProducts(new ProductSearchCondition()
            {
                CategoryId = categoryId,
                DateFrom = DateTime.MinValue,
                DateTo = DateTime.Now,
                PageSize = 9,
                PageNumber = 0,
            });
            return View(products);
        }

        public ActionResult Detail(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return View("List");
            var otherProducts = _productService.GetOthersProductByCategoryId(product.Id, product.ProductCategoryId, 4);
            var model = new ProductDetailModel()
            {
                Product = product,
                OtherProducts = otherProducts
            };
            return View(model);
        }
    }
}