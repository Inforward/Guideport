using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Portal.Infrastructure.Helpers;

namespace Portal.Web.Admin
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.With(s =>
            {
                // Ignore circular references (this is key for many-to-many Entity Framework entities)
                s.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                // Ensure Timezone is specified on dates so they can be correctly parsed in javascript
                s.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });
        }
    }
}
