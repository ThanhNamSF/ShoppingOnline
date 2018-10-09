using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DataAccess.Entity;
using DataAccess.Interfaces;

namespace BusinessLogic.BusinessLogics
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IUserService _userService;

        public UserBusinessLogic(IUserService userService)
        {
            _userService = userService;
        }
        public bool CheckUser(UserModel userModel)
        {
            var user = Mapper.Map<User>(userModel);
            return _userService.CheckUser(user);
        }
    }
}
