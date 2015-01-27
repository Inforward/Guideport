using System;
using System.Web.Mvc;

namespace Portal.Web.Areas
{
    public abstract class BaseAreaRegistration : AreaRegistration
    {
        abstract public string AreaSlug { get; }

        public override string AreaName
        {
            get { throw new NotImplementedException(); }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Route requests for survey's in each area to the root Survey Controller
            //context.MapRoute(
            //    AreaName + ".Survey",
            //    AreaSlug + "/survey",
            //    new { controller = "Survey", action = "Index", area = AreaName },
            //    new[] { "Portal.Web.Controllers" }
            //);

            context.MapRoute(
                AreaName + ".Default",
                AreaSlug + "/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = AreaName },
                new[] { string.Format("Portal.Web.Areas.{0}.Controllers", AreaName) }
            );
        }
    }
}