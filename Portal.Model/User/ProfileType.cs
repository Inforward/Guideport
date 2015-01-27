using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Portal.Model
{
    public class ProfileType
    {
        public int ProfileTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<User> Users { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContent> SiteContents { get; set; }
    }

    public enum ProfileTypes
    {
        Other = 0,
        Employee = 1,
        FinancialAdvisor = 2,
        BranchAssistant = 3
    }

    public class ProfileTypeValues
    {
        public const string Employee = "Employee";
        public const string FinancialAdvisor = "Financial Advisor";
        public const string BranchAssistant = "Branch Assistant";
        public const string Other = "Other";
    }
}