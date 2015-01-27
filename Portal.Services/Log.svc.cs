using Portal.Data;
using Portal.Domain.Services;

namespace Portal.Services
{
    public class Log : LogService
    {

        public Log(ILogRepository logRepository) 
            : base(logRepository)
        { }
    }
}