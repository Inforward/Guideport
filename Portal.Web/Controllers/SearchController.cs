using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.ActionResults;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class SearchController : BaseController
    {
        #region Private Members

        private readonly ICmsService _cmsService;

        #endregion

        #region Constructor

        public SearchController(IUserService userService, ICmsService cmsService, ILogger logger)
            : base(userService, logger)
        {
            _cmsService = cmsService;
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonNetResult JsonSearch(string searchText, string siteName, int textMaxChars = 100)
        {
            var content = new List<SiteContentViewModel>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = Regex.Matches(searchText, @"([\""\''])(.+?)([\""\''])|[^ ,]+")
                                   .Cast<Match>()
                                   .Select(m => m.Groups[2].Success ? m.Groups[2].Value : m.Groups[0].Value)
                                   .ToArray();

                var request = new SiteContentRequest()
                {
                    SearchTerms = searchTerms.ToList(),
                    ContentDocumentTypes = Settings.SearchableDocumentTypes.ToList(),
                    SiteName = siteName != "All" ?  siteName : null,
                    ProfileTypeID = AssistedUser.ProfileTypeID,
                    AffiliateID = AssistedUser.AffiliateID,
                    MaxContentCharacters = textMaxChars
                };

                content = _cmsService.SearchSiteContents(request).ToList();
            }

            return new JsonNetResult(content.OrderByDescending(r => r.SearchRank).ThenBy(r => r.Title).Select(r => new
                {
                    r.Title,
                    Text = r.ContentHtml,
                    r.SiteName,
                    SiteIconCssClass = r.SiteName.ToLower().Replace(" ", "-"),
                    ItemCssClass = r.ContentType == ContentType.File ? "document" : "link",
                    r.Url,
                    ShortUrl = StripProtocol(r.Url),
                    UrlTarget = r.ContentType == ContentType.File ? "_blank" : "_self"
                }));
        }

        #endregion

        #region Private Methods

        private static string StripProtocol(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            return url.Replace("https://", string.Empty).Replace("http://", string.Empty);
        }

        #endregion
    }
}
