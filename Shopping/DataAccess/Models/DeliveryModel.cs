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

        [StringLength(30, ErrorMessage = "Tên khách hàng không được vượt quá 30 ký tự")]
        public string CustomerName { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 30 ký tự")]
        public string CustomerAddress { get; set; }

        [StringLength(12, ErrorMessage = "Số điện thoại không được vượt quá 30 ký tự")]
        public string CustomerPhone { get; set; }

        [StringLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự")]
        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDateTime { get; set; }

        public int? OrderId { get; set; }

        public bool Status { get; set; }

        public bool ContinueEditing { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
