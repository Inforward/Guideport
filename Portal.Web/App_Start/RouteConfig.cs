using System.Web.Mvc;
using System.Web.Routing;
using Portal.Web.Routing;

namespace Portal.Web
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
                new { controller = "Content", action = "Index" }, 
                new { permalink = new CmsUrlConstraint() },
                new[] { "Portal.Web.Controllers" }
            );
            
            routes.MapRoute(
               "Auth",
               "Auth/Saml/{action}",
               new { controller = "Auth", action = "Login" },
               new[] { "Portal.Web.Controllers" }
            );

            routes.MapRoute(
               "Saml",
               "Saml/{action}",
               new { controller = "Auth", action = "Login" },
               new[] { "Portal.Web.Controllers" }
            );

            AreaRegistration.RegisterAllAreas();

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Portal.Web.Controllers" }
            );
        }
    }
}