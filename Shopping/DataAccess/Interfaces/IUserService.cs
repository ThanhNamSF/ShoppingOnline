using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IUserService
    {
        UserModel GetUserLogin(UserModel userModel, int role);
        IEnumerable<UserModel> GetAllUserByRole(int role);
        UserModel GetUserById(int id);
        void CreateUser(UserModel userModel);
    }
}
