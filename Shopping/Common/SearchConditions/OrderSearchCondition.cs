using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchConditions
{
    public class OrderSearchCondition
    {
        public OrderSearchCondition()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }
        public bool? ApprovedStatus { get; set; }
        public bool? DeliveredStatus { get; set; }
        public bool? AsignmentStatus { get; set; }
        public bool? Canceled { get; set; }
        public string ReceiverName { get; set; }
        public string OrderCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
