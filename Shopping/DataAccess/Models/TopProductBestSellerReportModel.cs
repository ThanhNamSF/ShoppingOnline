using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class TopProductBestSellerReportModel
    {
        public TopProductBestSellerReportModel()
        {
            Details = new List<TopProductBestSellerDetailReportModel>();
        }
        public string Title { get; set; }
        public string CreatedAtString => $".......,Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
        public string CreatedBy { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public List<TopProductBestSellerDetailReportModel> Details { get; set; }
    }
}
