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
    public class FeedbackService : IFeedbackService
    {
        private readonly ShoppingContext _shoppingContext;

        public FeedbackService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public void AddReply(FeedbackModel feedbackModel)
        {
            var feedBackGroup = new FeedbackGroup()
            {
                FeedbackId = feedbackModel.Id,
                RepliedDateTime = feedbackModel.RepliedDateTime,
                ReplyContent = feedbackModel.ReplyContent,
                UserId = feedbackModel.ReplierId
            };
            if (feedBackGroup != null)
            {
                _shoppingContext.FeedbackGroups.Add(feedBackGroup);
                _shoppingContext.SaveChanges();
            }
        }

        public void DeleteFeedback(int id)
        {
            var feedbackDeleted = _shoppingContext.Feedbacks.Find(id);
            if (feedbackDeleted != null)
            {
                _shoppingContext.Feedbacks.Remove(feedbackDeleted);
                _shoppingContext.SaveChanges();
            }
        }

        public IEnumerable<FeedbackModel> GetAllFeedbackByFeedbackId(int feedbackId)
        {
            var feedbacks = _shoppingContext.FeedbackGroups.AsNoTracking().Where(w => w.FeedbackId == feedbackId)
                .Select(s => new FeedbackModel()
                {
                    ReplyContent = s.ReplyContent,
                    RepliedDateTime = s.RepliedDateTime,
                    ReplierUserName = s.User.LastName + " " + s.User.FirstName
                });
            return feedbacks;
        }

        public FeedbackModel GetFeedbackById(int id)
        {
            var feedback = _shoppingContext.Feedbacks.Find(id);
            if (feedback != null)
                return Mapper.Map<FeedbackModel>(feedback);
            return null;
        }

        public void InsertFeedback(FeedbackModel feedbackModel)
        {
            var feedback = Mapper.Map<Feedback>(feedbackModel);
            _shoppingContext.Feedbacks.Add(feedback);
            _shoppingContext.SaveChanges();
            feedbackModel.Id = feedback.Id;
        }

        public PageList<FeedbackModel> SearchFeedbacks(FeedbackSearchCondition condition)
        {
            var query = _shoppingContext.Feedbacks.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query = query.Where(p => p.Customer.UserName.ToLower().Contains(condition.UserName.ToLower()));
            }

            if (condition.IsReplied.HasValue)
            {
                if (condition.IsReplied.Value)
                {
                    query = query.Where(p => p.FeedbackGroups.Count > 0);
                }
                else
                {
                    query = query.Where(p => p.FeedbackGroups.Count == 0);
                }
                
            }

            var dateTo = condition.DateTo.AddDays(1);

            query = query.Where(p =>
                p.CreatedDateTime >= condition.DateFrom && p.CreatedDateTime < dateTo);
            var feedbacks = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<FeedbackModel>(Mapper.Map<List<FeedbackModel>>(feedbacks), query.Count());
        }
    }
}
