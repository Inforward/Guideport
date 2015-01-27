using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public class Branch
    {
        public int BranchID { get; set; }
        public int AffiliateID { get; set; }
        public string BranchNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string MailingAddress1 { get; set; }
        public string MailingAddress2 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingCountry { get; set; }
        public string MailingZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        [NotMapped]
        public string Address
        {
            get
            {
                var address = new Address()
                {
                    AddressLine1 = Address1,
                    AddressLine2 = Address2,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    Country = Country
                };

                return address.ToString();
            }
        }

        [NotMapped]
        public string MailingAddress
        {
            get
            {
                var address = new Address()
                {
                    AddressLine1 = MailingAddress1,
                    AddressLine2 = MailingAddress2,
                    City = MailingCity,
                    State = MailingState,
                    ZipCode = MailingZipCode,
                    Country = MailingCountry
                };

                return address.ToString();
            }
        }

        [IgnoreDataMember]
        public virtual ICollection<User> Users { get; set; }
    }
}
