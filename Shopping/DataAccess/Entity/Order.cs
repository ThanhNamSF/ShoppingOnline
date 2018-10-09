using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Order : BaseEntity
    {
        public double Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverName { get; set; }

        [Required]
        public string ReceiverAddress { get; set; }

        [Required]
        [StringLength(12)]
        public string ReceiverPhone { get; set; }

        public bool Status { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public DateTime? ReceivedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Approver")]
        public int? ApprovedId { get; set; }
        [ForeignKey("Deliver")]
        public int? DiliveredId { get; set; }

        public virtual User User { get; set; }

        public virtual User Approver { get; set; }
        public virtual User Deliver { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
