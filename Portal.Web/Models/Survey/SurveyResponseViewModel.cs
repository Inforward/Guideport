using System.Collections.Generic;

namespace Portal.Web.Models
{
    public class SurveyResponseViewModel
    {
        public int SurveyId { get; set; }
        public int SelectedPageId { get; set; }
        public string SurveyName { get; set; }
        public bool ApplyAsComplete { get; set; }
        public List<JsonSurveyResponseAnswerViewModel> Answers { get; set; }
    }

    public class JsonSurveyResponseAnswerViewModel
    {
        public int SurveyQuestionId { get; set; }
        public int SurveyPageId { get; set; }
        public string Answer { get; set; }
    }
}