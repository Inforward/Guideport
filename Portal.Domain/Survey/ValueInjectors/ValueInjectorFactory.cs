using Portal.Domain.Survey.ValueInjectors.Implementations;

namespace Portal.Domain.Survey.ValueInjectors
{
    public class ValueInjectorFactory
    {
        internal static IValueInjector CreateValueInjector(string surveyName)
        {
            IValueInjector injector = null;

            switch (surveyName)
            {
                case "Enrollment":
                    injector = new EnrollmentValueInjector();
                    break;
                case "Business Assessment":
                    injector = new BusinessAssessmentValueInjector();
                    break;
            }

            return injector;
        }
    }
}
