using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class GroupUserService : IGroupUserService
    {
        private readonly ShoppingContext _shoppingContext;

        public GroupUserService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public IEnumerable<GroupUserModel> GetAllGroupUsers()
        {
            var groupUsers = _shoppingContext.GroupUsers.AsNoTracking().ToList();
            return Mapper.Map<List<GroupUserModel>>(groupUsers);
        }
    }
}
