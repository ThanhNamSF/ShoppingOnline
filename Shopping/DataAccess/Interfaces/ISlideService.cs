using Common;
using Common.SearchConditions;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ISlideService
    {
        PageList<SlideModel> SearchSlides(SlideSearchCondition condition);
        IEnumerable<SlideModel> GetSlideForDisplay();
        void InsertSlide(SlideModel slideModel);
        SlideModel GetSlideById(int id);
        void DeleteSlide(int id);
        void UpdateSlide(SlideModel slideModel);
    }
}
