using Portal.Infrastructure.Logging;
using Portal.Model.Geo;
using Portal.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class GeoController : BaseController
    {
        private readonly IGeoService _geoService;

        public GeoController(IUserService userService, IGeoService geoService, ILogger logger)
            : base(userService, logger)
        {
            _geoService = geoService;
        }

        [HttpPost]
        public JsonResult JsonGetCountries()
        {
            return JsonResponse(() => _geoService.GetCountries());
        }

        [HttpPost]
        public JsonResult JsonGetStateProvinciesByCountryId(int countryId)
        {
            return JsonResponse(() => _geoService.GetStateProvinces(new StateProvinceRequest() { CountryID = countryId }));
        }

        [HttpPost]
        public JsonResult JsonGetStateProvinciesByCountryAbbreviation(string countryAbbreviation)
        {
            return JsonResponse(() => _geoService.GetStateProvinces(new StateProvinceRequest() { CountryCode = countryAbbreviation }));
        }

        [HttpPost]
        public JsonResult JsonGetLocations(string searchPattern)
        {
            var list = new List<City>();

            if (!string.IsNullOrWhiteSpace(searchPattern))
            {
                list = _geoService.GetCities(new CityRequest() { Name = searchPattern }).ToList();
            }

            return Json(list.Select(l => new
                            {
                                Text = string.Format("{0}, {1}", l.Name, l.StateProvince.Name),
                                Value = string.Format("{0}, {1}", l.Name, l.StateProvince.Name)
                            }));
        }
    }
}
