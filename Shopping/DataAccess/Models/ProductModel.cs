using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataAccess.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Mã sản phẩm không được bỏ trống")]
        [StringLength(30, ErrorMessage = "Mã sản phẩm không vượt quá 30 ký tự")]
        public string Code { get; set; }

        [StringLength(250, ErrorMessage = "Tên sản phẩm không vượt quá 250 ký tự")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Tiêu đề sản phẩm không vượt quá 250 ký tự")]
        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public float Promotion { get; set; }

        public int Quantity { get; set; }

        [AllowHtml]
        public string Detail { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
        public int UpdatedBy { get; set; }

        public bool Status { get; set; }

        public bool ContinueEditing { get; set; }

        public int ProductCategoryId { get; set; }
        
        public string Trend { get; set; }
    }
}
