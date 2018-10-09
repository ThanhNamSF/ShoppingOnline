using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Entity;

namespace BusinessLogic.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>();
            CreateMap<Product, ProductModel>();
            CreateMap<ProductCategory, ProductCategoryModel>();
        }
    }
}
