using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Constants;
using DataAccess.Interfaces;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[Values.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = cart as List<CartItem>;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult AddItem(int productId, int quantity)
        {
            var cart = Session[Values.CartSession];
            if (cart != null)
            {
                var list = cart as List<CartItem>;
                var itemExisted = list.FirstOrDefault(x => x.Product.Id == productId);
                if (itemExisted != null)
                {
                    itemExisted.Quantity += quantity;
                }
                else
                {
                    var product = _productService.GetProductById(productId);
                    var item = new CartItem()
                    {
                        Quantity = quantity,
                        Product = product
                    };
                    list.Add(item);
                }

                Session[Values.CartSession] = list;
            }
            else
            {
                var product = _productService.GetProductById(productId);
                var list = new List<CartItem>();
                var item = new CartItem()
                {
                    Quantity = quantity,
                    Product = product
                };
                list.Add(item);

                Session[Values.CartSession] = list;
            }

            return RedirectToAction("Index");
        }
    }
}