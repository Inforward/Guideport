using System.Collections.Generic;

namespace Portal.Model
{
    public class SurveyPageSummary
    {
        public SurveyPageSummary()
        {
            SuggestedContent = new List<SurveySummaryContent>();
        }

        public int SurveyPageID { get; set; }
        public string PageName { get; set; }
        public string ScoreSummary { get; set; }
        public string Tooltip { get; set; }
        public SurveyState State { get; set; }
        public decimal Score { get; set; }
        public int DisplayScore { get; set; }
        public List<SurveySummaryContent> SuggestedContent { get; set; }
    }

    public class SurveySummaryContent
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
