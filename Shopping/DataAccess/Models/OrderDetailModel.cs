using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class OrderDetailModel
    {
        public int Id { get; set; }
        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }

        public double SubAmount { get; set; }
    }
}
