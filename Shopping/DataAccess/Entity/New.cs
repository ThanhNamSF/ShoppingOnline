using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class New
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Detail { get; set; }
        
        public DateTime CreatedDateTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public bool Status { get; set; }

        public int NewCategoryId { get; set; }

        public virtual NewCategory NewCategory { get; set; }

    }
}
