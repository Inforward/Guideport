using System.Web.Security;
using System;
using System.Web.Mvc;
using Portal.Model;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Common.Filters.Mvc
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public virtual void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            var cookieData = context.Request.GetUserData<CookieUserData>();

            var isAuthorized = cookieData != null && cookieData.ToUser().IsAdmin();

            if (!isAuthorized)
            {
                if (context.IsDebuggingEnabled)
                {
                    filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }
    }
}