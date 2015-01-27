
using Portal.Model.App;
using RefactorThis.GraphDiff;

namespace Portal.Data.Sql.EntityFramework
{
    public class ConfigurationRepository : EntityRepository<MasterContext>, IConfigurationRepository
    {
        public void UpdateConfiguration(Configuration configuration)
        {
            Context.UpdateGraph(configuration, map => map.OwnedCollection(a => a.ConfigurationSettings));

            Save();            
        }
    }
}
