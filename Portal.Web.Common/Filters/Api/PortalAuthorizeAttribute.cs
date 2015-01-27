using System;
using System.Web.Http.Controllers;
using Portal.Model;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Common.Filters.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PortalAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        private readonly string[] _authorizedRoles;

        public PortalAuthorizeAttribute(params string[] roles)
        {
            _authorizedRoles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var cookieData = actionContext.Request.GetUserData<CookieUserData>();

            return cookieData != null && cookieData.ToUser().IsInRole(_authorizedRoles);
        }
    }
}