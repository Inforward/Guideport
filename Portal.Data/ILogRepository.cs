using Portal.Model;

namespace Portal.Data
{
    public interface ILogRepository : IEntityRepository
    {
        void Log(EventLog eventLog);
    }
}
