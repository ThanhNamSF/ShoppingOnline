using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IReportService
    {
        ProductCategoryReportModel GetProductCategoryReport(int categoryId, string createdBy);
        RevenueReportModel GetRevenueReport(DateTime dateFrom, DateTime dateTo, string createdBy);
        TopProductProfitableReportModel GetTopProductProfitableReportModel(int topNumber);

    }
}
