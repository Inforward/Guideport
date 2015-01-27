using System.Collections.Generic;
using System.Web.Http;
using Portal.Infrastructure.Logging;
using Portal.Model.Geo;
using Portal.Services.Contracts;

namespace Portal.Web.Admin.Controllers
{
    [RoutePrefix("api/geo")]
    public class GeoController : BaseApiController
    {
        private readonly IGeoService _geoService;

        public GeoController(IUserService userService, ILogger logger, IGeoService geoService) 
            : base(userService, logger)
        {
            _geoService = geoService;
        }

        [HttpGet]
        [Route("countries")]
        [AllowAnonymous]
        public IEnumerable<Country> GetCountries()
        {
            return _geoService.GetCountries();
        }

        [HttpGet]
        [Route("countries/{countryCode}/state-provinces")]
        [AllowAnonymous]
        public IEnumerable<StateProvince> GetStateProvincesByCountryCode([FromUri]string countryCode)
        {
            return _geoService.GetStateProvinces(new StateProvinceRequest{ CountryCode = countryCode });
        }
    }
}