using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.SearchConditions;
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

        public void Approved(OrderModel orderModel)
        {
            var receive = _shoppingContext.Orders.Find(orderModel.Id);
            if (receive == null)
            {
                return;
            }
            receive.ApprovedId = orderModel.ApprovedId;
            receive.ApprovedDateTime = DateTime.Now;
            receive.Status = orderModel.Status;
            _shoppingContext.SaveChanges();
        }

        public void Cancel(int orderId)
        {
            var orderCancel = _shoppingContext.Orders.Find(orderId);
            if (orderCancel != null)
            {
                orderCancel.ReceivedDateTime = null;
                orderCancel.Canceled = true;
                _shoppingContext.SaveChanges();
            }
        }

        public void DeleteOrder(int id)
        {
            var orderDeleted = _shoppingContext.Orders.Find(id);
            if (orderDeleted != null)
            {
                _shoppingContext.Orders.Remove(orderDeleted);
                _shoppingContext.SaveChanges();
            }
        }

        public OrderModel GetOrderById(int id)
        {
            var order = _shoppingContext.Orders.Find(id);
            if (order != null)
                return Mapper.Map<OrderModel>(order);
            return null;
        }

        public IEnumerable<OrderDetailModel> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = _shoppingContext.OrderDetails.AsNoTracking().Where(w => w.OrderId == orderId).ToList();
            return Mapper.Map<IEnumerable<OrderDetailModel>>(orderDetails);
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

        public PageList<OrderDetailModel> SearchOrderDetails(OrderDetailSearchCondition condition)
        {
            var query = _shoppingContext.OrderDetails.AsNoTracking().AsQueryable();
            if (condition.OrderId > 0)
            {
                query = query.Where(p => p.OrderId == condition.OrderId);
            }

            var receiveDetails = query.OrderBy(o => o.Id).Skip(condition.PageSize * condition.PageNumber)
                .Take(condition.PageSize).ToList();
            return new PageList<OrderDetailModel>(Mapper.Map<List<OrderDetailModel>>(receiveDetails), query.Count());
        }

        public PageList<OrderModel> SearchOrders(OrderSearchCondition condition)
        {
            var query = _shoppingContext.Orders.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(condition.ReceiverName))
            {
                query = query.Where(p => p.ReceiverName.ToLower().Contains(condition.ReceiverName.ToLower()));
            }

            if (!string.IsNullOrEmpty(condition.OrderCode))
            {
                query = query.Where(p => p.Code.ToLower().Contains(condition.OrderCode.ToLower()));
            }

            if (condition.ApprovedStatus.HasValue)
            {
                query = query.Where(p => p.ApprovedId.HasValue == condition.ApprovedStatus.Value);
            }

            if (condition.AsignmentStatus.HasValue)
            {
                query = query.Where(p => p.DeliveredId.HasValue == condition.AsignmentStatus.Value);
            }

            if (condition.DeliveredStatus.HasValue)
            {
                query = query.Where(p => p.ReceivedDateTime.HasValue == condition.DeliveredStatus.Value);
            }

            if (condition.Canceled.HasValue)
            {
                query = query.Where(p => p.Canceled == condition.Canceled.Value);
            }
            var dateTo = condition.DateTo.AddDays(1);

            query = query.Where(p =>
                p.CreatedDateTime >= condition.DateFrom && p.CreatedDateTime < dateTo);
            var orders = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<OrderModel>(Mapper.Map<List<OrderModel>>(orders), query.Count());
        }

        public void UpdateOrder(OrderModel orderModel)
        {
            var orderUpdated = _shoppingContext.Orders.Find(orderModel.Id);
            if (orderUpdated != null)
            {
                orderUpdated = Mapper.Map<Order>(orderModel);
                _shoppingContext.Orders.AddOrUpdate(orderUpdated);
                _shoppingContext.SaveChanges();
            }
        }
    }
}
