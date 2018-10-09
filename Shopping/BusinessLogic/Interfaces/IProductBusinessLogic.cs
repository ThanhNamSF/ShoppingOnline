using System.Collections.Generic;
using BusinessLogic.Models;
using Common.SearchConditions;

namespace BusinessLogic.Interfaces
{
    public interface IProductBusinessLogic
    {
        IEnumerable<ProductModel> SearchProducts(ProductSearchCondition condition);
    }
}
