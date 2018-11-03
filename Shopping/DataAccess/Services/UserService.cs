using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Constants;
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

        public void CreateUser(UserModel userModel)
        {
            var user = Mapper.Map<User>(userModel);
            _shoppingContext.Users.Add(user);
            _shoppingContext.SaveChanges();
        }

        public IEnumerable<UserModel> GetAllUserByRole(int role)
        {
            var users = _shoppingContext.Users.AsNoTracking().Where(p =>p.Role == role).ToList();
            return Mapper.Map<List<UserModel>>(users);
        }

        public UserModel GetUserById(int id)
        {
            var user = _shoppingContext.Users.Find(id);
            return Mapper.Map<UserModel>(user);
        }

        public UserModel GetUserLogin(UserModel userModel, int role)
        {
            var userLogin = _shoppingContext.Users.AsNoTracking().FirstOrDefault(x =>
                x.UserName.Equals(userModel.UserName) && x.Password.Equals(userModel.Password) && x.Role == role);
            return Mapper.Map<UserModel>(userLogin);
        }
    }
}
