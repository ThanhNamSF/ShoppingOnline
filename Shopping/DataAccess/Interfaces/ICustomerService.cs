using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface ICustomerService
    {
        CustomerModel GetCustomerLogin(CustomerModel customerModel);
        IEnumerable<CustomerModel> GetAllCustomer();
        CustomerModel GetCustomerById(int id);
        void CreateCustomer(CustomerModel customerModel);
    }
}
