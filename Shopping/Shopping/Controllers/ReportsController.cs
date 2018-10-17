using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.WebApi;

namespace Shopping.Controllers
{
    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration configurationInstance;
        public ReportsController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }
        static ReportsController()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/");
            var reportsPath = Path.Combine(appPath, "Reports");

            var resolver = new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());
            configurationInstance = new ReportServiceConfiguration
            {
                HostAppId = "ShoppingOnlineApp",
                Storage = new FileStorage(),
                ReportResolver = resolver
            };
        }
    }
}