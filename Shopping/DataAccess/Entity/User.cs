using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class User : BaseEntity
    {
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

        public int Role { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public  int? UpdatedBy { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
