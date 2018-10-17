using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ProductCategoryReportModel
    {
        public ProductCategoryReportModel()
        {
            Details = new List<ProductCategoryDetailReportModel>();
            CreatedDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public string CreatedDateTime { get; set; }
        public string CreatedAtString => $".......,Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
        public string CreatedBy { get; set; }
        public string CategoryName { get; set; }
        public List<ProductCategoryDetailReportModel> Details { get; set; }
    }
}
