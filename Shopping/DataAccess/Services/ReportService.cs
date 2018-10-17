using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common.Constants;
using DataAccess.Entity;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class ReportService : IReportService
    {
        private readonly ShoppingContext _shoppingContext;

        public ReportService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public ProductCategoryReportModel GetProductCategoryReport(int categoryId)
        {
            var currentUser = HttpContext.Current.Session[Values.USER_SESSION] as UserModel;
            var model = new ProductCategoryReportModel();
            IQueryable<Product> products;
            if (categoryId > 0)
            {
                var category = _shoppingContext.ProductCategories.Find(categoryId);
                if (category == null)
                    return model;
                products = _shoppingContext.Products.AsNoTracking().Where(x => x.ProductCategoryId == categoryId && x.Quantity > 0);
                model.CategoryName = category.Name;
            }
            else
            {
                products = _shoppingContext.Products.AsNoTracking().Where(x => x.Quantity > 0);
                model.CategoryName = Values.All;
            }
            
            model.Details = products.Select(s => new ProductCategoryDetailReportModel()
            {
                Price = s.Price,
                Quantity = s.Quantity,
                Amount = s.Price * s.Quantity,
                CategoryId = categoryId,
                CategoryName = s.ProductCategory.Name,
                ProductCode = s.Code,
                ProductName = s.Name
            }).ToList();
            return model;
        }

        public RevenueReportModel GetRevenueReport(DateTime dateFrom, DateTime dateTo)
        {
            var monthFrom = dateFrom.Month;
            var yearFrom = dateFrom.Year;
            var monthTo = dateTo.Month;
            var yearTo = dateTo.Year;
            var orders = _shoppingContext.Orders.AsNoTracking().Where(w => w.ApprovedId.HasValue &&
                                                                           (w.CreatedDateTime.Month >= monthFrom && w.CreatedDateTime.Year >= yearFrom) &&
                                                                           (w.CreatedDateTime.Month <= monthTo && w.CreatedDateTime.Year <= yearTo));
            var model = new RevenueReportModel()
            {
                TimeFrom = dateFrom.ToString("MM/yyyy"),
                TimeTo = dateTo.ToString("MM/yyyy")
            };
            foreach (var item in orders)
            {
                foreach (var orderDetail in item.OrderDetails)
                {
                    var existedItem =
                        model.Details.FirstOrDefault(w => w.CategoryId == orderDetail.Product.ProductCategoryId);
                    if (existedItem != null)
                    {
                        existedItem.Quantity += orderDetail.Quantity;
                        existedItem.Amount += orderDetail.Quantity * orderDetail.UnitPrice;
                    }
                    else
                    {
                        model.Details.Add(new RevenueDetailReportModel()
                        {
                            Quantity = orderDetail.Quantity,
                            Amount = orderDetail.Quantity * orderDetail.UnitPrice,
                            CategoryId = orderDetail.Product.ProductCategoryId,
                            CategoryName = orderDetail.Product.ProductCategory.Name,
                            Time = item.CreatedDateTime.ToString("MM/yyyy")
                        });
                    }
                }
            }

            return model;
        }
    }
}
