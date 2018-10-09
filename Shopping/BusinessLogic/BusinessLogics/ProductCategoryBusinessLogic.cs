using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DataAccess.Interfaces;

namespace BusinessLogic.BusinessLogics
{
    public class ProductCategoryBusinessLogic : IProductCategoryBusinessLogic
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryBusinessLogic(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public IEnumerable<ProductCategoryModel> GetAllProductCategories()
        {
            var productCategories = _productCategoryService.GetAllProductCategories();
            var productCategoryModels = Mapper.Map<List<ProductCategoryModel>>(productCategories);
            return productCategoryModels;
        }
    }
}
