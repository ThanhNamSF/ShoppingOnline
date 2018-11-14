using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Interfaces;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly ISlideService _slideService;

        public HomeController(IProductCategoryService productCategoryService, ISlideService slideService)
        {
            _productCategoryService = productCategoryService;
            _slideService = slideService;
        }
        public ActionResult Index()
        {
            var productCategories = _productCategoryService.GetAllProductCategories();
            var slides = _slideService.GetSlideForDisplay();
            var model = new HomeModel()
            {
                Slides = slides,
                ProductCategories = productCategories
            };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}