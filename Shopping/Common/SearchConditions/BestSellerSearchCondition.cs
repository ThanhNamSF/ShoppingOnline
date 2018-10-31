using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchConditions
{
    public class BestSellerSearchCondition
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CreatedBy { get; set; }
        public int TopNumber { get; set; }
    }
}
