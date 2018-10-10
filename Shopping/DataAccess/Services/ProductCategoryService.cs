using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entity;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ShoppingContext _shoppingContext;

        public ProductCategoryService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public IEnumerable<ProductCategoryModel> GetAllProductCategories()
        {
            var productCategories = _shoppingContext.ProductCategories.AsNoTracking().ToList();
            return Mapper.Map<List<ProductCategoryModel>>(productCategories);
        }
    }
}
