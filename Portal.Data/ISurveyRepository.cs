using System.Collections.Generic;
using Portal.Model;

namespace Portal.Data
{
    public interface ISurveyRepository : IEntityRepository
    {
        Survey GetSurveyByID(int surveyId);
        Survey GetSurveyByName(string surveyName);
        void UpdateSurvey(Survey survey, int auditUserId);

        SurveyResponse GetSurveyResponse(string surveyName, int userId);
        void CreateSurveyResponse(SurveyResponse surveyResponse);
        void UpdateSurveyResponse(SurveyResponse surveyResponse);
        void UpdateSurveyResponseHistory(List<SurveyResponseHistory> surveyResponseHistories);
    }
}
