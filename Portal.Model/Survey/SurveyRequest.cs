
namespace Portal.Model
{
    public class SurveyRequest
    {
        public int UserID { get; set; }
        public int SurveyID { get; set; }
        public string SurveyName { get; set; }
        public bool IncludeResponse { get; set; }
        public bool ApplyResponse { get; set; }
        public bool CacheForUser { get; set; }

        public SurveyResponse ResponseToApply { get; set; }

        public static SurveyRequest DefaultRequest(string surveyName)
        {
            return new SurveyRequest()
            {
                SurveyName = surveyName,
                IncludeResponse = false,
                ApplyResponse = false
            };
        }
    }
}
