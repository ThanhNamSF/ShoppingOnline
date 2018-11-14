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
    public class DeliveryService : IDeliveryService
    {
        private readonly ShoppingContext _shoppingContext;

        public DeliveryService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public bool Approved(DeliveryModel deliveryModel)
        {
            var delivery = _shoppingContext.Deliveries.Find(deliveryModel.Id);
            if (delivery == null)
            {
                return false;
            }

            delivery.ApprovedBy = deliveryModel.ApprovedBy;
            delivery.ApprovedDateTime = DateTime.Now;
            delivery.Status = deliveryModel.Status;
            if (DecreaseQuantityIntoProduct(delivery))
            {
                _shoppingContext.SaveChanges();
                return true;
            }

            return false;

        }

        private bool DecreaseQuantityIntoProduct(Delivery delivery)
        {
            var deliveryDetails = delivery.DeliveryDetails;
            foreach (var item in deliveryDetails)
            {
                var product = _shoppingContext.Products.FirstOrDefault(w =>w.Id == item.ProductId);
                if (product != null)
                {
                    if (product.Quantity < item.Quantity)
                        return false;
                    product.Quantity -= item.Quantity;
                }
            }

            return true;
        }

        public bool CheckProductExistedInDelivery(int deliveryId, int productId)
        {
            var delivery = _shoppingContext.Deliveries.Find(deliveryId);
            if (delivery != null)
            {
                return delivery.DeliveryDetails.Any(x => x.ProductId == productId);
            }
            return true;
        }

        public bool CheckQuantityInDeliveryDetail(int deliveryId)
        {
            return _shoppingContext.DeliveryDetails.AsNoTracking().Any(w => w.Quantity < 0 && w.DeliveryId == deliveryId);
        }

        public void DeleteDelivery(int id)
        {
            var deliveryDeleted = _shoppingContext.Deliveries.Find(id);
            if (deliveryDeleted != null)
            {
                _shoppingContext.Deliveries.Remove(deliveryDeleted);
                _shoppingContext.SaveChanges();
            }
        }

        public void DeleteDeliveryDetail(int id)
        {
            var deliveryDetail = _shoppingContext.DeliveryDetails.Find(id);
            if (deliveryDetail == null)
                return;
            _shoppingContext.DeliveryDetails.Remove(deliveryDetail);
            _shoppingContext.SaveChanges();
        }

        public DeliveryModel GetDeliveryById(int id)
        {
            var delivery = _shoppingContext.Deliveries.Find(id);
            if (delivery != null)
                return Mapper.Map<DeliveryModel>(delivery);
            return null;
        }

        public DeliveryDetailModel GetDeliveryDetailById(int id)
        {
            var deliveryDetail = _shoppingContext.DeliveryDetails.Find(id);
            if (deliveryDetail != null)
                return Mapper.Map<DeliveryDetailModel>(deliveryDetail);
            return null;
        }

        public bool HasDeliveryDetail(int deliveryId)
        {
            return _shoppingContext.DeliveryDetails.AsNoTracking().Any(w =>w.DeliveryId == deliveryId);
        }

        public void InsertDelivery(DeliveryModel deliveryModel)
        {
            var delivery = Mapper.Map<Delivery>(deliveryModel);
            _shoppingContext.Deliveries.Add(delivery);
            _shoppingContext.SaveChanges();
            deliveryModel.Id = delivery.Id;
        }

        public void InsertDeliveryDetail(DeliveryDetailModel deliveryDetailModel)
        {
            var deliveryDetail = Mapper.Map<DeliveryDetail>(deliveryDetailModel);
            _shoppingContext.DeliveryDetails.Add(deliveryDetail);
            _shoppingContext.SaveChanges();
        }

        public void Open(DeliveryModel deliveryModel)
        {
            var delivery = _shoppingContext.Deliveries.Find(deliveryModel.Id);
            if (delivery == null)
            {
                return;
            }
            delivery.ApprovedBy = deliveryModel.ApprovedBy;
            delivery.ApprovedDateTime = deliveryModel.ApprovedDateTime;
            delivery.Status = deliveryModel.Status;
            IncreaseQuantityInProduct(delivery);
            _shoppingContext.SaveChanges();
        }

        private void IncreaseQuantityInProduct(Delivery delivery)
        {
            var deliveryDetails = delivery.DeliveryDetails;
            foreach (var item in deliveryDetails)
            {
                var product = _shoppingContext.Products.FirstOrDefault(p =>p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }
        }

        public PageList<DeliveryModel> SearchDeliveries(DeliverySearchCondition condition)
        {
            var query = _shoppingContext.Deliveries.AsNoTracking().AsQueryable();
            if (condition.ApprovedBy > 0)
            {
                query = query.Where(p => p.ApprovedBy == condition.ApprovedBy);
            }

            if (!string.IsNullOrEmpty(condition.Code))
            {
                query = query.Where(p => p.Code.ToLower().Contains(condition.Code.ToLower()));
            }

            var dateTo = condition.DateTo.AddDays(1);

            query = query.Where(p =>
                p.CreatedDateTime >= condition.DateFrom && p.CreatedDateTime < dateTo);
            var products = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<DeliveryModel>(Mapper.Map<List<DeliveryModel>>(products), query.Count());
        }

        public PageList<DeliveryDetailModel> SearchDeliveryDetails(DeliveryDetailSearchCondition condition)
        {
            var query = _shoppingContext.DeliveryDetails.AsNoTracking().AsQueryable();
            if (condition.DeliveryId > 0)
            {
                query = query.Where(p => p.DeliveryId == condition.DeliveryId);
            }

            var deliveryDetails = query.OrderBy(o => o.Id).Skip(condition.PageSize * condition.PageNumber)
                .Take(condition.PageSize).ToList();
            return new PageList<DeliveryDetailModel>(Mapper.Map<List<DeliveryDetailModel>>(deliveryDetails), query.Count());
        }

        public void UpdateDelivery(DeliveryModel deliveryModel)
        {
            var deliveryUpdated = _shoppingContext.Deliveries.Find(deliveryModel.Id);
            if (deliveryUpdated != null)
            {
                deliveryUpdated = Mapper.Map<Delivery>(deliveryModel);
                _shoppingContext.Deliveries.AddOrUpdate(deliveryUpdated);
                _shoppingContext.SaveChanges();
            }
        }

        public void UpdateDeliveryDetail(DeliveryDetailModel deliveryDetailModel)
        {
            var deliveryDetail = _shoppingContext.DeliveryDetails.Find(deliveryDetailModel.Id);
            if (deliveryDetail == null)
                return;
            deliveryDetail = Mapper.Map<DeliveryDetail>(deliveryDetailModel);
            _shoppingContext.DeliveryDetails.AddOrUpdate(deliveryDetail);
            _shoppingContext.SaveChanges();
        }
    }
}
