using System;
using Portal.Data;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Domain.Services
{
    public class FirstAlliedEmailService : IEmailService
    {
        private readonly IServicesRepository _servicesRepository;

        public FirstAlliedEmailService(IServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public void Send(Email email)
        {
            Required.NotNull(email, "Email");
            Required.NotNullOrEmpty(email.To, string.Empty, "To");
            Required.NotNullOrEmpty(email.From, string.Empty, "From");
            Required.NotNullOrEmpty(email.Subject, string.Empty, "Subject");
            Required.NotNullOrEmpty(email.Body, string.Empty, "Body");

            email.CreateDate = DateTime.Now;
            email.Attempts = 0;

            _servicesRepository.Add(email);
            _servicesRepository.Save();
        }
    }
}
