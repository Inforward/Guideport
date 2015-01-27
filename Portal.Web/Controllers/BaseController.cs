using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.ActionResults;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Common.Helpers;
using Portal.Web.Filters;
using Portal.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Web.Controllers
{
    [HandleExceptions]
    [ProfileTypeAuthorize(ProfileTypeValues.FinancialAdvisor, ProfileTypeValues.Employee, ProfileTypeValues.BranchAssistant)]
    public abstract class BaseController : Controller
    {
        #region Private Members

        private readonly ILogger _logger;
        protected readonly IUserService _userService;
        private User _currentUser;
        private User _assistedUser;
        private SiteContentViewModel _currentContent;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the actual logged in user.  
        /// Contains minimal user data.  If full user data is required, call GetCurrentUser() instead
        /// </summary>
        public User CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;

                if (_currentUser == null)
                {
                    var cookieData = Request.GetUserData<CookieUserData>();

                    if (cookieData == null)
                        throw new Exception("Could not retrieve user data from cookie.  Cannot continue.");

                    _currentUser = cookieData.ToUser();
                }

                return _currentUser;
            }
        }

        /// <summary>
        /// Represents the user being assisted via Advisor View. 
        /// Defaults to CurrentUser if no user is being assisted
        /// Contains minimal user data.  If full user data is required, call GetAssistedUser() instead
        /// </summary>
        public User AssistedUser
        {
            get
            {
                if (_assistedUser == null)
                {
                    var cookieData = Request.GetCookieData<CookieUserData>(CookieHelper.AssistedUserCookieName);

                    if (cookieData != null)
                    {
                        _assistedUser = cookieData.ToUser();
                    }
                }

                return _assistedUser ?? CurrentUser;
            }
        }

        protected AnalyticsViewModel AnalyticsInfo
        {
            get
            {
                if (AssistedUser == null)
                    return null;

                return new AnalyticsViewModel()
                {
                    ProfileID = CurrentUser.ProfileID,
                    UserName = CurrentUser.DisplayName,
                    ProfileType = CurrentUser.ProfileTypeName,
                    AdvisorName = AssistedUser.DisplayName,
                    BusinessConsultantName = AssistedUser.BusinessConsultantDisplayName,
                    AdvisorAffiliateName = AssistedUser.AffiliateName
                };
            }
        }

        protected SiteContentViewModel CurrentContent
        {
            get
            {
                if (_currentContent != null)
                    return _currentContent;

                if (Request.Url != null)
                {
                    _currentContent = Using<ICmsService>().GetSiteContentViewModel(Request.Url.AbsolutePath, AssistedUser.ProfileTypeID, AssistedUser.AffiliateID);
                }

                return _currentContent;
            }
        }

        protected bool IsAssisting
        {
            get
            {
                return (CurrentUser != null && AssistedUser != null && CurrentUser.UserID != AssistedUser.UserID);
            }
        }

        protected Sites CurrentSite
        {
            get
            {
                if (CurrentContent != null)
                    return (Sites) CurrentContent.SiteID;

                var area = ControllerContext.RouteData.DataTokens["area"];
                var site = Sites.Guideport;

                if (area != null)
                {
                    switch (area.ToString())
                    {
                        case "Pentameter":
                            site = Sites.Pentameter;
                            break;
                        case "Retirement":
                            site = Sites.GuidedRetirementSolutions;
                            break;
                    }
                }

                return site;
            }
        }

        #endregion

        #region Constructor

        protected BaseController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        #endregion

        #region Overrides

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!User.Identity.IsAuthenticated)
            {
                Redirect(FormsAuthentication.LoginUrl);
                return;
            }

            if (requestContext.HttpContext.Request.IsAjaxRequest())
                return;

            ViewBag.AdminEnabled = CurrentUser.IsAdmin();
            ViewBag.ContentAdminEnabled = CurrentUser.IsInRole(PortalRoleValues.ContentAdmin);
            ViewBag.AdvisorViewEnabled = CurrentUser.IsInRole(PortalRoleValues.AdvisorView);
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.AssistedUser = AssistedUser;
            ViewBag.IsAssisting = IsAssisting;
            ViewBag.AnalyticsInfo = AnalyticsInfo;
            ViewBag.IsFinancialAdvisor = (CurrentUser != null && CurrentUser.ProfileTypeID == (int)ProfileTypes.FinancialAdvisor);
            ViewBag.JsonSettings = new JsonSettings()
            {
                KeepAliveSettings = new KeepAliveSettings()
                {
                    Interval = Settings.Get<int>("app:KeepAlive.Interval.Seconds") * 1000,
                    PingUrl = Url.Action("KeepAlive", "Home", new { Area = "" })
                },
                SessionMonitorSettings = new SessionMonitorSettings()
                {
                    WarningTimeout = Settings.Get<int>("app:SessionMonitor.WarningTimeout.Seconds") * 1000,
                    ExpirationTimeout = Settings.Get<int>("app:SessionMonitor.ExpirationTimeout.Seconds") * 1000
                },
                AutoSaveSettings = new AutoSaveSettings()
                {
                    Interval = Settings.Get<int>("app:AutoSave.Interval.Seconds") * 1000
                },
                AdminConsoleUrl = Settings.Get<string>("app:AdminConsole.Url")
            };

            var userData = Request.GetCookieData<CookieUserData>(CookieHelper.AssistedUserCookieName) ?? Request.GetUserData<CookieUserData>();

            ViewBag.EnableConnect2Clients = userData.Connect2ClientsEnabled;
            ViewBag.Connect2ClientsTooltip = userData.Connect2ClientsMessage;
            ViewBag.SuccessionPlanningEnabled = userData.SuccessionPlanningEnabled;

            SetSiteViewBagData();
        }

        #endregion

        #region Public Methods

        public bool HasProfileType(params string[] authorizedProfiles)
        {
            return AssistedUser.HasProfileType(authorizedProfiles);
        }

        public User GetCurrentUser()
        {
            return _userService.GetUserByUserId(CurrentUser.UserID);
        }

        public User GetAssistedUser()
        {
            return IsAssisting ?  _userService.GetUserByUserId(AssistedUser.UserID) : GetCurrentUser();
        }

        #endregion

        #region Protected Methods

        protected T Using<T>() where T : class
        {
            var handler = NinjectWebCommon.Resolve<T>();

            if (handler == null)
            {
                throw new NullReferenceException("Unable to resolve type with Ninject; type " + typeof(T).Name);
            }
            return handler;
        }

        protected void SetSiteViewBagData()
        {
            var currentContent = CurrentContent;

            if (currentContent != null && currentContent.ContentType == ContentType.File)
                return;

            if (currentContent != null)
            {
                SetSiteMap(currentContent.SiteMap);

                if (currentContent.SiteID == (int)Sites.Pentameter)
                {
                    SetPentameterSiteViewBagData(currentContent);
                }
            }
            else
            {
                var siteMap = Using<ICmsService>().GetSiteMap((int)CurrentSite, CurrentUser.ProfileTypeID, CurrentUser.AffiliateID);

                SetSiteMap(siteMap);
            }
        }

        protected void SetSiteMap(SiteMap siteMap)
        {
            ViewBag.SiteID = siteMap.SiteID;
            ViewBag.SiteMap = siteMap;
            ViewBag.SiteName = siteMap.SiteName;
            ViewBag.ContactUrl = siteMap.SiteContactUrl;
            ViewBag.TermsUrl = siteMap.SiteTermsUrl;
        }

        protected void SetPentameterSiteViewBagData(SiteContentViewModel currentContent)
        {
            if (!HasProfileType(ProfileTypeValues.FinancialAdvisor) || currentContent == null || currentContent.ContentType == ContentType.File)
                return;

            var rootPage = currentContent.SiteMap.Items.GetRootPage(currentContent.SiteContentID);
            var surveySummary = Using<ISurveyService>().GetSurveySummary(Settings.SurveyNames.BusinessAssessment, AssistedUser.UserID);

            if (surveySummary != null && surveySummary.State != SurveyState.NotStarted)
            {
                ViewBag.BusinessAssessmentSummary = surveySummary.Pages.FirstOrDefault(p => p.PageName.Equals(rootPage.Title));
            }
        }

        protected JsonResult JsonResponse(Action action)
        {
            var response = new JsonResponse();

            try
            {
                action();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.FullMessage();

                _logger.Log(ex);
            }

            return new JsonNetResult(response);
        }

        protected JsonResult JsonResponse<T>(Func<T> method)
        {
            var response = new JsonResponse();

            try
            {
                response.Data = method();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.FullMessage();

                _logger.Log(ex);
            }

            return new JsonNetResult(response) { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        protected void SaveUserObjectCache(ObjectCache cacheItem)
        {
            _userService.SaveUserObjectCache(cacheItem);
        }

        #endregion
    }
}
