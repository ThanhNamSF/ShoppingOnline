using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Amount { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }
        public bool Status { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public DateTime? ReceivedDateTime { get; set; }
        public string PaymentId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool Canceled { get; set; }
        public int CustomerId { get; set; }
        public int? ApproverId { get; set; }
        public int? DeliverId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool ContinueEditing { get; set; }
        public bool IsHasInvoice { get; set; }
    }
}
