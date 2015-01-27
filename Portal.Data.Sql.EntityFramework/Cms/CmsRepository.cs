using System.Data.Entity;
using System.Linq;
using Portal.Model;
using RefactorThis.GraphDiff;

namespace Portal.Data.Sql.EntityFramework
{
    public class CmsRepository : EntityRepository<MasterContext>, ICmsRepository
    {
        public SiteContent SaveSiteContent(SiteContent siteContent)
        {
            var updatedSiteContent = Context.UpdateGraph(siteContent, 
                                        map => map.AssociatedCollection(s => s.ProfileTypes)
                                                  .AssociatedCollection(a => a.Affiliates)
                                                  .OwnedCollection(v => v.Versions, z => z.AssociatedCollection(a => a.Affiliates))
                                                  .OwnedCollection(k => k.KnowledgeLibraries));
            Save();

            if (siteContent.SiteContentID <= 0)
                siteContent.SiteContentID = updatedSiteContent.SiteContentID;

            return siteContent;
        }

        public ThirdPartyResource SaveThirdPartyResource(ThirdPartyResource resource)
        {
            var updatedResource = Context.UpdateGraph(resource,
                                        map => map.AssociatedCollection(a => a.Affiliates));
            Save();

            return updatedResource;
        }
    }
}
