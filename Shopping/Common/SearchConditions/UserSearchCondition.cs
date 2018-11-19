using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchConditions
{
    public class UserSearchCondition
    {
        public UserSearchCondition()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }

        public string UserName { get; set; }
        public int GroupUserId { get; set; }
        public bool? Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
