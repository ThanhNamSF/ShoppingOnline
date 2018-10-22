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

        public bool Status { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public DateTime? ReceivedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        [Required]
        public string PaymentId { get; set; }

        public bool Canceled { get; set; }

        public int? UpdatedBy { get; set; }
        public int UserId { get; set; }
        
        public int? ApprovedId { get; set; }
        
        public int? DeliveredId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("ApprovedId")]
        public virtual User Approver { get; set; }
        [ForeignKey("DeliveredId")]
        public virtual User Deliver { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
