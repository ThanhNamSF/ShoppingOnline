using System.Collections.Generic;
using Common;
using Common.SearchConditions;
using DataAccess.Entity;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IProductService
    {
        PageList<ProductModel> SearchProducts(ProductSearchCondition condition);
        void InsertProduct(ProductModel productModel);
        ProductModel GetProductById(int id);
        void DeleteProduct(int id);
        void UpdateProduct(ProductModel productModel);
        ProductModel GetProductByCode(string code);
    }
}
