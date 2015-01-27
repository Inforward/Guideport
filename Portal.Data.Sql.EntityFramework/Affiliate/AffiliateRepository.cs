
using Portal.Model;
using RefactorThis.GraphDiff;

namespace Portal.Data.Sql.EntityFramework
{
    public class AffiliateRepository : EntityRepository<MasterContext>, IAffiliateRepository
    {
        public void SaveAffiliateFeature(AffiliateFeature feature)
        {
            Context.UpdateGraph(feature, map => map.OwnedCollection(f => f.Settings));
            Save();
        }
    }
}
