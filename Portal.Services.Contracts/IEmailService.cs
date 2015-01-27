using Portal.Model;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface IEmailServiceChannel : IEmailService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IEmailService
    {
        [OperationContract(IsOneWay = true)]
        void Send(Email email);
    }
}
