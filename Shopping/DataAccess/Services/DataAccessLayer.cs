using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class DataAccessLayer
    {
        private readonly ReportService _reportService;


        public DataAccessLayer()
        {   
            var shoppingContext = new ShoppingContext();
            _reportService = new ReportService(shoppingContext);
        }

        public ProductCategoryReportModel GetProductCategoryReport(int categoryId)
        {
            return _reportService.GetProductCategoryReport(categoryId);
        }

        public RevenueReportModel GetRevenueReport(DateTime dateFrom, DateTime dateTo)
        {
            return _reportService.GetRevenueReport(dateFrom, dateTo);
        }
    }
}
