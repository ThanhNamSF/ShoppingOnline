using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;

namespace DataAccess.Models
{
    public class DeliveryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Mã phiếu không được bỏ trống!")]
        [StringLength(30, ErrorMessage = "Mã phiếu không được vượt quá 30 ký tự")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Nơi xuất bán không được để trống")]
        [StringLength(250, ErrorMessage = "Nới xuất bán không được vượt quá 250 ký tự")]
        public string DeliveryTo { get; set; }

        [StringLength(50, ErrorMessage = "Tên người vận chuyển không được vượt quá 50 ký tự")]
        public string Deliver { get; set; }

        [StringLength(50, ErrorMessage = "Tên tài xế không được vượt quá 50 ký tự")]
        public string Driver { get; set; }

        [StringLength(32, ErrorMessage = "Biển số xe không được vượt quá 32 ký tự")]
        public string CarNumber { get; set; }

        [StringLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự")]
        public string Description { get; set; }

        [StringLength(30, ErrorMessage = "Số hóa đơn không được vượt quá 30 ký tự")]
        public string InvoiveNo { get; set; }

        public DateTime DocumentDateTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public bool Status { get; set; }

        public bool ContinueEditing { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
