using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Contact : BaseEntity
    {
        public string Content { get; set; }

        public bool Status { get; set; }
    }
}
