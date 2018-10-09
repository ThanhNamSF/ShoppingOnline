using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.BusinessLogics;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Services;
using SimpleInjector;

namespace BusinessLogic.Infrastructure
{
    public static class BusinessLogicDependencyRegistration
    {
        public static void Register(Container container)
        {
            container.Register<IUserBusinessLogic, UserBusinessLogic>(Lifestyle.Scoped);
            container.Register<IProductBusinessLogic, ProductBusinessLogic>(Lifestyle.Scoped);
            container.Register<IProductCategoryBusinessLogic, ProductCategoryBusinessLogic>(Lifestyle.Scoped);
        }
    }
}
