using Portal.Data;
using Portal.Model.Geo;
using Portal.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Domain.Services
{
    public class GeoService : IGeoService
    {
        private readonly IGeoRepository _geoRepository;

        public GeoService(IGeoRepository geoRepository)
        {
            _geoRepository = geoRepository;
        }

        public IEnumerable<Country> GetCountries()
        {
            return _geoRepository.GetAll<Country>().OrderBy(c => c.Name).ToList();
        }

        public IEnumerable<StateProvince> GetStateProvinces(StateProvinceRequest request)
        {
            if (request == null) return null;

            var query = _geoRepository.GetAll<StateProvince>();

            if (!string.IsNullOrEmpty(request.CountryCode))
                query = query.Where(s => s.Country.CountryCode == request.CountryCode);

            if (request.CountryID > 0)
                query = query.Where(s => s.CountryID == request.CountryID);

            return query.OrderBy(s => s.Name).ToList();
        }

        public IEnumerable<City> GetCities(CityRequest request)
        {
            if (request == null) return null;

            return _geoRepository.GetCityStateByName(request.Name, request.CountryCode);
        }
    }
}
