using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class DeliveryDetailModel
    {
        public int Id { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public float DiscountRate { get; set; }
        public float VatRate { get; set; }
        public double SubAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double VatAmount { get; set; }
        public double Amount { get; set; }
        public DateTime? ExpiryDateTime { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
    }
}
