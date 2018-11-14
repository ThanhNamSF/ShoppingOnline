using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Constants;
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
            var condition = new ProductClientSearchCondition()
            {
                ProductCategoryId = categoryId,
                PageSize = Values.ProductClientPageSize,
                PageNumber = 0,
            };
            var products = _productService.SearchProducts(condition);
            var bestSellerProduct = _productService.GetHostestProducts(Values.BestSellerNumber);
            var model = new ProductClientModel()
            {
                Products = products,
                SearchCondition = condition,
                BestSellerProducts = bestSellerProduct
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult List(ProductClientSearchCondition condition)
        {
            var splits = Regex.Split(condition.Range, " - ");
            condition.MinPrice = GetPrice(splits[0]);
            condition.MaxPrice = GetPrice(splits[1]);
            condition.PageSize = Values.ProductClientPageSize;
            condition.PageNumber = 0;
            var products = _productService.SearchProducts(condition);
            var bestSellerProduct = _productService.GetHostestProducts(Values.BestSellerNumber);
            var model = new ProductClientModel()
            {
                Products = products,
                BestSellerProducts = bestSellerProduct,
                SearchCondition = condition
            };
            return View(model);
        }

        private double GetPrice(string value)
        {
            return Double.Parse(value.Remove(0, 1));
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