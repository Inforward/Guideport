using System.Collections.Generic;
using Portal.Model;

namespace Portal.Model
{
    public class SaveSurveyResponseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public List<SurveyQuestionError> Result { get; set; }

        public SaveSurveyResponseResponse()
        {
            Result = new List<SurveyQuestionError>();
        }
    }
}
