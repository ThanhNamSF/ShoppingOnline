using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.SearchConditions;
using DataAccess.Entity;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IUserService
    {
        UserModel GetUserLogin(UserModel userModel);
        IEnumerable<UserModel> GetAllUserByRole(int role);
        UserModel GetUserById(int id);
        void CreateUser(UserModel userModel);
        PageList<UserModel> SearchUsers(UserSearchCondition condition);
        void InsertUser(UserModel userModel);
        void DeleteUser(int id);
        void UpdateUser(UserModel userModel);
        bool CheckUserNameIsExisted(string userName);
        IEnumerable<UserModel> GetSortedShipperByOrderQuantity();
    }
}
