using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IProductCategoryService
    {
        IEnumerable<ProductCategoryModel> GetAllProductCategories();
    }
}
