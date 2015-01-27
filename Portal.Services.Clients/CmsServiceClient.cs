using System.Collections.Generic;
using System.ServiceModel;
using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;

namespace Portal.Services.Clients
{
    public class CmsServiceClient : ICmsService
    {
        private readonly ServiceClient<ICmsServiceChannel> _cmsService = new ServiceClient<ICmsServiceChannel>();

        public Site GetSite(SiteRequest request)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetSite(request);
        }

        public IEnumerable<Site> GetSites()
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetSites();
        }

        public IEnumerable<Site> GetSites(SiteRequest request)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetSites(request);
        }

        public SiteMap GetSiteMap(int siteId, int profileTypeId, int affiliateId)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetSiteMap(siteId, profileTypeId, affiliateId);
        }

        public bool IsContentUrl(string permalink)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.IsContentUrl(permalink);
        }

        public SiteContentViewModel GetSiteContentViewModel(string permalink, int profileTypeId, int affiliateId)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetSiteContentViewModel(permalink, profileTypeId, affiliateId);
        }

        public IEnumerable<SiteContent> GetSiteContents(SiteContentRequest request)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetSiteContents(request);
        }

        public SiteContent GetSiteContent(SiteContentRequest request)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetSiteContent(request);
        }

        public IEnumerable<SiteContentViewModel> SearchSiteContents(SiteContentRequest request)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.SearchSiteContents(request);
        }

        public void SaveSiteContent(ref SiteContent content, int auditUserId)
        {
            var proxy = _cmsService.CreateProxy();
            proxy.SaveSiteContent(ref content, auditUserId);
        }

        public PermalinkResponse GeneratePermalink(PermalinkRequest request)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GeneratePermalink(request);            
        }

        public IEnumerable<SiteContent> GetParentPages(int siteId, int excludeId = 0)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetParentPages(siteId, excludeId);
        }

        public IEnumerable<MenuIcon> GetMenuIcons()
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetMenuIcons();
        }

        public IEnumerable<KnowledgeLibrary> GetKnowledgeLibrary(int siteId)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetKnowledgeLibrary(siteId);
        }

        public IEnumerable<KnowledgeLibraryTopic> GetKnowledgeLibraryTopics(int siteId)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetKnowledgeLibraryTopics(siteId);
        }

        public IEnumerable<ThirdPartyResource> GetThirdPartyResources()
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetThirdPartyResources();
        }

        public IEnumerable<ThirdPartyResourceService> GetThirdPartyResourceServices()
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetThirdPartyResourceServices();
        }

        public ThirdPartyResource GetThirdPartyResource(int thirdPartyResourceId)
        {
            var proxy = _cmsService.CreateProxy();
            return proxy.GetThirdPartyResource(thirdPartyResourceId);
        }

        public void SaveThirdPartyResource(ref ThirdPartyResource resource)
        {
            var proxy = _cmsService.CreateProxy();
            proxy.SaveThirdPartyResource(ref resource);
        }

        public void DeleteThirdPartyResource(int thirdPartyResourceId)
        {
            var proxy = _cmsService.CreateProxy();
            proxy.DeleteThirdPartyResource(thirdPartyResourceId);
        }
    }
}