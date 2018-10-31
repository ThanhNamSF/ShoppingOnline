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

        public ProductCategoryReportModel GetProductCategoryReport(int categoryId, string createdBy)
        {
            return _reportService.GetProductCategoryReport(categoryId, createdBy);
        }

        public RevenueReportModel GetRevenueReport(DateTime dateFrom, DateTime dateTo, string createdBy)
        {
            return _reportService.GetRevenueReport(dateFrom, dateTo, createdBy);
        }

        public TopProductProfitableReportModel GetTopProductProfitableReport(int topNumber, string createdBy)
        {
            return _reportService.GetTopProductProfitableReportModel(topNumber, createdBy);
        }

        public TopProductBestSellerReportModel GetTopProductBestSellerReportModel(int topNumber, DateTime dateFrom,
            DateTime dateTo, string createdBy)
        {
            return _reportService.GetTopProductBestSellerReportModel(topNumber, dateFrom, dateTo, createdBy);
        }
    }
}
