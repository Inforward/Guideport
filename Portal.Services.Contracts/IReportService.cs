using System.Collections.Generic;
using Portal.Model;
using System.ServiceModel;
using Portal.Model.Report;

namespace Portal.Services.Contracts
{
    public interface IReportServiceChannel : IReportService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IReportService
    {
        [OperationContract]
        IEnumerable<Category> GetViewCategories();

        [OperationContract]
        IEnumerable<View> GetViews();

        [OperationContract]
        ReportResponse GetViewResult(ReportRequest request);
    }
}
