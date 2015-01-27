using System;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Security;
using Portal.Infrastructure.Helpers;

namespace Portal.Model
{
    internal class RequestInfo
    {
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string RemoteIP { get; set; }
        public string Browser { get; set; }
        public string RequestMethod { get; set; }
        public string ScriptName { get; set; }
        public string PostData { get; set; }
        public string QueryString { get; set; }
        public string Referrer { get; set; }
        public string SessionData { get; set; }
        public string CookieData { get; set; }
        public string FormData { get; set; }
        public string Source { get; set; }
        public int UserID { get; set; }
        public int LoginUserID { get; set; }
        public int PortalID { get; set; }
        public int PageID { get; set; }
        public int HasAccess { get; set; }

        public static RequestInfo FromHttpContext(HttpContext context)
        {
            var request = context.Request;

            var info = new RequestInfo();

            try
            {
                info.ServerName = Environment.MachineName;
                info.ServerIP = request.ServerVariables["LOCAL_ADDR"];
                info.RemoteIP = request.ServerVariables["REMOTE_ADDR"];
                info.Browser = string.Format("{0} {1}", request.Browser.Browser, request.Browser.Version);
                info.RequestMethod = request.ServerVariables["REQUEST_METHOD"];
                info.Referrer = request.UrlReferrer.AbsoluteUri;

                if (request.Url != null)
                {
                    info.ScriptName = request.Url.OriginalString;
                }
            }
            catch { }

            info.PostData = request.Form.ToString();
            info.QueryString = request.QueryString.ToString().TrimToLength(1000, true);

            if (request.UrlReferrer != null)
            {
                info.Referrer = request.UrlReferrer.PathAndQuery;
            }

            if (context.Session != null && context.Session["Portal.CurrentUser"] != null)
            {
                var user = context.Session["Portal.CurrentUser"] as User;

                if (user != null)
                {
                    info.UserID = user.UserID;
                }
            }

            if (context.User != null)
            {
                var identity = context.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    var claimsData = new StringBuilder();

                    foreach (var claim in identity.Claims)
                    {
                        claimsData.AppendLine(string.Format("{0} = {1}", claim.Type, claim.Value));
                    }

                    info.Source = claimsData.ToString();
                }
            }

            return info;
        }
    }
}
