using Portal.Data;
using Portal.Domain.Services;

namespace Portal.Services
{
    public class Geo : GeoService
    {
        public Geo(IGeoRepository geoRepository)
            : base(geoRepository)
        { }
    }
}
