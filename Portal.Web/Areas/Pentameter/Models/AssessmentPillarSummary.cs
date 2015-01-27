using System.Collections.Generic;
using Portal.Model;

namespace Portal.Web.Areas.Pentameter.Models
{
    public class AssessmentPillarSummary
    {
        public string PillarName { get; set; }
        public int CurrentScore { get; set; }
        public bool Enabled { get; set; }
        public string Summary { get; set; }
        public Dictionary<string, string> SuggestedContents { get; set; }
    }
}