using System.Collections.Generic;

namespace Portal.Web.Areas.Pentameter.Models
{
    public class DashboardViewModel
    {
        public List<PillarProgressModel> Pillars { get; set; }
        public List<BusinessStatistic> BusinessStatistics { get; set; }
        public string AssessmentText { get; set; }
        public string AssessmentShortText { get; set; }
        public string AssessmentUrl { get; set; }
        public bool IsBannerExpanded { get; set; }
        public bool IsQlikViewDashboardEnabled { get; set; }
        public string QlikViewDocument { get; set; }

        public DashboardViewModel()
        {
            Pillars = new List<PillarProgressModel>();
            BusinessStatistics = new List<BusinessStatistic>();
        }
    }

    public class PillarProgressModel
    {
        public string Title { get; set; }
        public string Tooltip { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }
        public int Score { get; set; }
    }

    public class BusinessStatistic
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}