using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.Infrastructure.Configuration;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Admin.Models;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Common.Helpers;
using Portal.Web.Common.Results;

namespace Portal.Web.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
    {
        private readonly ISurveyService _surveyService;
        private readonly IReportService _reportService;
        private readonly ICmsService _cmsService;
        private readonly IFileService _fileService;

        public HomeController(ISurveyService surveyService, IReportService reportService, ICmsService cmsService, IFileService fileService)
        {
            _surveyService = surveyService;
            _reportService = reportService;
            _cmsService = cmsService;
            _fileService = fileService;
        }

        public ActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Surveys = _surveyService.GetSurveys().Where(s => s.IsActive), 
                Reports = _reportService.GetViews(),
                CurrentUser = GetCurrentUser(),
                Config = new ExpandoObject()  
            };

            viewModel.Config.GuideportUrl = Settings.Get<string>("app:Guideport.Url");

            return View(viewModel);
        }

        public ActionResult File(string permalink)
        {
            if (!permalink.StartsWith("/"))
                permalink = "/" + permalink;

            var request = new SiteContentRequest() { Permalink = permalink };

            var content = _cmsService.GetSiteContents(request)
                                .Where(s => s.SiteContentStatusID == (int)ContentStatus.Published || s.SiteContentStatusID == (int)ContentStatus.Draft)
                                .FirstOrDefault(s => s.SiteContentTypeID == (int)ContentType.File);

            if (content == null || content.FileID == null)
                return new EmptyResult();

            var file = _fileService.DownloadFile(new FileRequest { FileID = content.FileID.Value });

            return new FileServiceResult(file);
        }

        [Route("get-file/{fileId:int}")]
        public ActionResult File(int fileId)
        {
            var file = _fileService.DownloadFile(new FileRequest { FileID = fileId });

            return new FileServiceResult(file);
        }

        private User GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            var cookieData = Request.GetUserData<CookieUserData>();

            if (cookieData == null)
                throw new Exception("Could not retrieve user data from cookie.  Cannot continue.");

            return cookieData.ToUser();
        }
    }
}