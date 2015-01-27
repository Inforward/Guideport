using System.Collections.Generic;
using Portal.Model;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface ISurveyServiceChannel : ISurveyService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface ISurveyService
    {
        [OperationContract]
        Survey GetSurvey(SurveyRequest request);

        [OperationContract]
        IEnumerable<Survey> GetSurveys();

        [OperationContract]
        void UpdateSurvey(Survey survey, int auditUserId);

        [OperationContract]
        SurveyResponse GetSurveyResponse(SurveyResponseRequest request);

        [OperationContract]
        void CreateSurveyResponse(ref SurveyResponse surveyResponse);

        [OperationContract]
        SaveSurveyResponseResponse SaveSurveyResponse(ref SurveyResponse surveyResponse);

        [OperationContract]
        SurveySummary GetSurveySummary(string surveyName, int userId);
    }
}
