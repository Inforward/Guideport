using System;

namespace Portal.Model
{
    public class SurveyResponseHistoryRequest
    {
        public string SurveyName { get; set; }
        public DateTime? ResponseStartDate { get; set; }
        public DateTime? ResponseEndDate { get; set; }
    }
}
