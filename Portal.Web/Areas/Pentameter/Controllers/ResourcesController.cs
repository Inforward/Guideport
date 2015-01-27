using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.ActionResults;
using Portal.Web.Controllers;
using Portal.Web.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal.Web.Areas.Pentameter.Controllers
{
    public class ResourcesController : BaseController
    {
        private readonly ICmsService _cmsService;

        public ResourcesController(IUserService userService, ILogger logger, ICmsService cmsService)
            : base(userService, logger)
        {
            _cmsService = cmsService;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            ViewBag.CanEdit = CurrentUser.IsInRole(PortalRoleValues.ContentAdmin);
        }

        [HttpGet]
        public ActionResult ThirdParty()
        {
            var thirdPartyServices = _cmsService.GetThirdPartyResourceServices().Select(s => s.ServiceName).ToList();

            return View("ThirdParty", thirdPartyServices);
        }

        [HttpGet]
        public ActionResult KnowledgeLibrary()
        {
            var viewModel = _cmsService.GetKnowledgeLibraryTopics(CurrentContent.SiteID);

            return View("KnowledgeLibrary", viewModel);
        }

        [HttpPost]
        public JsonNetResult JsonGetThirdPartyResources()
        {
            var resources = _cmsService.GetThirdPartyResources()
                                .Where(r => !r.Affiliates.Any() || r.Affiliates.Any(a => a.AffiliateID == AssistedUser.AffiliateID)).ToList();
                
            return new JsonNetResult(resources.Select(r => new 
                    {
                        r.ThirdPartyResourceID,
                        r.Name,
                        r.Description,
                        r.AddressLine1,
                        r.AddressLine2,
                        r.City,
                        r.State,
                        r.PostalCode,
                        r.Country,
                        r.AddressHtml,
                        r.PhoneNo,
                        r.PhoneNoExt,
                        r.FaxNo,
                        r.Email,
                        r.WebsiteUrl,
                        r.Services
                    }));
        }

        [HttpPost]
        public JsonNetResult JsonGetKnowledgeLibrary(int siteId)
        {
            var knowledgeLibrary = _cmsService.GetKnowledgeLibrary(siteId)
                                        .Where(k => !k.SiteContent.ProfileTypes.Any() || HasProfileType(k.SiteContent.ProfileTypes.Select(r => r.Name).ToArray()))
                                        .Where(k => !k.SiteContent.Affiliates.Any() || k.SiteContent.Affiliates.Any(a => a.AffiliateID == AssistedUser.AffiliateID))
                                        .ToList();

            return new JsonNetResult(knowledgeLibrary.Select(k => new
                {
                    k.SiteContent.Permalink, 
                    k.SiteContent.Title, 
                    k.Topic, 
                    k.Subtopic, 
                    k.CreatedBy
                }));
        }
    }
}
