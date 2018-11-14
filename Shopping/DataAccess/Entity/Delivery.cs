using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        [StringLength(30)]
        public string CustomerName { get; set; }

        [StringLength(255)]
        public string CustomerAddress { get; set; }

        [StringLength(12)]
        public string CustomerPhone { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey("Approver")]
        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual User Approver { get; set; }

        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; }

        public bool Status { get; set; }

    }
}
