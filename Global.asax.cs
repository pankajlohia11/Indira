using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Euro
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Add /MyVeryOwn/ folder to the default location scheme for STANDARD Views
            var razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault();
            razorEngine.ViewLocationFormats =
                razorEngine.ViewLocationFormats.Concat(new string[] {
            "~/Views/Admin/{1}/{0}.cshtml",
            "~/Views/Admin/{0}.cshtml",
             "~/Views/Agency/{1}/{0}.cshtml",
            "~/Views/Agency/{0}.cshtml",
             "~/Views/Trading/{1}/{0}.cshtml",
            "~/Views/Trading/{0}.cshtml",
            "~/Views/Purchase/{1}/{0}.cshtml",
            "~/Views/Purchase/{0}.cshtml",
            "~/Views/Sales/{1}/{0}.cshtml",
            "~/Views/Sales/{0}.cshtml",
             "~/Views/Mail/{1}/{0}.cshtml",
            "~/Views/Mail/{0}.cshtml",
               "~/Views/Reports/{1}/{0}.cshtml",
            "~/Views/Reports/{0}.cshtml",
             "~/Views/Calender/{1}/{0}.cshtml",
            "~/Views/Calender/{0}.cshtml"
                    // add other folders here (if any)
                }).ToArray();

            //// Add /MyVeryOwnPartialFolder/ folder to the default location scheme for PARTIAL Views
            //razorEngine.PartialViewLocationFormats =
            //    razorEngine.PartialViewLocationFormats.Concat(new string[] {
            //"~/MyVeryOwnPartialFolder/{0}.cshtml"
            //        // add other folders here (if any)
            //    }).ToArray();
        }
    }
}
