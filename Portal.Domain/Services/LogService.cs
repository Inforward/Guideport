using Portal.Data;
using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Domain.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void LogEvent(EventLog eventLog)
        {
            _logRepository.Log(eventLog);
        }
    }
}
