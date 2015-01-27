using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Model.Planning;
using Portal.Services.Contracts;
using Portal.Web.Areas.Pentameter.Models.Planning;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Controllers;
using Portal.Web.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Web.Areas.Pentameter.Controllers
{
    public class PlanningController : BaseController
    {
        #region Private Members

        private readonly IPlanningService _planningService;

        #endregion

        #region Constructor

        public PlanningController(IUserService userService, ILogger logger, IPlanningService planningService)
            : base(userService, logger)
        {
            _planningService = planningService;
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(int wizardId)
        {
            var wizard = _planningService.GetWizard(new WizardRequest() { PlanningWizardID = wizardId });

            if (wizard == null)
                throw new Exception("Invalid Wizard ID: " + wizardId);

            var enrollmentStatus = _planningService.GetEnrollmentStatus(AssistedUser.UserID);                
            var enrollmentInterest = enrollmentStatus.EnrollmentInterests.First(i => i.Name == wizard.Name);

            if (!enrollmentInterest.Interested)
                throw new Exception("According to this user's enrollment profile, this planning tool is locked.");

            return View(wizard);
        }

        [HttpPost]
        public JsonResult JsonGetPlanningProgress(int wizardId)
        {
            return JsonResponse(() =>
            {
                var wizard = _planningService.GetProgress(new ProgressRequest{ UserID = AssistedUser.UserID, PlanningWizardID = wizardId });
                    
                return ToWizardViewModel(wizard);
            });
        }

        [HttpPost]
        public JsonResult JsonSavePlanningProgress(int wizardId, ProgressViewModel progress)
        {
            return JsonResponse(() =>
            {
                var currentProgress = FromProgressViewModel(progress);

                currentProgress.ModifyDate = DateTime.Now;
                currentProgress.ModifyDateUtc = DateTime.UtcNow;
                currentProgress.ModifyUserID = CurrentUser.UserID;

                _planningService.SaveProgress(currentProgress, AssistedUser.UserID);

                return new { LastModifiedDate = DateTime.UtcNow };
            });
        }

        [HttpPost]
        [PortalAuthorize(PortalRoleValues.ContentAdmin)]
        public JsonResult JsonUpdateActionItem(ActionItemViewModel viewModel)
        {
            return JsonResponse(() =>
            {
                var actionItem = FromActionItemViewModel(viewModel);

                var existing = _planningService.GetActionItem(actionItem.PlanningWizardActionItemID);

                if (existing == null)
                    throw new Exception("Invalid Action Item.  Cannot update");

                existing.ResourcesContent = actionItem.ResourcesContent;
                existing.ModifyUserID = CurrentUser.UserID;
                existing.ModifyDate = DateTime.Now;

                _planningService.UpdateActionItem(ref existing);
            });
        }

        #endregion

        #region Mapping Methods

        public static WizardViewModel ToWizardViewModel(Wizard wizard)
        {
            var viewModel = new WizardViewModel
            {
                WizardId = wizard.PlanningWizardID,
                ProgressId = wizard.Progress.PlanningWizardProgressID,
                PercentComplete = wizard.Progress.PercentComplete,
                LastModifiedDate = wizard.Progress.ModifyDateUtc,
                CompleteMessage = wizard.CompleteMessage
            };

            var phaseNo = 0;

            foreach (var phaseViewModel in wizard.Phases.OrderBy(p => p.SortOrder).Select(ToWorkflowPhaseViewModel))
            {
                phaseViewModel.Number = ++phaseNo;
                viewModel.Phases.Add(phaseViewModel);
            }

            return viewModel;
        }

        public static PhaseViewModel ToWorkflowPhaseViewModel(Phase phase)
        {
            var viewModel = new PhaseViewModel()
            {
                PhaseId = phase.PlanningWizardPhaseID,
                Name = phase.NameHtml,
                Selected = phase.IsSelected,
                SortOrder = phase.SortOrder
            };

            viewModel.Steps.AddRange(phase.Steps.Select(ToWorkflowStepViewModel));

            if (!viewModel.Steps.Any(s => s.Selected))
            {
                viewModel.Steps.First().Selected = true;
            }

            return viewModel;
        }

        public static StepViewModel ToWorkflowStepViewModel(Step step)
        {
            var viewModel = new StepViewModel()
            {
                Name = step.Name,
                Description = step.Description,
                StepId = step.PlanningWizardStepID,
                StepNo = step.StepNo,
                StepWeight = step.StepWeight,
                Notes = step.Notes,
                Selected = step.IsSelected,
                PhaseId = step.PlanningWizardPhaseID
            };

            for (var i = 0; i < step.ActionItems.Count; i++)
            {
                var item = step.ActionItems.ElementAt(i);

                viewModel.ActionItems.Add(new ActionItemViewModel()
                    {
                        ActionItemId = item.PlanningWizardActionItemID,
                        StepId = item.PlanningWizardStepID,
                        Text = item.ActionItemText,
                        ListNumber = (i + 1).ToString(),
                        Complete = item.IsComplete,
                        ResourceHtml = item.ResourcesContent
                    });
            }

            return viewModel;
        }

        public static Progress FromProgressViewModel(ProgressViewModel viewModel)
        {
            return new Progress()
            {
                PlanningWizardProgressID = viewModel.ProgressId,
                PercentComplete = viewModel.PercentComplete,
                CurrentPlanningWizardPhaseID = viewModel.CurrentPhaseId,
                Phases = viewModel.Phases.Select(FromPhaseViewModel).ToList()
            };
        }

        public static Phase FromPhaseViewModel(PhaseViewModel viewModel)
        {
            return new Phase()
            {
                PlanningWizardPhaseID = viewModel.PhaseId,
                IsSelected = viewModel.Selected,
                Steps = viewModel.Steps.Select(FromStepViewModel).ToList()
            };
        }

        public static Step FromStepViewModel(StepViewModel viewModel)
        {
            return new Step()
            {
                PlanningWizardStepID = viewModel.StepId,
                IsSelected = viewModel.Selected,
                Notes = viewModel.Notes,
                StepWeight = viewModel.StepWeight,
                ActionItems = viewModel.ActionItems.Select(FromActionItemViewModel).ToList()
            };
        }

        public static ActionItem FromActionItemViewModel(ActionItemViewModel viewModel)
        {
            return new ActionItem()
            {
                PlanningWizardActionItemID = viewModel.ActionItemId,
                IsComplete = viewModel.Complete,
                ResourcesContent = viewModel.ResourceHtml
            };
        }

        #endregion
    }
}
