
using System.Collections.Generic;
using Portal.Model.Geo;

namespace Portal.Data
{
    public interface IGeoRepository : IEntityRepository
    {
        IEnumerable<City> GetCityStateByName(string name, string countryCode = "US");
    }
}
