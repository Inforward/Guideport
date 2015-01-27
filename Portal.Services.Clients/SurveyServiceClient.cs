using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;
using System.Collections.Generic;

namespace Portal.Services.Clients
{
    public class SurveyServiceClient : ISurveyService
    {
        private readonly ServiceClient<ISurveyServiceChannel> _surveyService = new ServiceClient<ISurveyServiceChannel>();

        public Survey GetSurvey(SurveyRequest request)
        {
            var proxy = _surveyService.CreateProxy();
            return proxy.GetSurvey(request);
        }

        public IEnumerable<Survey> GetSurveys()
        {
            var proxy = _surveyService.CreateProxy();
            return proxy.GetSurveys();
        }

        public void UpdateSurvey(Survey survey, int auditUserId)
        {
            var proxy = _surveyService.CreateProxy();
            proxy.UpdateSurvey(survey, auditUserId);
        }

        public SurveyResponse GetSurveyResponse(SurveyResponseRequest request)
        {
            var proxy = _surveyService.CreateProxy();
            return proxy.GetSurveyResponse(request);
        }

        public void CreateSurveyResponse(ref SurveyResponse surveyResponse)
        {
            var proxy = _surveyService.CreateProxy();
            proxy.CreateSurveyResponse(ref surveyResponse);
        }

        public SaveSurveyResponseResponse SaveSurveyResponse(ref SurveyResponse surveyResponse)
        {
            var proxy = _surveyService.CreateProxy();
            return proxy.SaveSurveyResponse(ref surveyResponse);
        }

        public SurveySummary GetSurveySummary(string surveyName, int userId)
        {
            var proxy = _surveyService.CreateProxy();
            return proxy.GetSurveySummary(surveyName, userId);
        }
    }
}