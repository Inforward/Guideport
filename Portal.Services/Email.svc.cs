using Portal.Domain.Services;
using Portal.Services.Contracts;

namespace Portal.Services
{
    public class Email : EmailService
    {
        public Email(ILogService logService)
            : base(logService)
        { }
    }
}
