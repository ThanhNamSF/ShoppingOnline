using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Footer 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        public int Status { get; set; }
    }
}
