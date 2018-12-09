using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32)]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required]
        [StringLength(16)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string Address { get; set; }
        
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(12)]
        public string Phone { get; set; }

        public int GroupUserId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("Creator")]
        public int? CreatedBy { get; set; }

        public bool Status { get; set; }

        public virtual User Creator { get; set; }

        public virtual GroupUser GroupUser { get; set; }

        public virtual ICollection<FeedbackDetail> FeedbackDetails { get; set; }

    }
}
