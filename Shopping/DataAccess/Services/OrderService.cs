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
    public class OrderService : IOrderService
    {
        private readonly ShoppingContext _shoppingContext;

        public OrderService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public void InsertOrder(OrderModel orderModel)
        {
            var order = Mapper.Map<Order>(orderModel);
            _shoppingContext.Orders.Add(order);
            _shoppingContext.SaveChanges();
            orderModel.Id = order.Id;
        }

        public void InsertOrderDetail(OrderDetailModel orderDetailModel)
        {
            var orderDetail = Mapper.Map<OrderDetail>(orderDetailModel);
            _shoppingContext.OrderDetails.Add(orderDetail);
            _shoppingContext.SaveChanges();
        }
    }
}
