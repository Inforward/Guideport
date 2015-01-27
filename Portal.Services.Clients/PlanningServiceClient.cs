using Portal.Model;
using Portal.Model.Planning;
using Portal.Model.Rules;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;
using System.Collections.Generic;

namespace Portal.Services.Clients
{
    public class PlanningServiceClient : IPlanningService
    {
        private readonly ServiceClient<IPlanningServiceChannel> _planningService = new ServiceClient<IPlanningServiceChannel>();

        public Wizard GetWizard(WizardRequest request)
        {
            var proxy = _planningService.CreateProxy();
            return proxy.GetWizard(request);
        }

        public Wizard GetProgress(ProgressRequest request)
        {
            var proxy = _planningService.CreateProxy();
            return proxy.GetProgress(request);
        }

        public Progress SaveProgress(Progress progress, int userId)
        {
            var proxy = _planningService.CreateProxy();
            return proxy.SaveProgress(progress, userId);
        }

        public ActionItem GetActionItem(int actionItemId)
        {
            var proxy = _planningService.CreateProxy();
            return proxy.GetActionItem(actionItemId);
        }

        public void UpdateActionItem(ref ActionItem actionItem)
        {
            var proxy = _planningService.CreateProxy();
            proxy.UpdateActionItem(ref actionItem);
        }

        public EnrollmentStatus GetEnrollmentStatus(int userId)
        {
            var proxy = _planningService.CreateProxy();
            return proxy.GetEnrollmentStatus(userId);            
        }

        public PlanningSummary GetPlanningSummary(int planningWizardId, int userId)
        {
            var proxy = _planningService.CreateProxy();
            return proxy.GetPlanningSummary(planningWizardId, userId);
        }
    }
}