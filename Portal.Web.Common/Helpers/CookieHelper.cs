using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;

namespace Portal.Web.Common.Helpers
{
    /// <summary>
    /// MVC specific cookie helpers
    /// </summary>
    public static partial class CookieHelper
    {
        static CookieHelper()
        {
            UserDataCookieName = Settings.Get<string>("Cookie.CurrentUserData.Name");
            AssistedUserCookieName = Settings.Get<string>("Cookie.AssistedUserData.Name");
        }

        public static string UserDataCookieName;
        public static string AssistedUserCookieName;

        public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
        {
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            if (ticket == null)
                return 0;

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                                                            ticket.IsPersistent, string.Empty, ticket.CookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(newTicket);

            if (encryptedTicket == null)
                return 0;

            if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
                cookie.Domain = FormsAuthentication.CookieDomain;

            cookie.Value = encryptedTicket;
            responseBase.Cookies.Add(cookie);
            responseBase.SetCookieData(UserDataCookieName, userData);

            return encryptedTicket.Length;
        }

        public static T GetUserData<T>(this HttpRequestBase requestBase)
        {
            return requestBase.GetCookieData<T>(UserDataCookieName);
        }

        public static T GetCookieData<T>(this HttpRequestBase requestBase, string cookieName)
        {
            var cookie = requestBase.Cookies[cookieName];

            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return default(T);

            return JsonConvert.DeserializeObject<T>(cookie.Value.Unprotect(cookieName));
        }

        public static void SetCookieData<T>(this HttpResponseBase responseBase, string cookieName, T data)
        {
            responseBase.Cookies.Remove(cookieName);

            var cookie = new HttpCookie(cookieName, data.ToJson().Protect(cookieName))
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddTicks(FormsAuthentication.Timeout.Ticks)
            };

            if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
                cookie.Domain = FormsAuthentication.CookieDomain;

            responseBase.Cookies.Add(cookie);
        }

        public static void RemoveCookie(this HttpResponseBase responseBase, string cookieName)
        {
            var cookie = new HttpCookie(cookieName, string.Empty)
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(-1)
            };

            if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
                cookie.Domain = FormsAuthentication.CookieDomain;

            responseBase.Cookies.Add(cookie);
        }
    }

    /// <summary>
    /// Web API specific cookie helpers
    /// </summary>
    public static partial class CookieHelper
    {
        public static T GetUserData<T>(this HttpRequestMessage request)
        {
            return request.GetCookieData<T>(UserDataCookieName);
        }

        public static T GetCookieData<T>(this HttpRequestMessage request, string cookieName)
        {
            var cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();

            if (cookie == null || string.IsNullOrEmpty(cookie[cookieName].Value))
                return default(T);

            return JsonConvert.DeserializeObject<T>(cookie[cookieName].Value.Unprotect(cookieName));
        }

        public static void SetCookieData<T>(this HttpResponseMessage response, string cookieName, T data)
        {
            var cookie = new CookieHeaderValue(cookieName, data.ToJson().Protect(cookieName))
                {
                    Expires = DateTime.Now.AddTicks(FormsAuthentication.Timeout.Ticks),
                    HttpOnly = true
                };

            if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
                cookie.Domain = FormsAuthentication.CookieDomain;

            response.Headers.AddCookies(new [] { cookie });
        }
    }
}
