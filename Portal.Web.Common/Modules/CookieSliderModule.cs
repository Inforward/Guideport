using System;
using System.Web;
using System.Web.Security;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Portal.Infrastructure.Configuration;
using Portal.Model;
using Portal.Web.Common.Helpers;
using Portal.Web.Common.Modules;

[assembly: PreApplicationStartMethod(typeof(CookieSliderModule), "LoadModule")]

namespace Portal.Web.Common.Modules
{
    public class CookieSliderModule : IHttpModule
    {
        public static void LoadModule()
        {
            DynamicModuleUtility.RegisterModule(typeof(CookieSliderModule));
        }

        #region IHttpModule Members

        public void Init(HttpApplication application)
        {
            if (!Settings.Get("app:CookieSliderModule.Enabled", false))
                return;

            application.BeginRequest += Application_BeginRequest;
        }

        public void Dispose()
        {
        }

        #endregion

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            if (HttpContext.Current == null)
                return;

            var request = new HttpRequestWrapper(HttpContext.Current.Request);
            var response = new HttpResponseWrapper(HttpContext.Current.Response);

            var cookieNames = new[] { CookieHelper.AssistedUserCookieName, CookieHelper.UserDataCookieName };

            foreach (var cookieName in cookieNames)
            {
                var data = request.GetCookieData<CookieUserData>(cookieName);

                if(data == null)
                    continue;

                var ticksElapsed = DateTime.Now.Ticks - data.Issued.Ticks;
                var halfTime = FormsAuthentication.Timeout.Ticks / 2;

                if (ticksElapsed >= halfTime)
                {
                    data.Issued = DateTime.Now;

                    response.SetCookieData(cookieName, data);
                }
            }
        }
    }
}