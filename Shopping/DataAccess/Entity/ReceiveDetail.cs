using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class ReceiveDetail : BaseEntity
    {
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public float DiscountRate { get; set; }
        public float VatRate { get; set; }
        public DateTime? ExpiryDateTime { get; set; }
        public int ReceiveId { get; set; }
        public int ProductId { get; set; }
        public virtual Receive Receive { get; set; }
        public virtual Product Product { get; set; }
    }
}
