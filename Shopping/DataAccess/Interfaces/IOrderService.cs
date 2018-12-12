using System.Collections.Generic;
using Common;
using Common.SearchConditions;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IOrderService
    {
        void InsertOrder(OrderModel orderModel);
        void InsertOrderDetail(OrderDetailModel orderDetailModel);
        PageList<OrderModel> SearchOrders(OrderSearchCondition condition);
        OrderModel GetOrderById(int id);
        void DeleteOrder(int id);
        void UpdateOrder(OrderModel orderModel);
        PageList<OrderDetailModel> SearchOrderDetails(OrderDetailSearchCondition condition);
        void Approved(OrderModel orderModel);
        void Cancel(OrderModel orderModel);
        void Close(OrderModel orderModel);
        IEnumerable<OrderDetailModel> GetOrderDetailsByOrderId(int orderId);
        void CreateInvoice(int orderId);
        IEnumerable<OrderModel> GetAllOrderByCustomerId(int customerId);
    }
}
