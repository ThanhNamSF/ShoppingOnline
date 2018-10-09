using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entity;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly ShoppingContext _shoppingContext;

        public UserService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        public IEnumerable<UserModel> GetAllUserByRole(int role)
        {
            var users = _shoppingContext.Users.AsNoTracking().Where(p => !p.Deleted && p.Role == role).ToList();
            return Mapper.Map<List<UserModel>>(users);
        }

        public UserModel GetUserLogin(UserModel user)
        {
            var userLogin = _shoppingContext.Users.AsNoTracking().FirstOrDefault(x =>
                x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password));
            return Mapper.Map<UserModel>(userLogin);
        }
    }
}
