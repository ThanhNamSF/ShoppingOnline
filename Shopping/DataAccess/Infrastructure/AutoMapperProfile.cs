using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entity;
using DataAccess.Models;

namespace DataAccess.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>();
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
            CreateMap<ProductCategory, ProductCategoryModel>();
            CreateMap<Receive, ReceiveModel>();
            CreateMap<ReceiveModel, Receive>();
            CreateMap<ReceiveDetailModel, ReceiveDetail>();
            CreateMap<ReceiveDetail, ReceiveDetailModel>();
            CreateMap<Delivery, DeliveryModel>();
            CreateMap<DeliveryModel, Delivery>();
            CreateMap<DeliveryDetailModel, DeliveryDetail>();
            CreateMap<DeliveryDetail, DeliveryDetailModel>();
            CreateMap<Order, OrderModel>();
            CreateMap<OrderModel, Order>();
            CreateMap<OrderDetail, OrderDetailModel>();
            CreateMap<OrderDetailModel, OrderDetail>();
        }
    }
}
