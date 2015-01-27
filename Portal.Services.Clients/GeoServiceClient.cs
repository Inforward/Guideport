using System.Collections.Generic;
using Portal.Model;
using Portal.Model.Geo;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;

namespace Portal.Services.Clients
{
    public class GeoServiceClient : IGeoService
    {
        private readonly ServiceClient<IGeoServiceChannel> _geoService = new ServiceClient<IGeoServiceChannel>();

        public IEnumerable<Country> GetCountries()
        {
            var proxy = _geoService.CreateProxy();
            return proxy.GetCountries();
        }

        public IEnumerable<StateProvince> GetStateProvinces(StateProvinceRequest request)
        {
            var proxy = _geoService.CreateProxy();
            return proxy.GetStateProvinces(request);
        }

        public IEnumerable<City> GetCities(CityRequest request)
        {
            var proxy = _geoService.CreateProxy();
            return proxy.GetCities(request);
        }
    }
}