using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Model
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            var address = string.Empty;

            if (!string.IsNullOrWhiteSpace(AddressLine1))
                address += AddressLine1;

            if (!string.IsNullOrWhiteSpace(AddressLine2))
            {
                if (address.Length > 0)
                    address += Environment.NewLine;

                address += AddressLine2;
            }

            var cityStateZip = string.Empty;

            if (!string.IsNullOrEmpty(City))
                cityStateZip += City;

            if (!string.IsNullOrEmpty(State))
            {
                if (cityStateZip.Length > 0)
                    cityStateZip += ", ";

                cityStateZip += State;
            }

            if (!string.IsNullOrEmpty(ZipCode))
            {
                if (cityStateZip.Length > 0)
                    cityStateZip += " ";

                cityStateZip += ZipCode;
            }

            if (address.Length > 0 && cityStateZip.Length > 0)
            {
                address += Environment.NewLine;
                address += cityStateZip;
            }

            if (!string.IsNullOrEmpty(Country) && Country != "US")
            {
                if (address.Length > 0)
                    address += Environment.NewLine;

                address += Country;
            }

            return address;
        }
    }
}
