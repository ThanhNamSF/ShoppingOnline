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

        [StringLength(255)]
        public string WelcomeImagePath { get; set; }

        public string Description { get; set; }
        [StringLength(255)]
        public string WhoWeAreImagePath { get; set; }
        public string Information { get; set; }
        public string Quality { get; set; }
        public string Service { get; set; }
        public string Support { get; set; }
    }
}
