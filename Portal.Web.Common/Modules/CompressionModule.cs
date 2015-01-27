using System;
using System.IO.Compression;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Portal.Infrastructure.Configuration;
using Portal.Web.Common.Modules;

[assembly: PreApplicationStartMethod(typeof(CompressionModule), "LoadModule")]

namespace Portal.Web.Common.Modules
{
    public class CompressionModule : IHttpModule
    {
        public static void LoadModule()
        {
            DynamicModuleUtility.RegisterModule(typeof(CompressionModule));
        }

        #region IHttpModule Members

        public void Init(HttpApplication application)
        {
            if (!Settings.Get("app:CompressionModule.Enabled", false))
                return;

            application.BeginRequest += Application_BeginRequest;
        }

        public void Dispose()
        {
        }

        #endregion

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = context.Request;
            var response = context.Response;
            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (request.IsLocal || string.IsNullOrEmpty(acceptEncoding))
                return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-Encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-Encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}