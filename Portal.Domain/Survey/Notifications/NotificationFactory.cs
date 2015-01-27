using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Domain.Survey.Notifications
{
    public class NotificationFactory
    {
        private readonly IEmailService _emailService;

        public NotificationFactory(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public INotification CreateNotification(SurveyNotificationType type, Model.Survey previousSurvey, Model.Survey currentSurvey, User currentUser)
        {
            INotification notification = null;

            if (type == SurveyNotificationType.BusinessConsultantEmail)
            {
                notification = new BusinessConsultantEmail(_emailService)
                {
                    CurrentUser = currentUser,
                    PreviousSurvey = previousSurvey,
                    CurrentSurvey = currentSurvey
                };
            }

            return notification;
        }
    }
}
