using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class CodeGeneratingModel
    {
        public int Id { get; set; }

        public string Prefix { get; set; }

        public DateTime LastGeneratedDateTime { get; set; }

        public int GeneratingNumber { get; set; }
    }
}
