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
        public ProductCategoryReportModel GetProductCategoryReport(int categoryId, string createdBy)
        {
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

            model.CreatedBy = createdBy;
            model.Details = products.Select(s => new ProductCategoryDetailReportModel()
            {
                Price = s.Price,
                Quantity = s.Quantity,
                Amount = s.Price * s.Quantity,
                CategoryId = s.ProductCategory.Id,
                CategoryName = s.ProductCategory.Name,
                ProductCode = s.Code,
                ProductName = s.Name
            }).ToList();
            return model;
        }

        public RevenueReportModel GetRevenueReport(DateTime dateFrom, DateTime dateTo, string createdBy)
        {
            var monthFrom = dateFrom.Month;
            var yearFrom = dateFrom.Year;
            var monthTo = dateTo.Month;
            var yearTo = dateTo.Year;
            var orders = _shoppingContext.Orders.AsNoTracking().Where(w => w.ApprovedId.HasValue && 
                                                                           (w.ApprovedDateTime.Value.Month >= monthFrom && w.ApprovedDateTime.Value.Year >= yearFrom) &&
                                                                           (w.ApprovedDateTime.Value.Month <= monthTo && w.ApprovedDateTime.Value.Year <= yearTo) &&
                                                                           !w.Canceled).OrderBy(w => w.CreatedDateTime).ToList();
            var deliveries = _shoppingContext.Deliveries.AsNoTracking().Where(w => w.ApprovedBy.HasValue &&
                                                                                   (w.ApprovedDateTime.Value.Month >= monthFrom && w.ApprovedDateTime.Value.Year >= yearFrom) &&
                                                                                   (w.ApprovedDateTime.Value.Month <= monthTo && w.ApprovedDateTime.Value.Year <= yearTo))
                                                                                    .OrderBy(w => w.CreatedDateTime).ToList();

            var model = new RevenueReportModel()
            {
                TimeFrom = dateFrom.ToString("MM/yyyy"),
                TimeTo = dateTo.ToString("MM/yyyy"),
                CreatedBy = createdBy
            };
            foreach (var item in orders)
            {
                foreach (var orderDetail in item.OrderDetails)
                {
                    var existedItem =
                        model.Details.FirstOrDefault(w => item.CreatedDateTime.ToString("MM/yyyy").Equals(w.Time, StringComparison.OrdinalIgnoreCase));
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

            foreach (var item in deliveries)
            {
                foreach (var detail in item.DeliveryDetails)
                {
                    var existedItem =
                        model.Details.FirstOrDefault(w => item.CreatedDateTime.ToString("MM/yyyy").Equals(w.Time, StringComparison.OrdinalIgnoreCase));
                    if (existedItem != null)
                    {
                        existedItem.Quantity += detail.Quantity;
                        existedItem.Amount += detail.Quantity * detail.UnitPrice;
                    }
                    else
                    {
                        model.Details.Add(new RevenueDetailReportModel()
                        {
                            Quantity = detail.Quantity,
                            Amount = detail.Quantity * detail.UnitPrice,
                            CategoryId = detail.Product.ProductCategoryId,
                            CategoryName = detail.Product.ProductCategory.Name,
                            Time = item.CreatedDateTime.ToString("MM/yyyy")
                        });
                    }
                }
            }

            return model;
        }

        public TopProductProfitableReportModel GetTopProductProfitableReportModel(int topNumber)
        {
            var deliveryDetails = _shoppingContext.DeliveryDetails.AsNoTracking().AsQueryable();
            var orderDetails = _shoppingContext.OrderDetails.AsNoTracking().AsQueryable();
            var receiveDetails = _shoppingContext.ReceiveDetails.AsNoTracking().AsQueryable();
            var groupDeliveryDetails = deliveryDetails.GroupBy(s => s.Product).Select(k => new
            {
                Product = k.Key,
                Quantity = k.Sum(i => i.Quantity),
                UnitPriceAverage = k.Sum(i => i.UnitPrice) / k.Count()
            });
            return null;

        }
    }
}
