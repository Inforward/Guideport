using Portal.Model;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface ILogServiceChannel : ILogService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface ILogService
    {
        [OperationContract(IsOneWay = true)]
        void LogEvent(EventLog eventLog);
    }
}
