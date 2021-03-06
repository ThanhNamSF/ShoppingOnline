﻿using System.Collections.Generic;
using Common;
using Common.SearchConditions;
using DataAccess.Entity;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IProductService
    {
        PageList<ProductModel> SearchProducts(ProductSearchCondition condition);
        PageList<ProductModel> SearchProducts(ProductClientSearchCondition condition);
        void InsertProduct(ProductModel productModel);
        ProductModel GetProductById(int id);
        void DeleteProduct(int id);
        void UpdateProduct(ProductModel productModel);
        ProductModel GetProductByCode(string code);
        void DescreaseProduct(int id, int quantity);
        IEnumerable<ProductModel> GetOthersProductByCategoryId(int productId, int categoryId, int number);
        IEnumerable<ProductModel> GetHostestProducts(int topNumber);
        bool IsProductCodeExisted(string productCode);
        bool IsProductExistedInOrderInvoiceReceive(int productId);
    }
}
