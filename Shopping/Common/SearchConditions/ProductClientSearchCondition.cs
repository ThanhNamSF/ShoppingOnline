using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchConditions
{
    public class ProductClientSearchCondition
    {
        public ProductClientSearchCondition()
        {
            Top = Trend.All.ToString();
            MinPrice = 50;
            MaxPrice = 6000;
        }
        public int ProductCategoryId { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Range { get; set; }
        public double Discount { get; set; }
        public string Top { get; set; }
        public string Name { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
