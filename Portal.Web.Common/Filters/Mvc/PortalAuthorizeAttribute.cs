using System;
using System.Web.Mvc;
using Portal.Model;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Common.Filters.Mvc
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class PortalAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly string[] _authorizedRoles;

        public PortalAuthorizeAttribute(params string[] authorizedRoles)
        {
            _authorizedRoles = authorizedRoles;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            var cookieData = context.Request.GetUserData<CookieUserData>();

            var isAuthorized = cookieData != null && cookieData.ToUser().IsInRole(_authorizedRoles);

            if (!isAuthorized)
            {
                filterContext.Result = new RedirectResult("~/error/unauthorized");
            }
        }
    }
}