using System.IO;
using System.Linq;
using System.Text;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Domain.Survey.Notifications
{
    public class BusinessConsultantEmail : INotification
    {
        private readonly IEmailService _emailService;

        public User CurrentUser { get; set; }
        public Model.Survey PreviousSurvey { get; set; }
        public Model.Survey CurrentSurvey { get; set; }
        public string SummaryUrl { get; set; }

        public BusinessConsultantEmail(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void Send()
        {
            // Do not send unless the survey is actually complete
            if (!CurrentSurvey.CurrentResponse.CompleteDate.HasValue)
                return;

            // Check for e-mail override (for dev / qa )
            var to = Settings.Get("app:Email.Recipients.To.Override", string.Empty).Trim();

            if (to.Length == 0)
                to = CurrentUser.BusinessConsultantEmail;

            if (!string.IsNullOrWhiteSpace(to))
            {
                var hasChanges = false;
                var body = GenerateChangeSummaryHtml(out hasChanges);

                if (!hasChanges)
                    return;

                var email = new Email
                {
                    To = to,
                    From = Settings.Get("app:Email.System", string.Empty),
                    Cc = Settings.Get("app:Email.Recipients.Cc", string.Empty),
                    Bcc = Settings.Get("app:Email.Recipients.Bcc", string.Empty),
                    Subject = string.Format("{0} completed by {1}", CurrentSurvey.SurveyName, CurrentUser.DisplayName),
                    Body = body
                };

                _emailService.Send(email);
            }
        }

        private string GenerateChangeSummaryHtml(out bool hasChanges)
        {
            hasChanges = false;

            using (var writer = new StringWriter())
            {
                writer.WriteLine("<html>");
                writer.WriteLine(" <head>");
                writer.WriteLine("  <style>");
                writer.WriteLine("    body, table, p, a, h3 { font-family: Arial, Verdana; font-size: 12px; }");
                writer.WriteLine("    table td { padding: 3px; }");
                writer.WriteLine("    .page { background-color: #666; color: #fff; font-size: 16px; }");
                writer.WriteLine("    .question { background-color: #ededed; font-size: 14px; }");
                writer.WriteLine("  </style>");
                writer.WriteLine(" </head>");

                writer.WriteLine("<body>");

                writer.WriteLine("<table>");
                writer.WriteLine(" <tr><td>Advisor:</td><td>{0}</td></tr>", CurrentUser.DisplayName);
                writer.WriteLine(" <tr><td>Completed:</td><td>{0}</td></tr>", CurrentSurvey.CurrentResponse.CompleteDate.Value.ToString("MMM dd, yyyy @ hh:mm tt"));
                writer.WriteLine(" <tr><td>Last Modified:</td><td>{0}</td></tr>", CurrentSurvey.ModifyDate.ToString("MMM dd, yyyy @ hh:mm tt"));
                writer.WriteLine("</table>");

                if (!string.IsNullOrEmpty(SummaryUrl))
                    writer.WriteLine("<p><a href='{0}'>Click here to view full survey</a></p>", SummaryUrl);

                writer.WriteLine("<h2>Change Summary</h2>");

                writer.WriteLine("<table>");

                if (PreviousSurvey != null)
                {
                    foreach (var page in PreviousSurvey.Pages.Where(p => p.IsVisible && !p.IsSummary))
                    {
                        var sb = new StringBuilder();

                        foreach (var previousQuestion in page.Questions.Where(q => q.IsVisible && q.InputType != InputType.None))
                        {
                            var currentQuestion = CurrentSurvey.GetQuestion(page.PageName, previousQuestion.QuestionName);

                            if (currentQuestion != null)
                            {
                                if (currentQuestion.Answers.Except(previousQuestion.Answers).Any() || previousQuestion.Answers.Except(currentQuestion.Answers).Any())
                                {
                                    sb.AppendFormat("<tr><td colspan='2' class='question'>{0}</td></tr>", currentQuestion.QuestionText);
                                    sb.AppendFormat("<tr><td>Previous Answer(s):</td><td>{0}</td></tr>", previousQuestion.Answers.ToCsv());
                                    sb.AppendFormat("<tr><td>Current Answer(s):</td><td>{0}</td></tr>", currentQuestion.Answers.ToCsv());
                                }
                            }
                        }

                        if (sb.Length > 0)
                        {
                            writer.WriteLine("<tr><td colspan='2' class='page'>{0}</td></tr>", page.PageName);
                            writer.WriteLine(sb.ToString());
                            hasChanges = true;
                        }
                    }
                }
                else
                {
                    writer.WriteLine("<tr><td>Initial completion, no changes to report.</td></tr>");
                }

                writer.WriteLine("  </table>");
                writer.WriteLine(" </body>");
                writer.WriteLine("</html>");

                return writer.ToString();
            }
        }
    }
}
