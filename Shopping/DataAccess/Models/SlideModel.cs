using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class SlideModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Status { get; set; }
        public bool ContinueEditing { get; set; }
    }
}
