using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using AutoMapper;
using Common;
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

            var dateTo = condition.DateTo.AddDays(1);

            query = query.Where(p =>
                p.CreatedDateTime >= condition.DateFrom && p.CreatedDateTime < dateTo);
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
    }
}
