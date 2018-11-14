using Common;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.SearchConditions;

namespace Shopping.Models
{
    public class ProductClientModel
    {
        public PageList<ProductModel> Products { get; set; }
        public IEnumerable<ProductModel> BestSellerProducts { get; set; }
        public ProductClientSearchCondition SearchCondition { get; set; }
    }
}