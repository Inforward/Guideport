using Portal.Model;
using Portal.Model.Planning;
using Portal.Model.Rules;
using System.Collections.Generic;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface IPlanningServiceChannel : IPlanningService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IPlanningService
    {
        [OperationContract]
        Wizard GetWizard(WizardRequest request);

        [OperationContract]
        Wizard GetProgress(ProgressRequest request);

        [OperationContract]
        Progress SaveProgress(Progress progress, int userId);

        [OperationContract]
        ActionItem GetActionItem(int actionItemId);
        [OperationContract]
        void UpdateActionItem(ref ActionItem actionItem);

        [OperationContract]
        EnrollmentStatus GetEnrollmentStatus(int userId);

        [OperationContract]
        PlanningSummary GetPlanningSummary(int planningWizardId, int userId);
    }
}
