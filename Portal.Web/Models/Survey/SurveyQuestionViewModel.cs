using System.Collections.Generic;

namespace Portal.Web.Models
{
    public class SurveyQuestionViewModel
    {
        public int Id { get; set; }        
        public bool IsRequired { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsVisible { get; set; }
        public bool HasTrigger { get; set; }
        public string Number { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public string InputType { get; set; }
        public string AnswerType { get; set; }
        public string Layout { get; set; }
        public List<JsonMultiTextAnswer> Answers { get; set; }
        public List<SurveyAnswerViewModel> PossibleAnswers { get; set; }
        
        public SurveyQuestionViewModel()
        {
            Answers = new List<JsonMultiTextAnswer>();
        }
    }
    
    public class JsonMultiTextAnswer
    {
        public string Text { get; set; }
        public string Value { get; set; }    
    }
}