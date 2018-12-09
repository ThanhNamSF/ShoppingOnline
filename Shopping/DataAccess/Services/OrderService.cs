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
            receive.ApproverId = orderModel.ApproverId;
            receive.ApprovedDateTime = DateTime.Now;
            _shoppingContext.SaveChanges();
        }

        public void Cancel(OrderModel orderModel)
        {
            var orderCancel = _shoppingContext.Orders.Find(orderModel.Id);
            if (orderCancel != null)
            {
                orderCancel.CanceledBy = orderModel.CanceledBy;
                foreach (var item in orderCancel.OrderDetails)
                {
                    var product = _shoppingContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                    product.Quantity += item.Quantity;
                }
                _shoppingContext.SaveChanges();
            }
        }

        public void Close(OrderModel orderModel)
        {
            var orderClose = _shoppingContext.Orders.Find(orderModel.Id);
            if (orderClose != null)
            {
                orderClose.ClosedBy = orderModel.ClosedBy;
                orderClose.ReceivedDateTime = DateTime.Now;
                _shoppingContext.SaveChanges();
            }
        }

        public void CreateInvoice(int orderId)
        {
            var order = _shoppingContext.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order != null)
            {
                order.IsHasInvoice = true;
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
            {
                var orderModel = Mapper.Map<OrderModel>(order);
                if (orderModel.CanceledBy.HasValue)
                {
                    orderModel.StatusEnum = (int)OrderStatus.Cancelled;
                }
                else if (orderModel.ApproverId.HasValue)
                {
                    orderModel.StatusEnum = (int)OrderStatus.Approved;
                }
                else if (orderModel.DeliverId.HasValue)
                {
                    orderModel.StatusEnum = (int)OrderStatus.Assigned;
                }
                else if (orderModel.ClosedBy.HasValue)
                {
                    orderModel.StatusEnum = (int)OrderStatus.Closed;
                }
                else
                {
                    orderModel.StatusEnum = (int)OrderStatus.Open;
                }
                return orderModel;
            }
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

            switch (condition.Status)
            {
                case (int)OrderStatus.Cancelled:
                    query = query.Where(p => p.CanceledBy.HasValue);
                    break;
                case (int)OrderStatus.Approved:
                    query = query.Where(p => p.ApproverId.HasValue);
                    break;
                case (int)OrderStatus.Assigned:
                    query = query.Where(p => p.DeliverId.HasValue);
                    break;
                case (int)OrderStatus.Closed:
                    query = query.Where(p => p.ClosedBy.HasValue);
                    break;
                case (int)OrderStatus.Open:
                    query = query.Where(p => !p.ClosedBy.HasValue && !p.DeliverId.HasValue && !p.ApproverId.HasValue && !p.CanceledBy.HasValue);
                    break;
                default:
                    break;
            }

            if (condition.DateFrom.HasValue)
            {
                query = query.Where(p => p.CreatedDateTime >= condition.DateFrom.Value);
            }

            if (condition.DateTo.HasValue)
            {
                var dateTo = condition.DateTo.Value.AddDays(1);
                query = query.Where(p => p.CreatedDateTime < dateTo);
            }
            var orders = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            var orderModels = Mapper.Map<List<OrderModel>>(orders);
            foreach (var item in orderModels)
            {
                if (item.CanceledBy.HasValue)
                {
                    item.Status = OrderStatus.Cancelled.ToString();
                }
                else if (item.ApproverId.HasValue)
                {
                    item.Status = OrderStatus.Approved.ToString();
                }
                else if (item.DeliverId.HasValue)
                {
                    item.Status = OrderStatus.Assigned.ToString();
                }
                else if (item.ClosedBy.HasValue)
                {
                    item.Status = OrderStatus.Closed.ToString();
                }
                else
                {
                    item.Status = OrderStatus.Open.ToString(); ;
                }
            }
            return new PageList<OrderModel>(orderModels, query.Count());
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
