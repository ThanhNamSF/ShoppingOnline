using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [StringLength(250)]
        public string Link { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public bool Status { get; set; }
    }
}
