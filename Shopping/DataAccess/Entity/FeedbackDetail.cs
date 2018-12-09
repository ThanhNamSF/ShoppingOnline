using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class FeedbackDetail
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FeedbackId { get; set; }

        public string ReplyContent { get; set; }

        public DateTime RepliedDateTime { get; set; }

        public virtual User User { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}
