using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entity;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class AboutService : IAboutService
    {
        private readonly ShoppingContext _shoppingContext;

        public AboutService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public AboutModel GetAboutUs()
        {
            var about = _shoppingContext.Abouts.FirstOrDefault();
            if (about != null)
            {
                return Mapper.Map<AboutModel>(about);
            }

            return null;
        }

        public void UpdateAbout(AboutModel aboutModel)
        {
            var aboutUpdated = _shoppingContext.Abouts.Find(aboutModel.Id);
            if (aboutUpdated != null)
            {
                aboutUpdated = Mapper.Map<About>(aboutModel);
                _shoppingContext.Abouts.AddOrUpdate(aboutUpdated);
                _shoppingContext.SaveChanges();
            }
        }
    }
}
