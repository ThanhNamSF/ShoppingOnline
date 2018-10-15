using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Models;

namespace Shopping.Models
{
    public class CartItem
    {
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }
    }
}