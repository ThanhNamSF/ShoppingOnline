using Common;
using Common.SearchConditions;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IInvoiceService
    {
        PageList<InvoiceModel> SearchInvoices(InvoiceSearchCondition condition);
        void InsertInvoice(InvoiceModel invoiceModel);
        InvoiceModel GetInvoiceById(int id);
        void DeleteInvoice(int id);
        void UpdateInvoice(InvoiceModel invoiceModel);
        void InsertInvoiceDetail(InvoiceDetailModel invoiceDetailModel);
        bool CheckProductExistedInInvoice(int invoiceId, int productId);
        PageList<InvoiceDetailModel> SearchInvoiceDetails(InvoiceDetailSearchCondition condition);
        PageList<InvoiceDetailModel> GetInvoiceDetailsByOrderId(InvoiceDetailSearchCondition condition);
        InvoiceDetailModel GetInvoiceDetailById(int id);
        void UpdateInvoiceDetail(InvoiceDetailModel invoiceDetailModel);
        void DeleteInvoiceDetail(int id);
        bool HasInvoiceDetail(int invoiceId);
        bool CheckQuantityInInvoiceDetail(int invoiceId);
        bool Approved(InvoiceModel invoiceModel);
        void Open(InvoiceModel invoiceModel);
        InvoiceModel CreateNewInvoice(int currentUserId, OrderModel order = null);
    }
}
