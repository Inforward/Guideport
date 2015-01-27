using System.Collections.Generic;

namespace Portal.Model
{
    public class SurveySummary
    {
        public SurveySummary()
        {
            Pages = new List<SurveyPageSummary>();
        }

        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public decimal Score { get; set; }
        public int DisplayScore { get; set; }
        public SurveyState State { get; set; }
        public List<SurveyPageSummary> Pages { get; set; }
    }
}
