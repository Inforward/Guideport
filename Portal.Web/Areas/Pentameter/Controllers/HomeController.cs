using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Areas.Pentameter.Models;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Controllers;
using Portal.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Web.Areas.Pentameter.Controllers
{
    public class HomeController : BaseController
    {
        private const string BannerStateCacheKey = "Pentameter.Dashboard.Banner.State";
        private readonly ISurveyService _surveyService;
        private readonly IAffiliateService _affiliateService;
        private Affiliate _affiliate;

        private Affiliate Affiliate
        {
            get
            {
                if (_affiliate != null)
                    return _affiliate;

                _affiliate = _affiliateService.GetAffiliates(new AffiliateRequest { AffiliateID = AssistedUser.AffiliateID }).FirstOrDefault();

                return _affiliate;
            }
        }

        private AffiliateFeature QlikViewFeature
        {
            get { return Affiliate.Features.FirstOrDefault(f => f.FeatureID == (int)Features.QlikView); }
        }

        public HomeController(IUserService userService, ISurveyService surveyService, ILogger logger, IAffiliateService affiliateService)
            : base(userService, logger)
        {
            _surveyService = surveyService;
            _affiliateService = affiliateService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (HasProfileType(ProfileTypeValues.FinancialAdvisor))
            {
                return Dashboard();
            }

            var siteMap = ViewBag.SiteMap as SiteMap;

            return View("Index", siteMap);
        }

        [HttpGet]
        [ProfileTypeAuthorize(ProfileTypeValues.FinancialAdvisor)]
        public ActionResult Dashboard()
        {
            var viewModel = new DashboardViewModel();

            var siteMap = ViewBag.SiteMap as SiteMap;
            var surveySummary = _surveyService.GetSurveySummary(Settings.SurveyNames.BusinessAssessment, AssistedUser.UserID);

            foreach (var page in surveySummary.Pages)
            {
                var pillar = new PillarProgressModel()
                {
                    Title = page.PageName,
                    Tooltip = page.Tooltip,
                    Enabled = page.State != SurveyState.NotStarted,
                    Score = page.DisplayScore
                };

                if (siteMap != null)
                {
                    var contentPage = siteMap.Items.FirstOrDefault(p => p.Title.Equals(page.PageName, StringComparison.InvariantCultureIgnoreCase));

                    if (contentPage != null)
                    {
                        pillar.Url = contentPage.Url;
                    }
                }

                viewModel.Pillars.Add(pillar);
            }

            viewModel.IsQlikViewDashboardEnabled = QlikViewFeature != null && QlikViewFeature.IsEnabled;
            viewModel.QlikViewDocument = Affiliate.GetFeatureSetting(Features.QlikView, FeatureSettings.PentameterDashboardDocument);
            viewModel.AssessmentText = (surveySummary.State == SurveyState.NotStarted ? "Launch Assessment" : "Update Assessment");
            viewModel.AssessmentShortText = viewModel.AssessmentText.Substring(0, 6);
            viewModel.IsBannerExpanded = GetCurrentUser().GetCachedObject(BannerStateCacheKey, new ExpandCollapseState() { Expanded = true }).Expanded;

            return View("Dashboard", viewModel);
        }

        [HttpPost]
        public JsonResult SaveBannerState(bool expanded)
        {
            return JsonResponse(() =>
            {
                var item = new ExpandCollapseState() {Expanded = expanded};

                var cacheItem = new ObjectCache()
                {
                    Key = BannerStateCacheKey,
                    ValueSerialized = ObjectCache.SerializeItem(item),
                    UserID = CurrentUser.UserID
                };

                SaveUserObjectCache(cacheItem);
            });
        }

        [HttpPost]
        public JsonResult GetQlikViewUrl(string documentName)
        {
            return JsonResponse(() =>
            {
                var userId = AssistedUser.UserID.ToString();
                var settingValue = Affiliate.GetFeatureSetting(Features.QlikView, FeatureSettings.UserIdentifierField);

                if (!string.IsNullOrWhiteSpace(settingValue))
                {
                    if(settingValue.Equals("ProfileID", StringComparison.InvariantCultureIgnoreCase))
                        userId = AssistedUser.ProfileID;
                }

                if (string.IsNullOrEmpty(userId))
                    throw new ApplicationException("User does not have a valid User ID!");

                var url = string.Empty;
                var baseUrl = Affiliate.GetFeatureSetting(Features.QlikView, FeatureSettings.BaseUrl);
                var webTicket = GetWebTicket(userId);

                if (!string.IsNullOrEmpty(webTicket))
                {
                    url = string.Format("{0}/authenticate.aspx?type=html&try=/QvAJAXZfc/opendocnotoolbar.htm?document=accesspoint/{1}&webticket={2}&back={3}",
                            baseUrl, documentName, webTicket, string.Empty);
                }

                return url;
            });
        }

        private string GetWebTicket(string userId, string userGroups = null)
        {
            var baseUrl = Affiliate.GetFeatureSetting(Features.QlikView, FeatureSettings.BaseUrl);

            if (string.IsNullOrWhiteSpace(baseUrl))
                return null;

            var result = string.Empty;
            var webTicketUrl = baseUrl + "/GetWebTicket.aspx";
            var groups = new StringBuilder();

            // Override if TestUserID exists
            var testUserId = Settings.Get<string>("app:Qlikview.TestUserID");
            if (!string.IsNullOrEmpty(testUserId))
                userId = testUserId;

            if (!string.IsNullOrEmpty(userGroups))
            {
                groups.Append("<GroupList>");

                foreach (var group in userGroups.Split(','))
                {
                    groups.Append(string.Format("<string>{0}</string>", group));
                }

                groups.Append("</GroupList>");
                groups.Append("<GroupsIsNames>true</GroupsIsNames>");
            }

            var webTicketRequest = string.Format("<Global method=\"GetWebTicket\"><UserId>{0}</UserId>{1}</Global>", userId, groups);
            var client = (HttpWebRequest)WebRequest.Create(new Uri(webTicketUrl));
            client.PreAuthenticate = true;
            client.Method = "POST";
            client.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            try
            {
                using (var sw = new StreamWriter(client.GetRequestStream()))
                {
                    sw.WriteLine(webTicketRequest);
                }

                var response = new StreamReader(client.GetResponse().GetResponseStream());
                result = response.ReadToEnd();

                var doc = XDocument.Parse(result);

                return doc.Root.Element("_retval_").Value;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Unable to retrieve WebTicket from QlikView.\nTicketUrl: {0},\nTicketRequest: {1}.\nResult: {2}",
                        webTicketUrl, webTicketRequest, result), ex);
            }
        }
    }
}
