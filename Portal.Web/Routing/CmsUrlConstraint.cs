using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Portal.Services.Contracts;

namespace Portal.Web.Routing
{
    public class CmsUrlConstraint : IRouteConstraint
    {
        private static readonly ICmsService _cmsService;

        static CmsUrlConstraint()
        {
            _cmsService = NinjectWebCommon.Resolve<ICmsService>();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext.Request.IsAjaxRequest())
                return false;

            if (values[parameterName] != null)
            {
                var permalink = values[parameterName].ToString();

                return _cmsService.IsContentUrl(permalink);
            }

            return false;
        }
    }
}