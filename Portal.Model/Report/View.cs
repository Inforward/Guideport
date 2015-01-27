using System.Collections.Generic;

namespace Portal.Model.Report
{
    public class View
    {
        public int ViewID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string StoredProcedureName { get; set; }
        public int? PageSize { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSortable { get; set; }
        public bool IsPageable { get; set; }
        public Category Category { get; set; }
        public ICollection<ViewColumn> ViewColumns { get; set; }
        public ICollection<Filter> Filters { get; set; }
    }

    public enum Views
    {
        BusinessAssessmentScores = 1,
        BusinessAssessmentProgression = 2,
        BusinessPlanProgression = 3,
        EnrollmentActivity = 4
    }
}