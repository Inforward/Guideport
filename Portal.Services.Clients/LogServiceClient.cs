using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;

namespace Portal.Services.Clients
{
    public class LogServiceClient : ILogService
    {
        private readonly ServiceClient<ILogServiceChannel> _logService = new ServiceClient<ILogServiceChannel>();

        public void LogEvent(EventLog eventLog)
        {
            var proxy = _logService.CreateProxy();
            proxy.LogEvent(eventLog);
        }
    }
}