using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.ActionResults;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Common.Helpers;
using Portal.Web.Models;
using Portal.Web.Models.AdvisorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Web.Controllers
{
    [PortalAuthorize(PortalRoleValues.AdvisorView)]
    public class AdvisorViewController : BaseController
    {
        private readonly IGroupService _groupService;
        private readonly IAffiliateService _affiliateService;
        private readonly ICmsService _cmsService;
        private const int NoSelectedGroupId = int.MaxValue;

        public AdvisorViewController(IUserService userService, ILogger logger, IGroupService groupService, IAffiliateService affiliateService, ICmsService cmsService)
            : base(userService, logger)
        {
            _groupService = groupService;
            _affiliateService = affiliateService;
            _cmsService = cmsService;
        }

        [HttpGet]
        [Route("advisor-view", Name="AdvisorView")]
        public ActionResult Index()
        {
            var isRestricted = CurrentUser.IsRestricted(PortalRoleValues.AdvisorView);

            var groups = isRestricted ? GetAccessibleGroups() :  _groupService.GetGroups(new GroupRequest()).Groups.OrderBy(g => g.Name).ToList();

            ViewBag.IsRestricted = isRestricted;
            ViewBag.DefaultGroupID = string.Empty;

            if (isRestricted)
            {
                groups.Insert(0, new Group() { Name = "", GroupID = NoSelectedGroupId });
                ViewBag.DefaultGroupID = NoSelectedGroupId;
            }

            var viewModel = new AdvisorViewModel
            {
                Groups = groups,
                Affiliates = _affiliateService.GetAffiliates(new AffiliateRequest())
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Search(SearchFilters filters)
        {
            var criteria = new UserRequest
                {
                    FirstName = filters.FirstName.SafeTrim(),
                    LastName = filters.LastName.SafeTrim(),
                    ProfileTypeID = (int)ProfileTypes.FinancialAdvisor,
                    AffiliateID = CurrentUser.AffiliateID,
                    IncludeGroups = true,
                    PageSize = filters.PageSize,
                    Page = filters.Page,
                    Skip = filters.Skip,
                    Take = filters.Take,
                    Sort = filters.Sort
                };

            if (!string.IsNullOrWhiteSpace(filters.GroupID))
            {
                var selectedGroupId = int.Parse(filters.GroupID);
                var groupIds = new List<int> {selectedGroupId};

                if (selectedGroupId == NoSelectedGroupId)
                {
                    // Set groupIds to all accessible groups for this user
                    var groups = GetAccessibleGroups();

                    if (!groups.IsNullOrEmpty())
                        groupIds = groups.Select(g => g.GroupID).ToList();
                }

                // Retrieve child groups from each group hierarchy
                 var allGroupIds = _groupService.GetGroupsFromHierarchy(groupIds).Select(g => g.GroupID).ToList();

                 criteria.MemberGroupIDs = !allGroupIds.IsNullOrEmpty() ? allGroupIds : groupIds;
            }

            if (!CurrentUser.IsRestricted(PortalRoleValues.AdvisorView))
            {
                criteria.AffiliateID = !string.IsNullOrEmpty(filters.AffiliateID) ? int.Parse(filters.AffiliateID) : 0;
            }

            var response = _userService.GetUsers(criteria);

            return new JsonNetResult(new {                
                Results = response.Users.Select(ToUserSearchResultViewModel),
                Total = response.TotalRecordCount
            });
        }

        [HttpGet]
        public ActionResult StartSession(int id)
        {
            var request = new UserRequest
            {
                UserID = id,
                IncludeApplicationRoles = true,
                IncludeGroups = true,
                IncludeObjectCache = true,
                IncludeAffiliates = true,
                IncludeAffiliateLogos = true,
                IncludeAffiliateFeatures = true
            };

            var user = _userService.GetUser(request);

            if (user == null)
                throw new Exception("Invalid User ID: " + id);

            var userData = user.ToCookieUserData();
            var siteMap = _cmsService.GetSiteMap((int)Sites.Pentameter, user.ProfileTypeID, user.AffiliateID);

            userData.Connect2ClientsEnabled = user.Affiliate.HasFeature(Features.Connect2Clients);
            userData.Connect2ClientsMessage = user.Affiliate.GetFeatureSetting(Features.Connect2Clients, FeatureSettings.DisabledMessage);
            userData.SuccessionPlanningEnabled = siteMap != null && siteMap.Items.AnyRecursive(p => p.Title == "Succession Planning");

            Response.SetCookieData(CookieHelper.AssistedUserCookieName, userData);

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [HttpPost]
        public ActionResult EndSession()
        {
            if (Request.Cookies[CookieHelper.AssistedUserCookieName] != null)
            {
                var cookie = new HttpCookie(CookieHelper.AssistedUserCookieName)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(-1)
                };

                if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
                    cookie.Domain = FormsAuthentication.CookieDomain;

                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public static UserSearchResultViewModel ToUserSearchResultViewModel(User u)
        {
            var viewModel = new UserSearchResultViewModel()
            {
                DisplayName = u.DisplayName.Capitalize(),
                UserId = u.UserID,
                PrimaryPhone = u.PrimaryPhone,
                BusinessConsultantDisplayName = u.BusinessConsultantDisplayName,
                AffiliateName = u.AffiliateName,
                City = u.City.Capitalize(),
                State = u.State.ToUpper()
            };

            return viewModel;
        }

        private List<Group> GetAccessibleGroups()
        {
            return _groupService.GetAccessibleGroups(CurrentUser.UserID).OrderBy(g => g.Name).ToList();
        }
    }
}