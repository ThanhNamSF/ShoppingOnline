using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(12)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public float Promotion { get; set; }

        public int Quantity { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedDateTime { get; set; }
        [ForeignKey("Creator")]
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
        [ForeignKey("Updater")]
        public int? UpdatedBy { get; set; }

        public bool Status { get; set; }

        public string ImagePath { get; set; }

        public string FrontImagePath { get; set; }

        public string BackImagePath { get; set; }

        public int ProductCategoryId { get; set; }

        public virtual User Creator { get; set; }
        public virtual User Updater { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        
    }
}
