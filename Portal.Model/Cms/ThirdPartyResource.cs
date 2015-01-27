using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public class ThirdPartyResource : Auditable
    {
        public ThirdPartyResource()
        {
            Affiliates = new List<Affiliate>();
        }

        public int ThirdPartyResourceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }
        public string PhoneNoExt { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string Services { get; set; }
        public ICollection<Affiliate> Affiliates { get; set; }

        [NotMapped]
        public string CountryName { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public string Address
        {
            get
            {
                var address = new Address()
                {
                    AddressLine1 = AddressLine1,
                    AddressLine2 = AddressLine2,
                    City = City,
                    State = State,
                    ZipCode = PostalCode,
                    Country = Country
                };

                return address.ToString();
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public string AddressHtml
        {
            get
            {
                var address = Address;

                if (!string.IsNullOrEmpty(address))
                    address = address.Replace(Environment.NewLine, "<br />");

                return address;
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public string ServicesHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Services))
                    return Services;

                var services = Services.Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries).ToArray();

                Array.Sort(services);

                return string.Join("<br />", services);
            }
        }

        [NotMapped]
        public List<string> ServicesList
        {
            get
            {
                var list = new List<string>();

                if (!string.IsNullOrEmpty(Services))
                    list = Services.Split(new[] {','}).ToList();

                return list;
            }
        }
    }
}