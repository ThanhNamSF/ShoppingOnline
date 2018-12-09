using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Slide
    {
        [Key]
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Title { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("Creator")]
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey("Updater")]
        public int? UpdatedBy { get; set; }

        public virtual User Creator { get; set; }

        public virtual User Updater { get; set; }

        public bool Status { get; set; }
    }
}
