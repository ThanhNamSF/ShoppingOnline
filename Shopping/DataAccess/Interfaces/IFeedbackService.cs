using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Common.SearchConditions;

namespace DataAccess.Interfaces
{
    public interface IFeedbackService
    {
        PageList<FeedbackModel> SearchFeedbacks(FeedbackSearchCondition condition);
        void DeleteFeedback(int id);
        FeedbackModel GetFeedbackById(int id);
        void AddReply(FeedbackModel feedbackModel);
        IEnumerable<FeedbackModel> GetAllFeedbackByFeedbackId(int feedbackId);
        void InsertFeedback(FeedbackModel feedbackModel);
    }
}
