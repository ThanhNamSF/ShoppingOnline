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
    public class ReceiveService : IReceiveService
    {
        private readonly ShoppingContext _shoppingContext;

        public ReceiveService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public void Approved(ReceiveModel receiveModel)
        {
            var receive = _shoppingContext.Receives.Find(receiveModel.Id);
            if (receive == null || receive.Deleted)
            {
                return;
            }

            receive.ApprovedBy = receiveModel.ApprovedBy;
            receive.ApprovedDateTime = DateTime.Now;
            receive.Status = receiveModel.Status;
            AddQuantityIntoProduct(receive);
            _shoppingContext.SaveChanges();
        }

        private void AddQuantityIntoProduct(Receive receive)
        {
            var receiveDetails = receive.ReceiveDetails.Where(w => !w.Deleted);
            foreach (var item in receiveDetails)
            {
                var product = _shoppingContext.Products.FirstOrDefault(p => !p.Deleted && p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }
        }

        public bool CheckProductExistedInReceive(int receiveId, int productId)
        {
            var receive = _shoppingContext.Receives.Find(receiveId);
            if (receive != null && !receive.Deleted)
            {
                return receive.ReceiveDetails.Any(x => x.ProductId == productId);
            }
            return true;
        }

        public bool CheckQuantityInReceiveDetail(int receiveId)
        {
            return _shoppingContext.ReceiveDetails.AsNoTracking().Any(w => w.Quantity < 0 && w.ReceiveId == receiveId && !w.Deleted);
        }

        public void DeleteReceive(int id)
        {
            var receiveDeleted = _shoppingContext.Receives.Find(id);
            if (receiveDeleted != null && !receiveDeleted.Deleted)
            {
                receiveDeleted.Deleted = true;
                _shoppingContext.SaveChanges();
            }
        }

        public void DeleteReceiveDetail(int id)
        {
            var receiveDetail = _shoppingContext.ReceiveDetails.Find(id);
            if(receiveDetail == null || receiveDetail.Deleted)
                return;
            receiveDetail.Deleted = true;
            _shoppingContext.SaveChanges();

        }

        public ReceiveModel GetReceiveById(int id)
        {
            var receive = _shoppingContext.Receives.Find(id);
            if (receive != null)
                return Mapper.Map<ReceiveModel>(receive);
            return null;
        }

        public ReceiveDetailModel GetReceiveDetailById(int id)
        {
            var receiveDetail = _shoppingContext.ReceiveDetails.Find(id);
            if (receiveDetail != null && !receiveDetail.Deleted)
                return Mapper.Map<ReceiveDetailModel>(receiveDetail);
            return null;
        }

        public bool HasReceiveDetail(int receiveId)
        {
            return _shoppingContext.ReceiveDetails.AsNoTracking().Any(w => !w.Deleted && w.ReceiveId == receiveId);
        }

        public void InsertReceiveDetail(ReceiveDetailModel receiveDetailModel)
        {
            var receiveDetail = Mapper.Map<ReceiveDetail>(receiveDetailModel);
            _shoppingContext.ReceiveDetails.Add(receiveDetail);
            _shoppingContext.SaveChanges();
        }

        public void InsertReceive(ReceiveModel receiveModel)
        {
            var receive = Mapper.Map<Receive>(receiveModel);
            _shoppingContext.Receives.Add(receive);
            _shoppingContext.SaveChanges();
            receiveModel.Id = receive.Id;
        }

        public bool Open(ReceiveModel receiveModel)
        {
            var receive = _shoppingContext.Receives.Find(receiveModel.Id);
            if (receive == null || receive.Deleted)
            {
                return false;
            }
            receive.ApprovedBy = receiveModel.ApprovedBy;
            receive.ApprovedDateTime = receiveModel.ApprovedDateTime;
            receive.Status = receiveModel.Status;
            if (DecreaseQuantityInProduct(receive))
            {
                _shoppingContext.SaveChanges();
                return true;
            }

            return false;
        }

        private bool DecreaseQuantityInProduct(Receive receive)
        {
            var receiveDetails = receive.ReceiveDetails.Where(w => !w.Deleted);
            foreach (var item in receiveDetails)
            {
                var product = _shoppingContext.Products.FirstOrDefault(p => !p.Deleted && p.Id == item.ProductId);
                if (product != null)
                {
                    if (product.Quantity < item.Quantity)
                        return false;
                    product.Quantity -= item.Quantity;
                }
            }

            return true;
        }

        public PageList<ReceiveDetailModel> SearchReceiveDetails(ReceiveDetailSearchCondition condition)
        {
            var query = _shoppingContext.ReceiveDetails.AsNoTracking().Where(p => !p.Deleted);
            if (condition.ReceiveId > 0)
            {
                query = query.Where(p => p.ReceiveId == condition.ReceiveId);
            }

            var receiveDetails = query.OrderBy(o => o.Id).Skip(condition.PageSize * condition.PageNumber)
                .Take(condition.PageSize).ToList();
            return new PageList<ReceiveDetailModel>(Mapper.Map<List<ReceiveDetailModel>>(receiveDetails), query.Count());
        }

        public PageList<ReceiveModel> SearchReceives(ReceiveSearchCondition condition)
        {
            var query = _shoppingContext.Receives.AsNoTracking().Where(p => !p.Deleted);
            if (condition.ApprovedBy > 0)
            {
                query = query.Where(p => p.ApprovedBy == condition.ApprovedBy);
            }

            if (!string.IsNullOrEmpty(condition.Code))
            {
                query = query.Where(p => p.Code.ToLower().Contains(condition.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(condition.ReceiveFrom))
            {
                query = query.Where(p => p.ReceiveFrom.ToLower().Contains(condition.ReceiveFrom.ToLower()));
            }


            var dateTo = condition.DateTo.AddDays(1);

            query = query.Where(p =>
                p.CreatedDateTime >= condition.DateFrom && p.CreatedDateTime < dateTo);
            var products = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<ReceiveModel>(Mapper.Map<List<ReceiveModel>>(products), query.Count());
        }

        public void UpdateReceive(ReceiveModel receiveModel)
        {
            var receiveUpdated = _shoppingContext.Receives.Find(receiveModel.Id);
            if (receiveUpdated != null && !receiveUpdated.Deleted)
            {
                receiveUpdated = Mapper.Map<Receive>(receiveModel);
                _shoppingContext.Receives.AddOrUpdate(receiveUpdated);
                _shoppingContext.SaveChanges();
            }
        }

        public void UpdateReceiveDetail(ReceiveDetailModel receiveDetailModel)
        {
            var receiveDetail = _shoppingContext.ReceiveDetails.Find(receiveDetailModel.Id);
            if (receiveDetail == null || receiveDetail.Deleted)
                return;
            receiveDetail = Mapper.Map<ReceiveDetail>(receiveDetailModel);
            _shoppingContext.ReceiveDetails.AddOrUpdate(receiveDetail);
            _shoppingContext.SaveChanges();
        }
    }
}
