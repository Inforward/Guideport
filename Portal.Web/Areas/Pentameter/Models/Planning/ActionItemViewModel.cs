namespace Portal.Web.Areas.Pentameter.Models.Planning
{
    public class ActionItemViewModel
    {
        public int ActionItemId { get; set; }
        public int StepId { get; set; }
        public string Text { get; set; }
        public string ListNumber { get; set; }
        public string ResourceHtml { get; set; }
        public bool Complete { get; set; }
    }
}