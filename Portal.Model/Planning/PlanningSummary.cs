
namespace Portal.Model.Planning
{
    public class PlanningSummary
    {
        public int PlanningWizardID { get; set; }
        public string PlanningName { get; set; }
        public decimal PercentComplete { get; set; }
        public bool IsComplete
        {
            get { return PercentComplete >= 100; }
        }
        public PlanningType PlanningType
        {
            get { return (PlanningType)PlanningWizardID; }
        }
    }
}
