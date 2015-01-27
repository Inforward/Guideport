using System.Collections;
using System.Collections.Generic;
using Portal.Model;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface ICmsServiceChannel : ICmsService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface ICmsService
    {
        [OperationContract]
        Site GetSite(SiteRequest request);

        [OperationContract(Name="GetAllSites")]
        IEnumerable<Site> GetSites();

        [OperationContract]
        IEnumerable<Site> GetSites(SiteRequest request);

        [OperationContract]
        SiteMap GetSiteMap(int siteId, int profileTypeId, int affiliateId);

        [OperationContract]
        bool IsContentUrl(string permalink);

        [OperationContract]
        SiteContentViewModel GetSiteContentViewModel(string permalink, int profileTypeId, int affiliateId);

        [OperationContract]
        IEnumerable<SiteContent> GetSiteContents(SiteContentRequest request);

        [OperationContract]
        SiteContent GetSiteContent(SiteContentRequest request);

        [OperationContract]
        IEnumerable<SiteContentViewModel> SearchSiteContents(SiteContentRequest request);

        [OperationContract]
        IEnumerable<SiteContent> GetParentPages(int siteId, int excludeId = 0);

        [OperationContract]
        void SaveSiteContent(ref SiteContent content, int auditUserId);

        [OperationContract]
        PermalinkResponse GeneratePermalink(PermalinkRequest request);

        [OperationContract]
        IEnumerable<MenuIcon> GetMenuIcons();

        [OperationContract]
        IEnumerable<KnowledgeLibrary> GetKnowledgeLibrary(int siteId);

        [OperationContract]
        IEnumerable<KnowledgeLibraryTopic> GetKnowledgeLibraryTopics(int siteId);

        [OperationContract]
        IEnumerable<ThirdPartyResource> GetThirdPartyResources();

        [OperationContract]
        IEnumerable<ThirdPartyResourceService> GetThirdPartyResourceServices();

        [OperationContract]
        ThirdPartyResource GetThirdPartyResource(int thirdPartyResourceId);

        [OperationContract]
        void SaveThirdPartyResource(ref ThirdPartyResource resource);

        [OperationContract]
        void DeleteThirdPartyResource(int thirdPartyResourceId);
    }
}
