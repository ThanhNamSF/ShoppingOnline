using System.Collections.Generic;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Common.SearchConditions;
using DataAccess.Interfaces;

namespace BusinessLogic.BusinessLogics
{
    public class ProductBusinessLogic : IProductBusinessLogic
    {
        private readonly IProductService _productService;

        public ProductBusinessLogic(IProductService productService)
        {
            _productService = productService;
        }
        public IEnumerable<ProductModel> SearchProducts(ProductSearchCondition condition)
        {
            var products = _productService.SearchProducts(condition);
            var productModels = Mapper.Map<List<ProductModel>>(products);
            return productModels;
        }
    }
}
