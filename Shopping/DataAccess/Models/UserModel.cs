using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(32, ErrorMessage = "UserName cannot be greater than 32")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(16, ErrorMessage = "Password cannot be greater than 16")]
        public string Password { get; set; }

        [StringLength(16, ErrorMessage = "Password cannot be greater than 16")]
        public string PasswordConfirm { get; set; }

        [StringLength(50, ErrorMessage = "FirstName cannot be greater than 50")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "LastName cannot be greater than 50")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "Email cannot be greater than 50")]
        public string Email { get; set; }

        [StringLength(12, ErrorMessage = "Phone cannot be greater than 50")]
        public string Phone { get; set; }

        public int Role { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
