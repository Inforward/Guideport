using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.ActionResults;
using Portal.Web.Models;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class ContentController : BaseController
    {
        #region Private Members

        private readonly ICmsService _cmsService;
        private readonly IFileService _fileService;

        #endregion

        #region Constructor

        public ContentController(IUserService userService, ICmsService cmsService, ILogger logger, IFileService fileService)
            : base(userService, logger)
        {
            _cmsService = cmsService;
            _fileService = fileService;
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(string permalink)
        {
            var content = CurrentContent;

            if (content == null)
                throw new HttpException(404, "Content does not exist.");

            // Check / Set e-Tags for files only
            if (content.ContentType == ContentType.File)
            {
                var requestedETag = Request.Headers["If-None-Match"];
                var eTag = string.Format("\"{0}-{1}\"", content.ModifyDate.Ticks, AssistedUser.UserID);

                if (!string.IsNullOrEmpty(requestedETag) && requestedETag.Equals(eTag))
                    return new HttpStatusCodeResult(HttpStatusCode.NotModified);

                // Required for getting eTags in the response:
                Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);

                // Required to ensure no server caching:
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));

                // Set it!
                Response.Cache.SetETag(eTag);

                return new FileServiceResult(content);
            }

            return View("~/Views/Content/Index.cshtml", ToContentViewModel(content));
        }

        [HttpGet]
        [Route("content/{contentId}/preview/{versionId}")]
        public ActionResult Preview(int contentId, int versionId)
        {
            var request = new SiteContentRequest()
            {
                SiteContentID = contentId,
                IncludeSite = true,
                IncludeSiteTemplates = true
            };

            var content = _cmsService.GetSiteContent(request);

            if (content == null)
                throw new Exception("Invalid Content ID");

            var version = content.Versions.FirstOrDefault(v => v.SiteContentVersionID == versionId);

            if (version == null)
                throw new Exception("Invalid Version ID");

            // Now set the site map (so we can render any menus, etc.)
            var siteMap = _cmsService.GetSiteMap(content.SiteID, CurrentUser.ProfileTypeID, CurrentUser.AffiliateID);

            SetSiteMap(siteMap);

            var viewModel = new ContentViewModel()
            {
                Title = string.Format("{0} | {1}", content.Site.SiteName, content.Title),
                PageTitle = content.Title,
                LayoutPath = version.SiteTemplate != null ? version.SiteTemplate.LayoutPath : "~/Views/Shared/Layouts/_PortalDefault.cshtml",
                Html = version.ContentText
            };

            return View("~/Views/Content/Index.cshtml", viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("content/file/{fileId}")]
        public ActionResult File(int fileId)
        {
            var file = _fileService.DownloadFile(new FileRequest() {FileID = fileId});

            var requestedETag = Request.Headers["If-None-Match"];
            var eTag = string.Format("\"file-{0}\"", file.FileID);

            if (!string.IsNullOrEmpty(requestedETag) && requestedETag.Equals(eTag))
                return new HttpStatusCodeResult(HttpStatusCode.NotModified);

            // Required for getting eTags in the response:
            Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);

            // Required to ensure no server caching:
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));

            // Set it!
            Response.Cache.SetETag(eTag);

            return new Portal.Web.Common.Results.FileServiceResult(file);
        }

        #endregion

        #region Mapping Methods

        private static ContentViewModel ToContentViewModel(SiteContentViewModel content)
        {
            return new ContentViewModel()
            {
                Title = string.Format("{0} | {1}", content.SiteName, content.Title),
                PageTitle = content.Title,
                LayoutPath = !string.IsNullOrEmpty(content.TemplatePath) ? content.TemplatePath : "~/Views/Shared/Layouts/_PortalDefault.cshtml",
                Scripts = content.ContentScripts,
                Styles = content.ContentStyles,
                Html = content.ContentHtml,
            };
        }

        #endregion
    }
}
