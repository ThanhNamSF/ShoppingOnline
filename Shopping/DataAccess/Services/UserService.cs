﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Constants;
using Common.SearchConditions;
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

        public bool CheckUserNameIsExisted(string userName)
        {
            var userNameIsExisted = _shoppingContext.Users.AsNoTracking()
                .Any(x => x.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            return userNameIsExisted;
        }

        public void CreateUser(UserModel userModel)
        {
            var user = Mapper.Map<User>(userModel);
            _shoppingContext.Users.Add(user);
            _shoppingContext.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var userDeleted = _shoppingContext.Users.Find(id);
            if (userDeleted != null)
            {
                _shoppingContext.Users.Remove(userDeleted);
                _shoppingContext.SaveChanges();
            }
        }

        public IEnumerable<UserModel> GetAllUserByRole(int role)
        {
            var users = _shoppingContext.Users.AsNoTracking().Where(w => w.GroupUser.Id == role && w.Status).ToList();
            return Mapper.Map<List<UserModel>>(users);
        }

        public IEnumerable<UserModel> GetSortedShipperByOrderQuantity()
        {
            var users = _shoppingContext.Users.AsNoTracking().Where(w => w.GroupUser.Id == (int)UserRole.Shipper && w.Status);
            var orders = _shoppingContext.Orders.AsNoTracking().Where(w => w.ApproverId.HasValue);
            var result = users.GroupJoin(orders,
                                        u => u.Id,
                                        o => o.DeliverId,
                                        (u, o) => new
                                        {
                                            user = u,
                                            order = o
                                        })
                            .SelectMany(temp => temp.order.DefaultIfEmpty(),
                                        (temp, o) => new
                                        {
                                            Id = temp.user.Id,
                                            UserName = temp.user.UserName,
                                            Quantity = o == null ? 0 : 1
                                        })
                            .GroupBy(s => s.Id)
                            .OrderBy(s => s.Count(item => item.Quantity > 0))
                            .Select(s => new UserModel
                                    {
                                        Id = s.Key,
                                        UserName = s.FirstOrDefault().UserName
                                    }).ToList();
            return result;
        }

        public UserModel GetUserById(int id)
        {
            var user = _shoppingContext.Users.Find(id);
            return Mapper.Map<UserModel>(user);
        }

        public UserModel GetUserLogin(UserModel userModel)
        {
            var userLogin = _shoppingContext.Users.AsNoTracking().FirstOrDefault(x =>
                x.UserName.Equals(userModel.UserName) && x.Password.Equals(userModel.Password) && x.Status && !x.GroupUser.Name.Equals(UserRole.Shipper.ToString()));
            return Mapper.Map<UserModel>(userLogin);
        }

        public void InsertUser(UserModel userModel)
        {
            var user = Mapper.Map<User>(userModel);
            _shoppingContext.Users.Add(user);
            _shoppingContext.SaveChanges();
            userModel.Id = user.Id;
        }

        public PageList<UserModel> SearchUsers(UserSearchCondition condition)
        {
            var query = _shoppingContext.Users.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query = query.Where(p => p.UserName.ToLower().Contains(condition.UserName.ToLower()));
            }

            if (condition.GroupUserId > 0)
            {
                query = query.Where(p => p.GroupUserId == condition.GroupUserId);
            }

            if (condition.Status.HasValue)
            {
                query = query.Where(p => p.Status == condition.Status.Value);
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
            var users = query.OrderBy(o => o.CreatedDateTime).Skip(condition.PageSize * condition.PageNumber).Take(condition.PageSize).ToList();
            return new PageList<UserModel>(Mapper.Map<List<UserModel>>(users), query.Count());
        }

        public void UpdateUser(UserModel userModel)
        {
            var userUpdated = _shoppingContext.Users.Find(userModel.Id);
            if (userUpdated != null)
            {
                userUpdated = Mapper.Map<User>(userModel);
                _shoppingContext.Users.AddOrUpdate(userUpdated);
                _shoppingContext.SaveChanges();
            }
        }
    }
}
