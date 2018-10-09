using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ProductModel
    {
        [Required(ErrorMessage = "Code is required")]
        [StringLength(6, ErrorMessage = "Max length is 6")]
        public string Code { get; set; }

        [StringLength(250, ErrorMessage = "Max length is 250")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Max length is 250")]
        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public float Promotion { get; set; }

        public int Quantity { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
        public int UpdatedBy { get; set; }

        public bool Status { get; set; }

        public int ProductCategoryId { get; set; }
    }
}
