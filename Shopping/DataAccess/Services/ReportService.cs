﻿using System;
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
                Amount = s.Price* s.Quantity * (1 - s.Promotion),
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
            var orders = _shoppingContext.Orders.AsNoTracking().Where(w => (w.CreatedDateTime.Month >= monthFrom && w.CreatedDateTime.Year >= yearFrom) &&
                                                                           (w.CreatedDateTime.Month <= monthTo && w.CreatedDateTime.Year <= yearTo) &&
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
                        existedItem.Amount += CalculateAmount(detail.UnitPrice, detail.Quantity, detail.DiscountRate, detail.VatRate);
                    }
                    else
                    {
                        model.Details.Add(new RevenueDetailReportModel()
                        {
                            Quantity = detail.Quantity,
                            Amount = CalculateAmount(detail.UnitPrice, detail.Quantity, detail.DiscountRate, detail.VatRate),
                            CategoryId = detail.Product.ProductCategoryId,
                            CategoryName = detail.Product.ProductCategory.Name,
                            Time = item.CreatedDateTime.ToString("MM/yyyy")
                        });
                    }
                }
            }

            return model;
        }

        public TopProductBestSellerReportModel GetTopProductBestSellerReportModel(int topNumber, DateTime dateFrom, DateTime dateTo, string createdBy)
        {
            var addDateTo = dateTo.AddDays(1);
            var deliveryDetails = _shoppingContext.DeliveryDetails.AsNoTracking().Where(w => w.Delivery.ApprovedBy.HasValue && w.Delivery.CreatedDateTime >= dateFrom && w.Delivery.CreatedDateTime < addDateTo)
                .Select(s =>
                new TopProductBestSellerDetailReportModel()
                {
                    Quantity = s.Quantity,
                    ProductCode = s.Product.Code,
                    CategoryName = s.Product.ProductCategory.Name,
                    ProductName = s.Product.Name
                }).ToList();
            var orderDetails = _shoppingContext.OrderDetails.AsNoTracking().Where(w => !w.Order.Canceled && w.Order.CreatedDateTime >= dateFrom && w.Order.CreatedDateTime < addDateTo).Select(s =>
                new TopProductBestSellerDetailReportModel()
                {
                    Quantity = s.Quantity,
                    ProductCode = s.Product.Code,
                    CategoryName = s.Product.ProductCategory.Name,
                    ProductName = s.Product.Name
                }).ToList();
            var details = deliveryDetails.Concat(orderDetails).GroupBy(s => s.ProductCode).Select(s =>
                new TopProductBestSellerDetailReportModel()
                {
                    Quantity = s.Sum(i => i.Quantity),
                    ProductName = s.FirstOrDefault()?.ProductName,
                    CategoryName = s.FirstOrDefault()?.CategoryName,
                    ProductCode = s.Key
                }).OrderByDescending(o => o.Quantity).Skip(0).Take(topNumber).ToList();
            var result = new TopProductBestSellerReportModel()
            {
                Title = "TOP " + topNumber + " SẢN PHẨM BÁN CHẠY NHẤT",
                TimeFrom = dateFrom.ToString("dd/MM/yyyy"),
                TimeTo = dateTo.ToString("dd/MM/yyyy"),
                CreatedBy = createdBy,
                Details = details
            };
            return result;
        }

        public TopProductProfitableReportModel GetTopProductProfitableReportModel(int topNumber, string createdBy)
        {
            var deliveryDetails = _shoppingContext.DeliveryDetails.AsNoTracking().Where(w => w.Delivery.ApprovedBy.HasValue).Select(s => new TopProductProfitableDetailReportModel()
            {
                Quantity = s.Quantity,
                UnitPrice = CalculateAmount(s.UnitPrice, 1, s.DiscountRate, s.VatRate),
                ProductName = s.Product.Name,
                CategoryName = s.Product.ProductCategory.Name,
                ProductCode = s.Product.Code
            }).ToList();

            var orderDetails = _shoppingContext.OrderDetails.AsNoTracking().Where(w => !w.Order.Canceled).Select(s => new TopProductProfitableDetailReportModel()
            {
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                ProductName = s.Product.Name,
                CategoryName = s.Product.ProductCategory.Name,
                ProductCode = s.Product.Code
            }).ToList();

            var receiveDetails = _shoppingContext.ReceiveDetails.AsNoTracking().GroupBy(s => s.ProductId).Select(s =>
                new TopProductProfitableDetailReportModel()
                {
                    ProductCode = s.FirstOrDefault().Product.Code,
                    ProductName = s.FirstOrDefault().Product.Name,
                    CategoryName = s.FirstOrDefault().Product.ProductCategory.Name,
                    ReceivePriceAverage = s.Average(k => CalculateAmount(k.UnitPrice, 1, k.DiscountRate, k.VatRate))
                });

            var mergeProducts = new List<TopProductProfitableDetailReportModel>(deliveryDetails.Count + orderDetails.Count);
            mergeProducts.AddRange(deliveryDetails);
            mergeProducts.AddRange(orderDetails);

            var sellProducts = mergeProducts.GroupBy(s => s.ProductCode).Select(s =>
                new TopProductProfitableDetailReportModel()
                {
                    ProductCode = s.Key,
                    Quantity = s.Sum(i => i.Quantity),
                    SellPriceAverage = s.Average(i => i.UnitPrice)
                });

            var details = sellProducts.Join(receiveDetails,
                sell => sell.ProductCode,
                receive => receive.ProductCode,
                (sell, receive) => new TopProductProfitableDetailReportModel()
                {
                    ProductCode = receive.ProductCode,
                    ProductName = receive.ProductName,
                    CategoryName = receive.CategoryName,
                    Quantity = sell.Quantity,
                    ReceivePriceAverage = receive.ReceivePriceAverage,
                    SellPriceAverage = sell.SellPriceAverage,
                    Profitable = (sell.SellPriceAverage - receive.ReceivePriceAverage) * sell.Quantity
                }).OrderByDescending(s => s.Profitable).Skip(0).Take(topNumber).ToList();

            var result = new TopProductProfitableReportModel()
            {
                Details = details,
                CreatedBy = createdBy,
                Title = "TOP " + topNumber + " SẢN PHẨM SINH LỜI CAO NHẤT"
            };
            return result;
        }

        private double CalculateAmount(double unitPrice, double quantity, double discountRate, double vatRate)
        {
            var subAmount = unitPrice * quantity;
            var discountAmount = subAmount * discountRate / 100;
            var vatAmount = (subAmount - discountAmount) * vatRate / 100;
            var amount = subAmount - discountAmount + vatAmount;
            return amount;
        }
    }
}
