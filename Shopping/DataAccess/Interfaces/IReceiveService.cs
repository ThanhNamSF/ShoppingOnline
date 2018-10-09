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
    public interface IReceiveService
    {
        PageList<ReceiveModel> SearchReceives(ReceiveSearchCondition condition);
        void InsertReceive(ReceiveModel receiveModel);
        ReceiveModel GetReceiveById(int id);
        void DeleteReceive(int id);
        void UpdateReceive(ReceiveModel receiveModel);
        void InsertReceiveDetail(ReceiveDetailModel receiveDetailModel);
        bool CheckProductExistedInReceive(int receiveId, int productId);
        PageList<ReceiveDetailModel> SearchReceiveDetails(ReceiveDetailSearchCondition condition);
        ReceiveDetailModel GetReceiveDetailById(int id);
        void UpdateReceiveDetail(ReceiveDetailModel receiveDetailModel);
        void DeleteReceiveDetail(int id);
        bool HasReceiveDetail(int receiveId);
        bool CheckQuantityInReceiveDetail(int receiveId);
        void Approved(ReceiveModel receiveModel);
        bool Open(ReceiveModel receiveModel);
    }
}
