using Portal.Model;
using Portal.Model.Report;

namespace Portal.Data
{
    public interface IReportRepository : IEntityRepository
    {
        ExecuteResult Execute(View view, Pager pager = null);
    }
}
