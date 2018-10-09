using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class FeedBack : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(12)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
