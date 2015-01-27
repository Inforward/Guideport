using System.Web.Mvc;
using System.Web.Routing;
using Portal.Web.RouteConstraints;

namespace Portal.Web.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "CmsRoute",
                "{*permalink}",
                new { controller = "Home", action = "File" },
                new { permalink = new CmsFileConstraint() }
            );

            routes.MapRoute(
                "Default", 
                "{controller}/{action}/{id}", 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
