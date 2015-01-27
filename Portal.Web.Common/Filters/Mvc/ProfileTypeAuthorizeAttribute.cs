using System;
using System.Linq;
using System.Web.Mvc;
using Portal.Model;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Common.Filters.Mvc
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class ProfileTypeAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly string[] _authorizedProfileTypes;

        public ProfileTypeAuthorizeAttribute(params string[] authorizedClaims)
        {
            _authorizedProfileTypes = authorizedClaims;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.AllowAnonymous()) return;

            var context = filterContext.RequestContext.HttpContext;
            var assistedCookieData = context.Request.GetCookieData<CookieUserData>(CookieHelper.AssistedUserCookieName);
            var cookieData = assistedCookieData ?? context.Request.GetUserData<CookieUserData>();

            var isAuthorized = cookieData != null && cookieData.ToUser().HasProfileType(_authorizedProfileTypes);

            if (!isAuthorized)
            {
                filterContext.Result = new RedirectResult("~/error/unauthorized");
            }
        }
    }
}