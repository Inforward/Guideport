using System.Collections.Generic;
using Portal.Model.Report;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;

namespace Portal.Services.Clients
{
    public class ReportServiceClient : IReportService
    {
        private readonly ServiceClient<IReportServiceChannel> _reportService = new ServiceClient<IReportServiceChannel>();

        public IEnumerable<Category> GetViewCategories()
        {
            var proxy = _reportService.CreateProxy();
            return proxy.GetViewCategories();
        }

        public IEnumerable<View> GetViews()
        {
            var proxy = _reportService.CreateProxy();
            return proxy.GetViews();
        }

        public ReportResponse GetViewResult(ReportRequest request)
        {
            var proxy = _reportService.CreateProxy();
            return proxy.GetViewResult(request);            
        }
    }
}