using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using AutoMapper;
using Common;
using Common.Constants;
using Common.SearchConditions;
using DataAccess.Entity;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class ProductService : IProductService
    {
        private readonly ShoppingContext _shoppingContext;

        public ProductService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public PageList<ProductModel> SearchProducts(ProductSearchCondition condition)
        {
            var query = _shoppingContext.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(condition.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(condition.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(condition.Code))
            {
                query = query.Where(p => p.Code.ToLower().Contains(condition.Code.ToLower()));
            }

            if (condition.CategoryId > 0)
            {
                query = query.Where(p => p.ProductCategoryId == condition.CategoryId);
            }

            if (condition.DateFrom.HasValue)
            {
                query = query.Where(p => p.CreatedDateTime >= condition.DateFrom.Value);
            }

            if (condition.DateTo.HasValue)
            {
                var dateTo = condition.DateTo.Value.AddDays(1);
                query = query.Where(p => p.CreatedDateTime < dateTo);
            }
            var products = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<ProductModel>(Mapper.Map<List<ProductModel>>(products), query.Count());
        }

        public void InsertProduct(ProductModel productModel)
        {
            var product = Mapper.Map<Product>(productModel);
            _shoppingContext.Products.Add(product);
            _shoppingContext.SaveChanges();
            productModel.Id = product.Id;
        }

        public ProductModel GetProductById(int id)
        {
            var product = _shoppingContext.Products.Find(id);
            if(product != null)
                return Mapper.Map<ProductModel>(product);
            return null;
        }

        public void DeleteProduct(int id)
        {
            var productDeleted = _shoppingContext.Products.Find(id);
            if (productDeleted != null)
            {
                _shoppingContext.Products.Remove(productDeleted);
                _shoppingContext.SaveChanges();
            }
            
        }

        public void UpdateProduct(ProductModel productModel)
        {
            var productUpdated = _shoppingContext.Products.Find(productModel.Id);
            if (productUpdated != null)
            {
                productUpdated = Mapper.Map<Product>(productModel);
                _shoppingContext.Products.AddOrUpdate(productUpdated);
                _shoppingContext.SaveChanges();
            }
        }

        public ProductModel GetProductByCode(string code)
        {
            var product = _shoppingContext.Products.AsNoTracking().FirstOrDefault(p =>
                p.Code.ToLower().Equals(code.ToLower()));
            if (product != null)
                return Mapper.Map<ProductModel>(product);
            return null;
        }

        public void DescreaseProduct(int id, int quantity)
        {
            var product = _shoppingContext.Products.Find(id);
            if (product == null || product.Quantity < quantity)
                return;
            product.Quantity -= quantity;
            _shoppingContext.SaveChanges();
        }

        public IEnumerable<ProductModel> GetOthersProductByCategoryId(int productId, int categoryId, int number)
        {
            var otherProducts = _shoppingContext.Products.AsNoTracking()
                .Where(w => w.Id != productId && w.ProductCategoryId == categoryId).OrderBy(r => Guid.NewGuid())
                .Take(number).ToList();
            var otherProductModels = Mapper.Map<List<ProductModel>>(otherProducts);

            var hostesProductIds = GetHostestProducts(Values.BestSellerNumber).Select(s => s.Id);
            var minDate = DateTime.Now.AddDays(-3);
            foreach (var item in otherProductModels)
            {
                if (hostesProductIds.Contains(item.Id))
                {
                    item.Trend = Trend.Hot.ToString();
                }
                else if (item.CreatedDateTime >= minDate)
                {
                    item.Trend = Trend.New.ToString();
                }
                else
                {
                    item.Trend = Trend.All.ToString();
                }
            }

            return otherProductModels;
        }

        public PageList<ProductModel> SearchProducts(ProductClientSearchCondition condition)
        {
            var query = _shoppingContext.Products.AsNoTracking().Where(w => w.ProductCategoryId == condition.ProductCategoryId);
            if (!string.IsNullOrEmpty(condition.Top))
            {
                if (condition.Top == Trend.Hot.ToString())
                {
                    query = GetHotestProducts(query);
                }
                else if(condition.Top == Trend.New.ToString())
                {
                    query = query.OrderByDescending(p => p.CreatedDateTime);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Name);
                }
            }
            if (!string.IsNullOrEmpty(condition.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(condition.Name.ToLower()));
            }

            if (condition.MinPrice >= 0 && condition.MaxPrice >= 0)
            {
                query = query.Where(p => p.Price >= condition.MinPrice && p.Price <= condition.MaxPrice);
            }
            if(condition.Discount > 0)
            {
                query = query.Where(p => p.Promotion >= condition.Discount);
            }
            var products = query.Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            var productModels = Mapper.Map<List<ProductModel>>(products);
            if (!string.IsNullOrEmpty(condition.Top))
            {
                if (condition.Top == Trend.All.ToString())
                {
                    var hostesProductIds = GetHostestProducts(Values.BestSellerNumber).Select(s => s.Id);
                    var minDate = DateTime.Now.AddDays(-3);
                    foreach (var item in productModels)
                    {
                        if (hostesProductIds.Contains(item.Id))
                        {
                            item.Trend = Trend.Hot.ToString();
                        }
                        else if (item.CreatedDateTime >= minDate)
                        {
                            item.Trend = Trend.New.ToString();
                        }
                        else
                        {
                            item.Trend = Trend.All.ToString();
                        }
                    }
                }
                else
                {
                    productModels.ForEach(x => x.Trend = condition.Top);
                }
            }
            return new PageList<ProductModel>(productModels, query.Count());
        }

        private IQueryable<Product> GetHotestProducts(IQueryable<Product> query)
        {
            var deliveryDetails = _shoppingContext.InvoiceDetails.AsNoTracking().Where(w => w.Invoice.ApprovedBy.HasValue)
                .Select(s =>
                    new
                    {
                        Quantity = s.Quantity,
                        Product = s.Product
                    }).ToList();
            var orderDetails = _shoppingContext.OrderDetails.AsNoTracking().Where(w => !w.Order.CanceledBy.HasValue).Select(s =>
                new
                {
                    Quantity = s.Quantity,
                    Product = s.Product
                }).ToList();
            var details = deliveryDetails.Concat(orderDetails).GroupBy(s => s.Product.Id).Select(s =>
                new
                {
                    Quantity = s.Sum(i => i.Quantity),
                    Product = s.FirstOrDefault()?.Product
                }).OrderByDescending(o => o.Quantity);
            var queryResult = details.Select(s => s.Product).AsQueryable();
            return queryResult;
        }

        public IEnumerable<ProductModel> GetHostestProducts(int topNumber)
        {
            var deliveryDetails = _shoppingContext.InvoiceDetails.AsNoTracking().Where(w => w.Invoice.ApprovedBy.HasValue)
                .Select(s =>
                    new
                    {
                        Quantity = s.Quantity,
                        Product = s.Product
                    }).ToList();
            var orderDetails = _shoppingContext.OrderDetails.AsNoTracking().Where(w => !w.Order.CanceledBy.HasValue).Select(s =>
                new
                {
                    Quantity = s.Quantity,
                    Product = s.Product
                }).ToList();
            var details = deliveryDetails.Concat(orderDetails).GroupBy(s => s.Product.Id).Select(s =>
                new
                {
                    Quantity = s.Sum(i => i.Quantity),
                    Product = s.FirstOrDefault()?.Product
                }).OrderByDescending(o => o.Quantity).Skip(0).Take(topNumber);
            var result = details.Select(s => s.Product).ToList();
            return Mapper.Map<IEnumerable<ProductModel>>(result);
        }

        public bool IsProductCodeExisted(string productCode)
        {
            var isExisted = _shoppingContext.Products.AsNoTracking()
                .Any(x => x.Code.Trim().Equals(productCode, StringComparison.CurrentCultureIgnoreCase));
            return isExisted;
        }

        public bool IsProductExistedInOrderInvoiceReceive(int productId)
        {
            var isExistedInOrderDetail =
                _shoppingContext.OrderDetails.AsNoTracking().Any(x => x.ProductId == productId);
            if (isExistedInOrderDetail)
                return true;
            var isExistedInInvoiceDetail =
                _shoppingContext.InvoiceDetails.AsNoTracking().Any(x => x.ProductId == productId);
            if (isExistedInInvoiceDetail)
                return true;
            var isExistedInReceiveDetail =
                _shoppingContext.ReceiveDetails.AsNoTracking().Any(x => x.ProductId == productId);
            return isExistedInReceiveDetail;
        }
    }
}
