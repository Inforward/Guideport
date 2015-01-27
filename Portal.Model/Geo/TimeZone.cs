using System.Collections.Generic;

namespace Portal.Model.Geo
{
    public partial class TimeZone
    {
        public TimeZone()
        {
            Cities = new List<City>();
        }

        public int TimeZoneID { get; set; }
        public string Name { get; set; }
        public double OffsetGMT { get; set; }
        public double OffsetDST { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
