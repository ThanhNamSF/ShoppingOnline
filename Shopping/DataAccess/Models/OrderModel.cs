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
        public double Amount { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverAddress { get; set; }

        public string ReceiverPhone { get; set; }

        public bool Status { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public DateTime? ReceivedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int UserId { get; set; }
        public int? ApprovedId { get; set; }
        public int? DiliveredId { get; set; }
    }
}
