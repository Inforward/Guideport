using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal.Web.RouteConstraints
{
    public class CmsFileConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[parameterName] == null)
                return false;

            if (httpContext.Request.IsAjaxRequest())
                return false;

            var permalink = values[parameterName].ToString().ToLower();

            if (!permalink.StartsWith("/"))
                permalink = "/" + permalink;

            return permalink.Contains("/file/");
        }
    }
}