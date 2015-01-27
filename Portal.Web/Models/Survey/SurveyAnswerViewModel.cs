namespace Portal.Web.Models
{
    public class SurveyAnswerViewModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public bool IsSelected { get; set; }
        public bool IsTrigger { get; set; }
        public string Text { get; set; }
        public string ReviewText { get; set; }
    }
}