using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface IUserBusinessLogic
    {
        bool CheckUser(UserModel userModel);
    }
}
