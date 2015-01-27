using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;

namespace Portal.Services.Clients
{
    public class EmailServiceClient : IEmailService
    {
        private readonly ServiceClient<IEmailServiceChannel> _emailService = new ServiceClient<IEmailServiceChannel>();

        public void Send(Email email)
        {
            var proxy = _emailService.CreateProxy();
            proxy.Send(email);
        }
    }
}