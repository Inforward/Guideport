using System.Collections.Generic;
using Portal.Model;

namespace Portal.Web.Models.AdvisorView
{
    public class AdvisorViewModel
    {
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Affiliate> Affiliates { get; set; }
    }
}