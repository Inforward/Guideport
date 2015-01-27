using System;
using Portal.Model;

namespace Portal.Web.Helpers
{
    public static class ExtensionHelpers
    {
        public static string StripLineBreaks(this string input)
        {
            return input.Replace("<br/>", string.Empty).Replace("<br />", string.Empty).Replace("<br>", string.Empty);
        }

        public static string ToSurveyStatusDisplayText(this SurveyState status)
        {
            var text = Enum.GetName(typeof(SurveyState), status);

            switch (status)
            {
                case SurveyState.InProgress:
                    text = "In Progress";
                    break;
                case SurveyState.NotStarted:
                    text = "Not Started";
                    break;
            }

            return text;
        }

        public static Sites FromSiteName(this string name)
        {
            switch (name)
            {
                case "Guideport":
                    return Sites.Guideport;
                case "Pentameter":
                    return Sites.Pentameter;
                case "Guided Retirement Solutions":
                    return Sites.GuidedRetirementSolutions;
            }

            return Sites.Unknown;
        }
    }
}