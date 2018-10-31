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
    public class ContactService : IContactService
    {
        private readonly ShoppingContext _shoppingContext;

        public ContactService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public void DeleteContact(int id)
        {
            var contactDeleted = _shoppingContext.Contacts.Find(id);
            if (contactDeleted != null)
            {
                _shoppingContext.Contacts.Remove(contactDeleted);
                _shoppingContext.SaveChanges();
            }
        }

        public PageList<ContactModel> SearchContacts(ContactSearchCondition condition)
        {
            var query = _shoppingContext.Contacts.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query = query.Where(p => p.Customer.UserName.ToLower().Contains(condition.UserName.ToLower()));
            }

            var dateTo = condition.DateTo.AddDays(1);

            query = query.Where(p =>
                p.CreatedDateTime >= condition.DateFrom && p.CreatedDateTime < dateTo);
            var contacts = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<ContactModel>(Mapper.Map<List<ContactModel>>(contacts), query.Count());
        }

        public void UpdateContact(ContactModel contactModel)
        {
            var contactUpdated = _shoppingContext.Contacts.Find(contactModel.Id);
            if (contactUpdated != null)
            {
                contactUpdated = Mapper.Map<Contact>(contactModel);
                _shoppingContext.Contacts.AddOrUpdate(contactUpdated);
                _shoppingContext.SaveChanges();
            }
        }
    }
}
