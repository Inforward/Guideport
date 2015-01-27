namespace Portal.Model.Geo
{
    public class CityRequest
    {
        public CityRequest()
        {
            CountryCode = "US";
        }

        public string Name { get; set; }
        public string CountryCode { get; set; }
    }
}
