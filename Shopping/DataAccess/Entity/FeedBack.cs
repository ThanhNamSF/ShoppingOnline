using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<FeedbackGroup> FeedbackGroups { get; set; }
    }
}
