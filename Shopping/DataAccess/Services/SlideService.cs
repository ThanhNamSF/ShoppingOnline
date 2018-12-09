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
    public class SlideService : ISlideService
    {
        private readonly ShoppingContext _shoppingContext;

        public SlideService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public void DeleteSlide(int id)
        {
            var slideDeleted = _shoppingContext.Slides.Find(id);
            if (slideDeleted != null)
            {
                _shoppingContext.Slides.Remove(slideDeleted);
                _shoppingContext.SaveChanges();
            }
        }

        public SlideModel GetSlideById(int id)
        {
            var slide = _shoppingContext.Slides.Find(id);
            if (slide != null)
                return Mapper.Map<SlideModel>(slide);
            return null;
        }

        public IEnumerable<SlideModel> GetSlideForDisplay()
        {
            var slides = _shoppingContext.Slides.AsNoTracking().Where(w => w.Status).ToList();
            return Mapper.Map<IEnumerable<SlideModel>>(slides);
        }

        public void InsertSlide(SlideModel slideModel)
        {
            var slide = Mapper.Map<Slide>(slideModel);
            _shoppingContext.Slides.Add(slide);
            _shoppingContext.SaveChanges();
            slideModel.Id = slide.Id;
        }

        public PageList<SlideModel> SearchSlides(SlideSearchCondition condition)
        {
            var query = _shoppingContext.Slides.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(condition.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(condition.Name.ToLower()));
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
            var Slides = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<SlideModel>(Mapper.Map<List<SlideModel>>(Slides), query.Count());
        }

        public void UpdateSlide(SlideModel slideModel)
        {
            var slideUpdated = _shoppingContext.Slides.Find(slideModel.Id);
            if (slideUpdated != null)
            {
                slideUpdated = Mapper.Map<Slide>(slideModel);
                _shoppingContext.Slides.AddOrUpdate(slideUpdated);
                _shoppingContext.SaveChanges();
            }
        }
    }
}
