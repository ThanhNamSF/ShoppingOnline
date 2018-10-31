using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataAccess.Models
{
    public class AboutModel
    {
        public int Id { get; set; }
        public string WelcomeImagePath { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string WhoWeAreImagePath { get; set; }
        [AllowHtml]
        public string Information { get; set; }
        [AllowHtml]
        public string Quality { get; set; }
        [AllowHtml]
        public string Service { get; set; }
        [AllowHtml]
        public string Support { get; set; }
        public bool Status { get; set; }
    }
}
