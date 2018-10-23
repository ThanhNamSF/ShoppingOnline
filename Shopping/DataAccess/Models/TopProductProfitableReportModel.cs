using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class TopProductProfitableReportModel
    {
        public TopProductProfitableReportModel()
        {
            Details = new List<TopProductProfitableDetailReportModel>();
        }
        public string Title { get; set; }
        public string CreatedAtString => $".......,Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
        public string CreatedBy { get; set; }
        public List<TopProductProfitableDetailReportModel> Details { get; set; }
    }
}
