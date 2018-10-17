using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Shopping.Reports
{
    public class DataAccessLayer
    {
        private readonly IReportService _reportService;

        public DataAccessLayer(IReportService reportService)
        {
            _reportService = reportService;
        }

        public ProductCategoryReportModel GetProductCategoryReport(int categoryId)
        {
            return _reportService.GetProductCategoryReport(categoryId);
        }
    }
}