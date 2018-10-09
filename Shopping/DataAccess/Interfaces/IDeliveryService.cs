using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.SearchConditions;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IDeliveryService
    {
        PageList<DeliveryModel> SearchDeliveries(DeliverySearchCondition condition);
        void InsertDelivery(DeliveryModel deliveryModel);
        DeliveryModel GetDeliveryById(int id);
        void DeleteDelivery(int id);
        void UpdateDelivery(DeliveryModel deliveryModel);
        void InsertDeliveryDetail(DeliveryDetailModel deliveryDetailModel);
        bool CheckProductExistedInDelivery(int deliveryId, int productId);
        PageList<DeliveryDetailModel> SearchDeliveryDetails(DeliveryDetailSearchCondition condition);
        DeliveryDetailModel GetDeliveryDetailById(int id);
        void UpdateDeliveryDetail(DeliveryDetailModel deliveryDetailModel);
        void DeleteDeliveryDetail(int id);
        bool HasDeliveryDetail(int deliveryId);
        bool CheckQuantityInDeliveryDetail(int deliveryId);
        bool Approved(DeliveryModel deliveryModel);
        void Open(DeliveryModel deliveryModel);
    }
}
