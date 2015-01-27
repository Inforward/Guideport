using Portal.Model;
using Portal.Model.Planning;

namespace Portal.Web.Areas.Pentameter.Models.Succession
{
    public class DashboardViewModel
    {
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public Status QualifiedBuyerStatus { get; set; }
        public Status ContinuityPlanningStatus { get; set; }
        public Status SuccessionPlanningStatus { get; set; }
        public Status BusinessAcquisitionStatus { get; set; }
    }

    public class Status
    {
        public string Title { get; set; }
        public bool Unlocked { get; set; }
        public string Message { get; set; }
        public int PercentComplete { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }

        public SurveyState State
        {
            get
            {
                if(PercentComplete <= 0)
                    return SurveyState.NotStarted;

                return PercentComplete >= 100 ? SurveyState.Complete : SurveyState.InProgress;
            }
        }
    }
}