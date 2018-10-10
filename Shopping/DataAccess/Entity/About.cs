using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public bool Status { get; set; }
    }
}
