using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Constants;
using Common.SearchConditions;
using DataAccess.Entity;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ShoppingContext _shoppingContext;
        private readonly ICodeGeneratingService _codeGeneratingService;

        public InvoiceService(ShoppingContext shoppingContext, ICodeGeneratingService codeGeneratingService)
        {
            _shoppingContext = shoppingContext;
            _codeGeneratingService = codeGeneratingService;
        }
        public bool Approved(InvoiceModel invoiceModel)
        {
            var invoice = _shoppingContext.Invoices.Find(invoiceModel.Id);
            if (invoice == null)
            {
                return false;
            }

            invoice.ApprovedBy = invoiceModel.ApprovedBy;
            invoice.ApprovedDateTime = DateTime.Now;
            invoice.Status = invoiceModel.Status;
            if (DecreaseQuantityIntoProduct(invoice))
            {
                _shoppingContext.SaveChanges();
                return true;
            }

            return false;

        }

        private bool DecreaseQuantityIntoProduct(Invoice invoice)
        {
            var invoiceDetails = invoice.InvoiceDetails;
            foreach (var item in invoiceDetails)
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

        public bool CheckProductExistedInInvoice(int invoiceId, int productId)
        {
            var invoice = _shoppingContext.Invoices.Find(invoiceId);
            if (invoice != null)
            {
                return invoice.InvoiceDetails.Any(x => x.ProductId == productId);
            }
            return true;
        }

        public bool CheckQuantityInInvoiceDetail(int invoiceId)
        {
            return _shoppingContext.InvoiceDetails.AsNoTracking().Any(w => w.Quantity < 0 && w.InvoiceId == invoiceId);
        }

        public void DeleteInvoice(int id)
        {
            var invoiceDeleted = _shoppingContext.Invoices.Find(id);
            if (invoiceDeleted != null)
            {
                _shoppingContext.Invoices.Remove(invoiceDeleted);
                _shoppingContext.SaveChanges();
            }
        }

        public void DeleteInvoiceDetail(int id)
        {
            var invoiceDetail = _shoppingContext.InvoiceDetails.Find(id);
            if (invoiceDetail == null)
                return;
            _shoppingContext.InvoiceDetails.Remove(invoiceDetail);
            _shoppingContext.SaveChanges();
        }

        public InvoiceModel GetInvoiceById(int id)
        {
            var invoice = _shoppingContext.Invoices.Find(id);
            if (invoice != null)
                return Mapper.Map<InvoiceModel>(invoice);
            return null;
        }

        public InvoiceDetailModel GetInvoiceDetailById(int id)
        {
            var invoiceDetail = _shoppingContext.InvoiceDetails.Find(id);
            if (invoiceDetail != null)
                return Mapper.Map<InvoiceDetailModel>(invoiceDetail);
            return null;
        }

        public bool HasInvoiceDetail(int invoiceId)
        {
            return _shoppingContext.InvoiceDetails.AsNoTracking().Any(w =>w.InvoiceId == invoiceId);
        }

        public void InsertInvoice(InvoiceModel invoiceModel)
        {
            var invoice = Mapper.Map<Invoice>(invoiceModel);
            
            _shoppingContext.Invoices.Add(invoice);
            _shoppingContext.SaveChanges();
            invoiceModel.Id = invoice.Id;
        }

        public void InsertInvoiceDetail(InvoiceDetailModel invoiceDetailModel)
        {
            var invoiceDetail = Mapper.Map<InvoiceDetail>(invoiceDetailModel);
            _shoppingContext.InvoiceDetails.Add(invoiceDetail);
            _shoppingContext.SaveChanges();
        }

        public void Open(InvoiceModel invoiceModel)
        {
            var invoice = _shoppingContext.Invoices.Find(invoiceModel.Id);
            if (invoice == null)
            {
                return;
            }
            invoice.ApprovedBy = invoiceModel.ApprovedBy;
            invoice.ApprovedDateTime = invoiceModel.ApprovedDateTime;
            invoice.Status = invoiceModel.Status;
            IncreaseQuantityInProduct(invoice);
            _shoppingContext.SaveChanges();
        }

        private void IncreaseQuantityInProduct(Invoice invoice)
        {
            var invoiceDetails = invoice.InvoiceDetails;
            foreach (var item in invoiceDetails)
            {
                var product = _shoppingContext.Products.FirstOrDefault(p =>p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }
        }

        public PageList<InvoiceModel> SearchInvoices(InvoiceSearchCondition condition)
        {
            var query = _shoppingContext.Invoices.AsNoTracking().AsQueryable();
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
            return new PageList<InvoiceModel>(Mapper.Map<List<InvoiceModel>>(products), query.Count());
        }

        public PageList<InvoiceDetailModel> SearchInvoiceDetails(InvoiceDetailSearchCondition condition)
        {
            var query = _shoppingContext.InvoiceDetails.AsNoTracking().AsQueryable();
            if (condition.InvoiceId > 0)
            {
                query = query.Where(p => p.InvoiceId == condition.InvoiceId);
            }

            var invoiceDetails = query.OrderBy(o => o.Id).Skip(condition.PageSize * condition.PageNumber)
                .Take(condition.PageSize).ToList();
            return new PageList<InvoiceDetailModel>(Mapper.Map<List<InvoiceDetailModel>>(invoiceDetails), query.Count());
        }

        public void UpdateInvoice(InvoiceModel invoiceModel)
        {
            var invoiceUpdated = _shoppingContext.Invoices.Find(invoiceModel.Id);
            if (invoiceUpdated != null)
            {
                invoiceUpdated = Mapper.Map<Invoice>(invoiceModel);
                _shoppingContext.Invoices.AddOrUpdate(invoiceUpdated);
                _shoppingContext.SaveChanges();
            }
        }

        public void UpdateInvoiceDetail(InvoiceDetailModel invoiceDetailModel)
        {
            var invoiceDetail = _shoppingContext.InvoiceDetails.Find(invoiceDetailModel.Id);
            if (invoiceDetail == null)
                return;
            invoiceDetail = Mapper.Map<InvoiceDetail>(invoiceDetailModel);
            _shoppingContext.InvoiceDetails.AddOrUpdate(invoiceDetail);
            _shoppingContext.SaveChanges();
        }

        public PageList<InvoiceDetailModel> GetInvoiceDetailsByOrderId(InvoiceDetailSearchCondition condition)
        {
            var query = _shoppingContext.OrderDetails.AsNoTracking().Where(w => w.OrderId == condition.OrderId);
            var orderDetails = query.OrderBy(o => o.Id).Skip(condition.PageSize * condition.PageNumber)
                .Take(condition.PageSize).ToList();
            return new PageList<InvoiceDetailModel>(Mapper.Map<List<InvoiceDetailModel>>(orderDetails), query.Count());
        }

        public InvoiceModel CreateNewInvoice(int currentUserId, OrderModel order = null)
        {
            var invoiceModel = new InvoiceModel();
            invoiceModel.CreatedDateTime = DateTime.Now;
            invoiceModel.CreatedBy = currentUserId;
            invoiceModel.Code = _codeGeneratingService.GenerateCode(Values.InvoicePrefix);
            if (order != null)
            {
                invoiceModel.OrderId = order.Id;
                invoiceModel.CustomerAddress = order.ReceiverAddress;
                invoiceModel.CustomerPhone = order.ReceiverPhone;
                invoiceModel.CustomerName = order.ReceiverName;
            }

            return invoiceModel;
        }
    }
}
