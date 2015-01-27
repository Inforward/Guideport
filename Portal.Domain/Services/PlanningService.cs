using System;
using System.Collections.Generic;
using Portal.Data;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Model.Planning;
using Portal.Services.Contracts;
using System.Data.Entity;
using System.Linq;

namespace Portal.Domain.Services
{
    public class PlanningService : IPlanningService
    {
        private readonly IPlanningRepository _planningRepository;
        private readonly ISurveyService _surveyService;

        public PlanningService(IPlanningRepository planningRepository, ISurveyService surveyService)
        {
            _planningRepository = planningRepository;
            _surveyService = surveyService;
        }

        public Wizard GetWizard(WizardRequest request)
        {
            if (request == null) return null;

            var query = _planningRepository.FindBy<Wizard>(w => w.PlanningWizardID == request.PlanningWizardID);

            if (request.IncludePhases)
                query = query.Include("Phases")
                             .Include("Phases.Steps")
                             .Include("Phases.Steps.ActionItems");

            return query.FirstOrDefault();
        }

        public Wizard GetProgress(ProgressRequest request)
        {
            var wizardId = request.PlanningWizardID;
            var userId = request.UserID;
            var wizard = _planningRepository.FindBy<Wizard>(w => w.PlanningWizardID == wizardId)
                                            .Include("Phases")
                                            .Include("Phases.Steps")
                                            .Include("Phases.Steps.ActionItems")
                                            .FirstOrDefault();

            if (wizard == null)
                throw new Exception("Invalid Planning Wizard Id: " + wizardId);

            var progress = _planningRepository.FindBy<Progress>(p => p.UserID == userId && p.PlanningWizardID == wizardId).FirstOrDefault();

            if (progress == null)
            {
                progress = new Progress()
                {
                    PlanningWizardID = wizardId,
                    UserID = userId,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    CreateUserID = userId,
                    ModifyUserID = userId,
                    CurrentPlanningWizardPhaseID = wizard.Phases.First().PlanningWizardPhaseID,
                    PercentComplete = 0,
                    ProgressXml = wizard.Phases.ToXml()
                };

                _planningRepository.Add(progress);
                _planningRepository.Save();
            }

            // Re-hydrate
            progress.Phases = progress.ProgressXml.FromXml<List<Phase>>();

            // Set progress as property
            wizard.Progress = progress;

            // Merge with Wizard data
            foreach (var progressPhase in progress.Phases)
            {
                var phase = wizard.Phases.First(p => p.PlanningWizardPhaseID == progressPhase.PlanningWizardPhaseID);

                phase.IsSelected = phase.PlanningWizardPhaseID.Equals(progress.CurrentPlanningWizardPhaseID);

                foreach (var progressStep in progressPhase.Steps)
                {
                    var step = phase.Steps.First(s => s.PlanningWizardStepID == progressStep.PlanningWizardStepID);

                    step.Notes = progressStep.Notes;
                    step.IsSelected = progressStep.IsSelected;

                    foreach (var progressActionItem in progressStep.ActionItems)
                    {
                        var actionItem = step.ActionItems.First(a => a.PlanningWizardActionItemID == progressActionItem.PlanningWizardActionItemID);

                        actionItem.IsComplete = progressActionItem.IsComplete;
                        actionItem.CompleteDate = progressActionItem.CompleteDate;
                        actionItem.CompleteDateUtc = progressActionItem.CompleteDateUtc;
                    }
                }
            }

            return wizard;
        }

        public Progress SaveProgress(Progress progress, int userId)
        {
            var progressId = progress.PlanningWizardProgressID;
            var existingProgress = _planningRepository.FindBy<Progress>(p => p.UserID == userId && p.PlanningWizardProgressID == progressId).FirstOrDefault();

            if (existingProgress == null)
                throw new Exception("Planning progress does not exist");

            UpdateActionItems(progress, existingProgress);

            existingProgress.ModifyDate = progress.ModifyDate;
            existingProgress.ModifyDateUtc = progress.ModifyDateUtc;
            existingProgress.ModifyUserID = progress.ModifyUserID;

            existingProgress.PercentComplete = progress.PercentComplete;
            existingProgress.CurrentPlanningWizardPhaseID = progress.CurrentPlanningWizardPhaseID;
            existingProgress.ProgressXml = progress.Phases.ToXml();

            _planningRepository.Update(existingProgress);
            _planningRepository.Save();

            return existingProgress;            
        }

        public ActionItem GetActionItem(int actionItemId)
        {
            return _planningRepository.FindBy<ActionItem>(a => a.PlanningWizardActionItemID == actionItemId).FirstOrDefault();
        }

        public void UpdateActionItem(ref ActionItem actionItem)
        {
            if (actionItem == null) return;

            _planningRepository.Update(actionItem);
            _planningRepository.Save();
        }

        public EnrollmentStatus GetEnrollmentStatus(int userId)
        {
            var survey = _surveyService.GetSurvey(new SurveyRequest
            {
                SurveyName = Settings.SurveyNames.Enrollment,
                UserID = userId,
                IncludeResponse = true
            });

            var enrollmentStatus = new EnrollmentStatus()
            {
                Pages = survey.Status.Pages,
                PercentComplete = survey.Status.PercentComplete,
                State = survey.Status.State
            };

            var questionA = survey.GetQuestion("My Interests", "Acquisition");
            var questionC = survey.GetQuestion("My Interests", "Continuity");
            var questionS = survey.GetQuestion("My Interests", "Succession");
            var questionF = survey.GetQuestion("My Interests", "NeedAcquisitionFunding");

            // Business Acquisitions
            if (questionA != null && questionA.HasAnswer)
            {
                var answer = questionA.FindPossibleAnswer("I am interested in Business Acquisitions.");

                enrollmentStatus.BusinessAcquisition.Interested = answer != null && answer.IsSelected;
                enrollmentStatus.BusinessAcquisition.HasAnswer = true;
            }

            // Funding
            if (questionF != null && questionF.HasAnswer && enrollmentStatus.BusinessAcquisition.Interested)
            {
                var answer = questionF.FindPossibleAnswer("Yes, I need funding and want to learn more.");

                enrollmentStatus.BusinessAcquisitionFunding.Interested = answer != null && answer.IsSelected;
                enrollmentStatus.BusinessAcquisitionFunding.HasAnswer = true;
            }

            // Continuity
            if (questionC != null && questionC.HasAnswer)
            {
                var answer = questionC.FindPossibleAnswer("I have a written Continuity Plan and I am satisfied that my plan meets all my needs.");

                enrollmentStatus.ContinuityPlanning.Interested = answer != null && !answer.IsSelected;
                enrollmentStatus.ContinuityPlanning.HasAnswer = true;
            }

            // Succession
            if (questionS != null && questionS.HasAnswer)
            {
                var answer = questionS.FindPossibleAnswer("I have a written Succession Plan and I am satisfied that my plan meets all my needs.");

                enrollmentStatus.SuccessionPlanning.Interested = answer != null && !answer.IsSelected;
                enrollmentStatus.SuccessionPlanning.HasAnswer = true;
            }

            // Messages
            enrollmentStatus.ContinuityPlanning.Message = GetInterestMessage(questionC);
            enrollmentStatus.SuccessionPlanning.Message = GetInterestMessage(questionS);
            enrollmentStatus.BusinessAcquisition.Message = GetInterestMessage(questionA);
            enrollmentStatus.BusinessAcquisitionFunding.Message = GetInterestMessage(questionF);

            // Overwrite message if interested in both acquisition and funding
            if (enrollmentStatus.BusinessAcquisition.Interested)
            {
                enrollmentStatus.BusinessAcquisition.Message = enrollmentStatus.BusinessAcquisitionFunding.Interested ?
                    "You are interested in acquiring a business and need funding and want to learn more." :
                    "You are interested in acquiring a business and do not need funding.";
            }

            return enrollmentStatus;            
        }

        public PlanningSummary GetPlanningSummary(int planningWizardId, int userId)
        {
            var wizard = GetProgress(new ProgressRequest { UserID = userId, PlanningWizardID = planningWizardId });

            return new PlanningSummary()
            {
                PlanningWizardID = wizard.PlanningWizardID,
                PlanningName = wizard.Name,
                PercentComplete = wizard.Progress.PercentComplete
            };
        }

        private static void UpdateActionItems(Progress progress, Progress existingProgress)
        {
            // Re-hydrate if necessary
            if (existingProgress.Phases.Count == 0 && !string.IsNullOrEmpty(existingProgress.ProgressXml))
                existingProgress.Phases = existingProgress.ProgressXml.FromXml<List<Phase>>();

            var existingItems = existingProgress.GetActionItems().ToList();

            foreach (var actionItem in progress.GetActionItems())
            {
                var existingItem = existingItems.FirstOrDefault(a => a.PlanningWizardActionItemID == actionItem.PlanningWizardActionItemID);

                if (existingItem != null)
                {
                    if (actionItem.IsComplete && !existingItem.IsComplete)
                    {
                        actionItem.CompleteDate = DateTime.Now;
                        actionItem.CompleteDateUtc = DateTime.UtcNow;
                    }
                    else if (actionItem.IsComplete && existingItem.IsComplete)
                    {
                        actionItem.CompleteDate = existingItem.CompleteDate;
                        actionItem.CompleteDateUtc = existingItem.CompleteDateUtc;
                    }
                    else if (!actionItem.IsComplete)
                    {
                        actionItem.CompleteDate = actionItem.CompleteDateUtc = null;
                    }
                }
            }
        }

        private string GetInterestMessage(SurveyQuestion question)
        {
            var message = "Your interest was not specified.  Please update your enrollment profile.";

            if (question != null && question.HasAnswer)
            {
                switch (question.Answer)
                {
                    case "I don't have a Continuity Plan and I would like assistance creating a Continuity Plan.":
                        message = "You don't have a Continuity Plan and would like assistance creating one.";
                        break;
                    case "I have a written Continuity Plan but I would like assistance updating my plan.":
                        message = "You have a Continuity Plan but would like assistance updating it.";
                        break;
                    case "I have a written Continuity Plan and I am satisfied that my plan meets all my needs.":
                        message = "You already have a Continuity Plan and are satisfied it meets all your needs.";
                        break;
                    case "I am planning to exit and want to get started on the process.":
                        message = "You are planning to exit and want to get started on the process.";
                        break;
                    case "I have a written Succession Plan and I would like assistance updating my plan.":
                        message = "You have a Succession Plan but would like assistance updating it.";
                        break;
                    case "I have a written Succession Plan and I am satisfied that my plan meets all my needs.":
                        message = "You already have a Succession Plan and are satisfied it meets all your needs.";
                        break;
                    case "I am interested in Business Acquisitions.":
                        message = "You are interested in acquiring a business.";
                        break;
                    case "I have no interests in Business Acquisitions at this time.":
                        message = "You are not interested in acquiring a business at this time.";
                        break;
                    case "Yes, I need funding and want to learn more.":
                        message = "You need funding and want to learn more.";
                        break;
                }
            }

            return message;
        }
    }
}
