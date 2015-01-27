using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public class License
    {
        public int LicenseID { get; set; }
        public int LicenseTypeID { get; set; }
        public string LicenseTypeName { get; set; }
        public int LicenseExamTypeID { get; set; }
        public string LicenseExamTypeName { get; set; }
        public string RegistrationCategory { get; set; }
        public string Description { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<User> Users { get; set; }
    }

    public enum LicenseExamTypes
    {
        FINRARegistrations = 1,
        StateNFAExams = 3,
        ProfessionalDesignations = 4
    }
}
