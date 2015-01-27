using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Portal.Infrastructure.Helpers;
using Portal.Model.Geo;

namespace Portal.Data.Sql.EntityFramework
{
    public class GeoRepository : EntityRepository<MasterContext>, IGeoRepository
    {
        public IEnumerable<City> GetCityStateByName(string name, string countryCode = "US")
        {
            return FindBy<City>(c => string.Concat(c.Name, ", ", c.StateProvince.StateCode).StartsWith(name))
                        .Where(c => c.Country.CountryCode == countryCode)
                        .Include(c => c.StateProvince)
                        .DistinctBy(c => new { c.Name, c.StateProvince.StateCode })
                        .OrderBy(c => c.Name)
                        .ToList();
        }
    }
}
