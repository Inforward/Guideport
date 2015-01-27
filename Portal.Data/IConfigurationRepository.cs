
using Portal.Model.App;

namespace Portal.Data
{
    public interface IConfigurationRepository : IEntityRepository
    {
        void UpdateConfiguration(Configuration configuration);
    }
}
