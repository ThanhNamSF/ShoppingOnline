using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class CodeGenerating
    {
        [Key]
        public int Id { get; set; }

        [StringLength(6)]
        [Required]
        public string Prefix { get; set; }

        [DataType("date")]
        public DateTime LastGeneratedDateTime { get; set; }

        public int GeneratingNumber { get; set; }
    }
}
