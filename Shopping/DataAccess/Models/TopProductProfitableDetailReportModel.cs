using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class TopProductProfitableDetailReportModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public double ReceivePriceAverage { get; set; }
        public double SellPriceAverage { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Profitable { get; set; }
    }
}
