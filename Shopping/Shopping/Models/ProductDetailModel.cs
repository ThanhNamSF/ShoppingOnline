using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Models;

namespace Shopping.Models
{
    public class ProductDetailModel
    {
        public ProductModel Product { get; set; }
        public IEnumerable<ProductModel> OtherProducts { get; set; }
    }
}