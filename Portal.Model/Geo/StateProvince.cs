using System.Collections.Generic;

namespace Portal.Model.Geo
{
    public partial class StateProvince
    {
        public StateProvince()
        {
            Cities = new List<City>();
        }

        public int StateProvinceID { get; set; }
        public int CountryID { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual Country Country { get; set; }
    }
}
