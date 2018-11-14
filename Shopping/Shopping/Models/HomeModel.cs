using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Models;

namespace Shopping.Models
{
    public class HomeModel
    {
        public IEnumerable<ProductCategoryModel> ProductCategories { get; set; }
        public IEnumerable<SlideModel> Slides { get; set; }
    }
}