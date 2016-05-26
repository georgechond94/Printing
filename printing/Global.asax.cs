using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using printing.Comparers;
using printing.Models;

namespace printing
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static PrintingQueue Queue { get; set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (ApplicationDbContext appc = new ApplicationDbContext())
            {
                Queue = new PrintingQueue(appc.Prints.Where(x => !x.Stopped && !x.Finished), new PrintComparer());

            }

        }

    }
}
