using System;
using System.Configuration;
using AutoMapper;
using DataAccess.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Twilio;
using Twilio.AspNet.Mvc;

namespace Shopping
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Twilio
            TwilioClient.Init(
                "AC5a90f4c08845d2654bd03e6ae0804777",
                "cb8b72b5dc75f921421b7b90dcbf7646"
            );
            //Telerik Report
            Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(System.Web.Http.GlobalConfiguration.Configuration);
            //AutoMapper
            Mapper.Initialize(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            //Ioc for MVC
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            ServiceDependecyRegistration.Register(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
