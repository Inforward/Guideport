using System;

namespace Portal.Model
{
    public class SurveyResponseRequest
    {
        public int UserID { get; set; }
        public string SurveyName { get; set; }
        public bool OnlyLatestScore { get; set; }
        public bool IncludeSurvey { get; set; }
        public bool IncludeAnswers { get; set; }
        public DateTime? ModifyStartDate { get; set; }
        public DateTime? ModifyEndDate { get; set; }
    }
}
