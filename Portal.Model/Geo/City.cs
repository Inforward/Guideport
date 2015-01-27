namespace Portal.Model.Geo
{
    public partial class City
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public int? StateProvinceID { get; set; }
        public int? CountryID { get; set; }
        public int? TimeZoneID { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public virtual Country Country { get; set; }
        public virtual StateProvince StateProvince { get; set; }
        public virtual TimeZone TimeZone { get; set; }
    }
}
