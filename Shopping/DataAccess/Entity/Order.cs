using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        public double Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverName { get; set; }

        [Required]
        public string ReceiverAddress { get; set; }

        [Required]
        [StringLength(12)]
        public string ReceiverPhone { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public DateTime? ReceivedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        [Required]
        public string PaymentId { get; set; }

        [ForeignKey("Canceller")]
        public int? CanceledBy { get; set; }

        [ForeignKey("Updater")]
        public int? UpdatedBy { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("Approver")]
        public int? ApproverId { get; set; }

        [ForeignKey("Deliver")]
        public int? DeliverId { get; set; }

        [ForeignKey("Closer")]
        public int? ClosedBy { get; set; }

        public bool IsHasInvoice { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual User Updater { get; set; }

        public virtual User Approver { get; set; }

        public virtual User Deliver { get; set; }

        public virtual User Closer { get; set; }

        public virtual User Canceller { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
