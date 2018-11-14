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
    public class CustomerService : ICustomerService
    {
        private readonly ShoppingContext _shoppingContext;

        public CustomerService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public void CreateCustomer(CustomerModel customerModel)
        {
            var customer = Mapper.Map<Customer>(customerModel);
            _shoppingContext.Customers.Add(customer);
            _shoppingContext.SaveChanges();
        }

        public IEnumerable<CustomerModel> GetAllCustomer()
        {
            var customer = _shoppingContext.Customers.AsNoTracking().ToList();
            return Mapper.Map<List<CustomerModel>>(customer);
        }

        public CustomerModel GetCustomerById(int id)
        {
            var customer = _shoppingContext.Customers.Find(id);
            return Mapper.Map<CustomerModel>(customer);
        }

        public CustomerModel GetCustomerLogin(CustomerModel customerModel)
        {
            var customerLogin = _shoppingContext.Customers.AsNoTracking().FirstOrDefault(x =>
                x.UserName.Equals(customerModel.UserName) && x.Password.Equals(customerModel.Password));
            return Mapper.Map<CustomerModel>(customerLogin);
        }
    }
}
