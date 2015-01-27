using System.Collections;
using System.Collections.Generic;
using Portal.Model;
using System.ServiceModel;
using Portal.Model.Geo;

namespace Portal.Services.Contracts
{
    public interface IGeoServiceChannel : IGeoService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IGeoService
    {
        [OperationContract]
        IEnumerable<Country> GetCountries();

        [OperationContract]
        IEnumerable<StateProvince> GetStateProvinces(StateProvinceRequest request);

        [OperationContract]
        IEnumerable<City> GetCities(CityRequest request);
    }
}
