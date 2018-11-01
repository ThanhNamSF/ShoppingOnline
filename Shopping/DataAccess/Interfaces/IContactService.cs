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
    public interface IContactService
    {
        PageList<ContactModel> SearchContacts(ContactSearchCondition condition);
        void DeleteContact(int id);
        void UpdateContact(ContactModel contactModel);
        ContactModel GetContactById(int id);
    }
}
