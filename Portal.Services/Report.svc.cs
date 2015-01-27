using Portal.Data;
using Portal.Domain.Services;

namespace Portal.Services
{
    public class Report : ReportService
    {
        public Report(IReportRepository reportRepository)
            : base(reportRepository)
        { }
    }
}
