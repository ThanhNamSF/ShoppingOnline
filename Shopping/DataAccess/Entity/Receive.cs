using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Receive
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        [Required]
        [StringLength(250)]
        public string ReceiveFrom { get; set; }

        [StringLength(50)]
        public string Deliver { get; set; }

        [StringLength(50)]
        public string Driver { get; set; }

        [StringLength(32)]
        public string CarNumber { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public DateTime DocumentDateTime { get; set; }

        [ForeignKey("Creator")]
        public int? CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("Updater")]
        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey("Approver")]
        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public virtual User Approver { get; set; }

        public virtual User Creator { get; set; }

        public virtual User Updater { get; set; }

        public virtual ICollection<ReceiveDetail> ReceiveDetails { get; set; }

        public bool Status { get; set; }
    }
}
