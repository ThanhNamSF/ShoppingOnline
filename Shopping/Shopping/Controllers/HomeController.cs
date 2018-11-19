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
        private readonly IAboutService _aboutService;

        public HomeController(IProductCategoryService productCategoryService, ISlideService slideService, IAboutService aboutService)
        {
            _productCategoryService = productCategoryService;
            _slideService = slideService;
            _aboutService = aboutService;
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
            var model = _aboutService.GetAboutUs();
            return View(model);
        }


    }
}