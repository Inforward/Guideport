using System.Collections.Generic;

namespace Portal.Model.Geo
{
    public partial class Country
    {
        public Country()
        {
            Cities = new List<City>();
            StateProvinces = new List<StateProvince>();
        }

        public int CountryID { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string PostalCodeRegEx { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<StateProvince> StateProvinces { get; set; }
    }
}
