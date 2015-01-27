using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using EvoPdf;
using Portal.Domain.Services;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Web.Common.Helpers;

namespace Portal.Web.PdfGenerators
{
    public abstract class BasePdfGenerator : IPdfGenerator
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public Services.Contracts.IFileService FileService { get; set; }
        protected HtmlToPdfConverter PdfConverter { get; private set; }

        public virtual byte[] GeneratePdf()
        {
            return PdfConverter.ConvertUrl(GetUrl(Url));
        }

        protected virtual void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            PdfConverter = new HtmlToPdfConverter
            {
                NavigationTimeout = 300,
                LicenseKey = Settings.EvoPdfLicenseKey
            };

            PdfConverter.With(pdf =>
            {
                pdf.PdfDocumentInfo.AuthorName = "Cetera Financial Group, Inc.";
                pdf.PdfDocumentInfo.Title = Title;
                pdf.PdfDocumentInfo.CreatedDate = DateTime.Now;
                pdf.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                pdf.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.NoCompression;
                pdf.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

                pdf.PdfDocumentOptions.TopMargin = 18;
                pdf.PdfDocumentOptions.RightMargin = 18;
                pdf.PdfDocumentOptions.BottomMargin = 18;
                pdf.PdfDocumentOptions.LeftMargin = 18;

                pdf.PdfDocumentOptions.TopSpacing = 10;
                pdf.PdfDocumentOptions.BottomSpacing = 20;

                pdf.PdfDocumentOptions.FitWidth = true;
                pdf.PdfDocumentOptions.EmbedFonts = false;
                pdf.PdfDocumentOptions.LiveUrlsEnabled = false;
                pdf.JavaScriptEnabled = true;
                pdf.PdfDocumentOptions.JpegCompressionEnabled = false;
                pdf.PdfBookmarkOptions.HtmlElementSelectors = new[] { "h2" };

                AddCookies();
            });
        }

        protected virtual void AddCookies()
        {
            var cookieNames = new[]
            {
                FormsAuthentication.FormsCookieName, 
                CookieHelper.AssistedUserCookieName,
                CookieHelper.UserDataCookieName,
                "FedAuth", 
                "FedAuth1"
            };

            var request = HttpContext.Current.Request;

            foreach (var cookieName in cookieNames.Where(cookieName => request.Cookies[cookieName] != null))
            {
                PdfConverter.HttpRequestCookies.Add(cookieName, request.Cookies[cookieName].Value);
            }
        }

        protected virtual Image GetLogo(AffiliateLogoTypes logoType)
        {
            var logo = User.Affiliate.Logos.FirstOrDefault(l => l.AffiliateLogoTypeID == (int) logoType);

            if (logo != null && logo.FileID > 0)
            {
                var file = FileService.DownloadFile(new FileRequest() {FileID = logo.FileID});

                return Image.FromStream(file.Stream);
            }

            return null;
        }

        private string GetUrl(string url)
        {
            var urlToConvert = Settings.Get<string>("app:PdfBaseUrl");

            if (!urlToConvert.EndsWith("/"))
                urlToConvert += "/";

            var decodedUrl = HttpUtility.UrlDecode(url);

            if (!string.IsNullOrEmpty(decodedUrl))
            {
                if (decodedUrl.StartsWith("/"))
                    decodedUrl = decodedUrl.Substring(1);

                urlToConvert += decodedUrl;
            }

            return urlToConvert;
        }
    }
}