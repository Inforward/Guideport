using Portal.Model;

namespace Portal.Data
{
    public interface ICmsRepository : IEntityRepository
    {
        SiteContent SaveSiteContent(SiteContent siteContent);
        ThirdPartyResource SaveThirdPartyResource(ThirdPartyResource resource);
    }
}
