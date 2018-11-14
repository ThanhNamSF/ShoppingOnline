using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Common.Constants;
using DataAccess.Interfaces;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class CartController : BaseController
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

        [HttpPost]
        public JsonResult UpdateQuantity(int productId, int quantity)
        {
            var cart = Session[Values.CartSession];
            var status = false;
            double amountTotal = 0;
            double amount = 0;
            if (cart != null)
            {
                var list = cart as List<CartItem>;
                var item = list.FirstOrDefault(x => x.Product.Id == productId);
                if (item.Quantity + quantity > 0)
                {
                    item.Quantity += quantity;
                    amount = item.Product.Price * (1 - item.Product.Promotion) * item.Quantity;
                    amountTotal = list.Sum(i => i.Quantity * i.Product.Price * (1 - i.Product.Promotion));
                    Session[Values.CartSession] = list;
                    status = true;
                } 
            }

            return Json(new
            {
                status = status,
                amountTotal = string.Format("{0:#,#}", amountTotal),
                amount = string.Format("{0:#,#}", amount)
            });
        }

        [HttpPost]
        public JsonResult DeleteProduct(int productId)
        {
            var cart = Session[Values.CartSession];
            var status = false;
            double amountTotal = 0;
            int totalProduct = 0;
            if (cart != null)
            {
                var list = cart as List<CartItem>;
                var item = list.FirstOrDefault(x => x.Product.Id == productId);
                list.Remove(item);
                amountTotal = list.Sum(i => i.Quantity * i.Product.Price * (1 - i.Product.Promotion));
                totalProduct = list.Count;
                Session[Values.CartSession] = list;
                status = true;
            }
            return Json(new
            {
                status = status,
                amountTotal = string.Format("{0:#,#}", amountTotal),
                totalProduct = totalProduct
            });
        }
    }
}