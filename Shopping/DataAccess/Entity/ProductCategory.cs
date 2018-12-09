using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        [StringLength(250)]
        public string SeoTitle { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("Creator")]
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey("Updater")]
        public int? UpdatedBy { get; set; }

        public bool Status { get; set; }

        public virtual User Creator { get; set; }

        public virtual User Updater { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        


    }
}
