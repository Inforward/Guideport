using Portal.Services.Contracts;

namespace Portal.Domain.Survey.ValueInjectors
{
    internal interface IValueInjector
    {
        void Inject(Model.Survey survey, int userId, IUserService userService, ISurveyService surveyService);
        void PreSelect(Model.Survey survey, int userId, IUserService userService, ISurveyService surveyService);
    }
}
