using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.Services;
using SimpleInjector;

namespace DataAccess.Infrastructure
{
    public static class ServiceDependecyRegistration
    {
        public static void Register(Container container)
        {
            container.Register<ShoppingContext>(Lifestyle.Scoped);

            container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.Register<IProductService, ProductService>(Lifestyle.Scoped);
            container.Register<IProductCategoryService, ProductCategoryService>(Lifestyle.Scoped);
            container.Register<IReceiveService, ReceiveService>(Lifestyle.Scoped);
            container.Register<IDeliveryService, DeliveryService>(Lifestyle.Scoped);
        }
    }
}
