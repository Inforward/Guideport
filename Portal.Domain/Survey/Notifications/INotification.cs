using Portal.Model;

namespace Portal.Domain.Survey.Notifications
{
    public interface INotification
    {
        User CurrentUser { get; set; }
        Model.Survey PreviousSurvey { get; set; }
        Model.Survey CurrentSurvey { get; set; }
        string SummaryUrl { get; set; }

        void Send();
    }
}
