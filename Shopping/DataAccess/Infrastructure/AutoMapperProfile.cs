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
            CreateMap<User, UserModel>();
            CreateMap<CustomerModel, Customer>();
            CreateMap<Customer, CustomerModel>();
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
            CreateMap<SlideModel, Slide>();
            CreateMap<Slide, SlideModel>();
            CreateMap<AboutModel, About>();
            CreateMap<About, AboutModel>();
            CreateMap<FeedbackModel, Feedback>();
            CreateMap<Feedback, FeedbackModel>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.Customer.LastName + " " + x.Customer.FirstName))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Customer.Email))
                .ForMember(x => x.Phone, opt => opt.MapFrom(x => x.Customer.Phone))
                .ForMember(x => x.CustomerUserName, opt => opt.MapFrom(x => x.Customer.UserName));
            CreateMap<FeedbackModel, FeedbackGroup>();
            CreateMap<GroupUser, GroupUserModel>();
        }
    }
}
