using ComponentSpace.SAML2.Configuration;
using Portal.Infrastructure.Configuration;
using System.Deployment.Internal.CodeSigning;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Portal.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Upn;

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

            var samlConfigLocation = Settings.Get<string>("Saml.Configuration.FileLocation");
            if (!string.IsNullOrEmpty(samlConfigLocation))
                SAMLConfiguration.Load(samlConfigLocation);
        }

        protected void Application_EndRequest()
        {
            // For ajax requests, check for 302 errors and respond with a custom 308 
            // that we will check for in javascript and redirect accordingly.
            // 302s are typically generated when the user is no longer authenticated

            var context = new HttpContextWrapper(Context);

            if (Context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {
                var location = Context.Response.Headers["Location"];

                Context.Response.Clear();
                Context.Response.StatusCode = 308;
                Context.Response.AddHeader("Location", location);
            }
        }
    }
}