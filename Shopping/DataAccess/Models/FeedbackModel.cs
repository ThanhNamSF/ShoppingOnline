using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataAccess.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ReplierId { get; set; }
        [StringLength(255)]
        [Required]
        public string Title { get; set; }
        [StringLength(255)]
        [Required]
        public string Content { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public string FullName { get; set; }
        public string CustomerUserName { get; set; }
        public string ReplierUserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [AllowHtml]
        [Required]
        public string ReplyContent { get; set; }
        public DateTime RepliedDateTime { get; set; }
        public bool ContinueEditing { get; set; }
    }
}
