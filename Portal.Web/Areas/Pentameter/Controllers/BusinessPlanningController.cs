using System.Collections.Generic;
using Portal.Domain.Extensions;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Areas.Pentameter.Models;
using Portal.Web.Common.Filters.Mvc;
using Portal.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Web.Areas.Pentameter.Controllers
{
    [ProfileTypeAuthorize(ProfileTypeValues.FinancialAdvisor)]
    public class BusinessPlanningController : BaseController
    {
        #region Private Members

        private readonly IBusinessPlanService _businessPlanService;
        private readonly IAffiliateService _affiliateService;

        #endregion

        #region Constructor

        public BusinessPlanningController(IUserService userService, ILogger logger, IBusinessPlanService businessPlanService, IAffiliateService affiliateService)
            : base(userService, logger)
        {
            _businessPlanService = businessPlanService;
            _affiliateService = affiliateService;
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Wizard(int? year)
        {
            var selectedYear = year ?? DateTime.Today.Year;
            var plans = _businessPlanService.GetBusinessPlans(new BusinessPlanRequest() {UserID = AssistedUser.UserID}).ToList();

            // If the user has plans but not one for the requested year...
            if (plans.Any() && plans.All(p => p.Year != selectedYear))
            {
                // Get the most recent plan that is less than the selected year
                var recentPlan = plans.OrderByDescending(p => p.Year).FirstOrDefault(p => p.Year < selectedYear);

                // If that doesn't exist, just get the last modified plan and call it a day
                selectedYear = recentPlan != null ? recentPlan.Year : plans.OrderByDescending(p => p.ModifyDate).First().Year;
            }

            ViewBag.SelectedYear = selectedYear;
            ViewBag.MaximumObjectives = Settings.Get<int>("Pentameter.BusinessPlan.Objectives.Max");

            return View("Wizard");
        }

        [HttpGet]
        public ActionResult Export(int year)
        {
            var plan = _businessPlanService.GetBusinessPlan(AssistedUser.UserID, year);

            return View("Export", plan);
        }

        [HttpGet]
        public ActionResult MyBusinessPlans(int? year)
        {
            var plans = _businessPlanService.GetBusinessPlans(new BusinessPlanRequest() { UserID = AssistedUser.UserID }).ToList();

            var businessPlan = default(BusinessPlan);

            if (year.HasValue)
            {
                businessPlan = plans.FirstOrDefault(p => p.Year == year.Value);

                if (businessPlan == null)
                    throw new ArgumentException("Business Plan does not exist for given year");
            }

            if (businessPlan == null)
            {
                businessPlan = plans.FirstOrDefault(p => p.Year == DateTime.Now.Year) ?? plans.FirstOrDefault();

                if (businessPlan == null)
                {
                    businessPlan = BusinessPlan.Default();
                }
            }

            var viewModel = new MyBusinessPlansViewModel
            {
                BusinessPlan = businessPlan,
                EditUrl = Url.Action("Wizard", new { year = businessPlan.Year }),
                ExportUrl = string.Format("/pdf/executeUrl?url={0}&title={1}%20Business%20Plan&type=BusinessPlan",
                                                Url.Action("Export", new { year = businessPlan.Year }),
                                                businessPlan.Year)
            };

            if (!plans.Any())
            {
                plans.Add(businessPlan);
            }

            foreach (var plan in plans.OrderByDescending(p => p.Year))
            {
                viewModel.PlanYears.Add(new SelectListItem
                {
                    Text = plan.Year.ToString(),
                    Value = Url.Action("MyBusinessPlans", new { year = plan.Year })
                });
            }

            return View("MyBusinessPlans", viewModel);
        }

        #endregion

        #region Swot Actions

        [HttpPost]
        public JsonResult JsonGetSwots(int businessPlanId)
        {
            return JsonResponse(() => _businessPlanService.GetSwots(businessPlanId));
        }

        [HttpPost]
        public JsonResult JsonCreateSwot(Swot swot)
        {
            return JsonResponse(() =>
            {
                swot.EnsureAuditFields(CurrentUser.UserID);

                _businessPlanService.CreateSwot(ref swot);

                return swot;
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateSwot(Swot swot)
        {
            return JsonResponse(() =>
            {
                swot.ModifyUserID = CurrentUser.UserID;
                swot.ModifyDate = DateTime.Now;
                swot.ModifyDateUtc = DateTime.UtcNow;

                _businessPlanService.UpdateSwot(ref swot);
            });
        }

        [HttpPost]
        public JsonResult JsonDeleteSwot(Swot swot)
        {
            return JsonResponse(() => _businessPlanService.DeleteSwot(swot));
        }

        #endregion

        #region Tactic Actions

        [HttpPost]
        public JsonResult JsonGetTactics(int businessPlanId)
        {
            return JsonResponse(() => _businessPlanService.GetTactics(businessPlanId));
        }

        [HttpPost]
        public JsonResult JsonCreateTactic(Tactic tactic)
        {
            return JsonResponse(() =>
            {
                tactic.EnsureAuditFields(CurrentUser.UserID);

                _businessPlanService.CreateTactic(ref tactic);

                return tactic;
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateTactic(Tactic tactic)
        {
            return JsonResponse(() =>
            {
                tactic.ModifyDate = DateTime.Now;
                tactic.ModifyDateUtc = DateTime.UtcNow;
                tactic.ModifyUserID = CurrentUser.UserID;

                _businessPlanService.UpdateTactic(ref tactic);
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateTacticStatus(int tacticId, bool isComplete)
        {
            return JsonResponse(() => _businessPlanService.UpdateTacticStatus(tacticId, isComplete, AssistedUser.UserID, CurrentUser.UserID));
        }

        [HttpPost]
        public JsonResult JsonDeleteTactic(Tactic tactic)
        {
            return JsonResponse(() => _businessPlanService.DeleteTactic(tactic));
        }

        #endregion

        #region Strategy Actions

        [HttpPost]
        public JsonResult JsonGetStrategies(int businessPlanId)
        {
            return JsonResponse(() => _businessPlanService.GetStrategies(businessPlanId));
        }

        [HttpPost]
        public JsonResult JsonCreateStrategy(Strategy strategy)
        {
            return JsonResponse(() =>
            {
                strategy.EnsureAuditFields(CurrentUser.UserID);

                _businessPlanService.CreateStrategy(ref strategy);

                return strategy;
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateStrategy(Strategy strategy)
        {
            return JsonResponse(() =>
            {
                strategy.ModifyDate = DateTime.Now;
                strategy.ModifyDateUtc = DateTime.UtcNow;
                strategy.ModifyUserID = CurrentUser.UserID;

                _businessPlanService.UpdateStrategy(ref strategy);
            });
        }

        [HttpPost]
        public JsonResult JsonDeleteStrategy(Strategy strategy)
        {
            return JsonResponse(() => _businessPlanService.DeleteStrategy(strategy));
        }

        #endregion

        #region Employee Actions

        [HttpPost]
        public JsonResult JsonGetEmployees(int businessPlanId)
        {
            return JsonResponse(() => _businessPlanService.GetEmployees(businessPlanId));
        }

        [HttpPost]
        public JsonResult JsonCreateEmployee(Employee employee)
        {
            return JsonResponse(() =>
            {
                employee.EnsureAuditFields(CurrentUser.UserID);

                _businessPlanService.CreateEmployee(ref employee);

                return employee;
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateEmployee(Employee employee)
        {
            return JsonResponse(() =>
            {
                employee.ModifyUserID = CurrentUser.UserID;
                employee.ModifyDate = DateTime.Now;
                employee.ModifyDateUtc = DateTime.UtcNow;

                _businessPlanService.UpdateEmployee(ref employee);
            });
        }

        [HttpPost]
        public JsonResult JsonDeleteEmployee(Employee employee)
        {
            return JsonResponse(() => _businessPlanService.DeleteEmployee(employee));
        }

        #endregion

        #region Objective Actions

        [HttpPost]
        public JsonResult JsonGetObjectives(int businessPlanId)
        {
            return JsonResponse(() => _businessPlanService.GetObjectives(businessPlanId));
        }

        [HttpPost]
        public JsonResult JsonCreateObjective(Objective objective)
        {
            return JsonResponse(() =>
            {
                objective.Validate();

                objective.EnsureAuditFields(CurrentUser.UserID);

                _businessPlanService.CreateObjective(ref objective, AssistedUser.UserID);

                return objective;
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateObjective(Objective objective)
        {
            return JsonResponse(() =>
            {
                objective.Validate();

                objective.ModifyDate = DateTime.Now;
                objective.ModifyDateUtc = DateTime.UtcNow;
                objective.ModifyUserID = CurrentUser.UserID;

                _businessPlanService.UpdateObjective(ref objective);
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateObjectiveStatus(int objectiveId, int percentComplete)
        {
            return JsonResponse(() => _businessPlanService.UpdateObjectiveStatus(objectiveId, percentComplete, AssistedUser.UserID, CurrentUser.UserID));
        }

        [HttpPost]
        public JsonResult JsonDeleteObjective(Objective objective)
        {
            return JsonResponse(() => _businessPlanService.DeleteObjective(objective));
        }

        #endregion

        #region Employee Role Actions

        [HttpPost]
        public JsonResult JsonGetEmployeeRoles(int businessPlanId)
        {
            return JsonResponse(() => _businessPlanService.GetEmployeeRoles(businessPlanId));
        }

        [HttpPost]
        public JsonResult JsonCreateEmployeeRole(EmployeeRole employeeRole)
        {
            return JsonResponse(() =>
            {
                employeeRole.EnsureAuditFields(CurrentUser.UserID, DateTime.Now);

                _businessPlanService.CreateEmployeeRole(ref employeeRole);

                return employeeRole;
            });
        }

        [HttpPost]
        public JsonResult JsonUpdateEmployeeRole(EmployeeRole employeeRole)
        {
            return JsonResponse(() =>
            {
                employeeRole.ModifyDate = DateTime.Now;
                employeeRole.ModifyDateUtc = DateTime.UtcNow;
                employeeRole.ModifyUserID = CurrentUser.UserID;

                _businessPlanService.UpdateEmployeeRole(ref employeeRole);
            });
        }

        [HttpPost]
        public JsonResult JsonDeleteEmployeeRole(EmployeeRole employeeRole)
        {
            return JsonResponse(() => _businessPlanService.DeleteEmployeeRole(employeeRole));
        }

        #endregion

        #region Business Plan Actions

        [HttpPost]
        public JsonResult JsonGetBusinessPlan(int year)
        {
            return JsonResponse(() =>
            {
                var businessPlan = _businessPlanService.GetBusinessPlan(AssistedUser.UserID, year);

                return GetBusinessPlanWithEditorViewModel(businessPlan);
            });
        }

        [HttpPost]
        public JsonResult JsonCreateBusinessPlan(int year, int? copyFromYear)
        {
            var plan = new BusinessPlan();

            if (copyFromYear.HasValue)
            {
                plan = _businessPlanService.CopyBusinessPlan(year, copyFromYear.Value, AssistedUser.UserID, CurrentUser.UserID);
            }
            else
            {
                // This will create the plan if it does not exist
                plan = _businessPlanService.GetBusinessPlan(AssistedUser.UserID, year);
            }

            return JsonResponse(() => GetBusinessPlanWithEditorViewModel(plan));
        }

        [HttpPost]
        public JsonResult JsonUpdateBusinessPlan(BusinessPlan businessPlan)
        {
            return JsonResponse(() =>
            {
                businessPlan.ModifyUserID = CurrentUser.UserID;
                businessPlan.ModifyDate = DateTime.Now;
                businessPlan.ModifyDateUtc = DateTime.UtcNow;

                _businessPlanService.UpdateBusinessPlan(ref businessPlan);
            });
        }

        [HttpPost]
        public JsonResult JsonDeleteBusinessPlan(int businessPlanId)
        {
            _businessPlanService.DeleteBusinessPlan(businessPlanId, AssistedUser.UserID, CurrentUser.UserID);

            var latestPlan = _businessPlanService.GetLatestBusinessPlan(AssistedUser.UserID);

            var year = latestPlan != null ? latestPlan.Year : DateTime.Now.Year;

            return JsonGetBusinessPlan(year);
        }

        #endregion

        #region Private Methods

        private WizardViewModel GetEditorViewModel()
        {
            var viewModel = new WizardViewModel();
            var currentYear = DateTime.Today.Year;
            var nextYear = currentYear + 1;

            var planYears = _businessPlanService.GetBusinessPlanYears(AssistedUser.UserID).ToList();

            //if (!planYears.Contains(currentYear))
            //{
            //    planYears.Add(currentYear);
            //}

            planYears.Sort();
            planYears.Reverse();

            // Add blanks
            viewModel.ExistingPlanYears.Add(new SelectListItem { Text = "", Value = "" });
            viewModel.AvailablePlanYears.Add(new SelectListItem { Text = "", Value = "" });

            foreach (var year in planYears)
            {
                viewModel.ExistingPlanYears.Add(new SelectListItem
                {
                    Text = year.ToString(),
                    Value = year.ToString()
                });
            }

            for (var i = currentYear; i < currentYear + 6; i++)
            {
                if (!planYears.Contains(i))
                {
                    viewModel.AvailablePlanYears.Add(new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }
            }

            return viewModel;
        }

        private object GetBusinessPlanWithEditorViewModel(BusinessPlan businessPlan)
        {
            var editorViewModel = GetEditorViewModel();

            // Trim down biz plan (these are all fetched separately)
            businessPlan.Objectives.Clear();
            businessPlan.Strategies.Clear();
            businessPlan.Tactics.Clear();
            businessPlan.Swots.Clear();
            businessPlan.EmployeeRoles.Clear();
            businessPlan.Employees.Clear();

            return new
                {
                    EmployeeRoles = _businessPlanService.GetPreDefinedEmployeeRoles(),
                    Objectives = GetPreDefinedObjectives(),
                    BusinessPlan = businessPlan,
                    editorViewModel.ExistingPlanYears,
                    editorViewModel.AvailablePlanYears,
                    selectedYear = businessPlan.Year
                };
        }

        private List<Objective> GetPreDefinedObjectives()
        {
            var preDefinedObjectives = _businessPlanService.GetPreDefinedObjectives().ToList();
            var affiliateObjectives = _affiliateService.GetObjectives(AssistedUser.AffiliateID).ToList();

            foreach (var po in preDefinedObjectives)
            {
                var ao = affiliateObjectives.FirstOrDefault(o => o.ObjectiveID == po.ObjectiveID);

                if (ao != null)
                    po.AutoTrackingEnabled = ao.AutoTrackingEnabled;
            }

            return preDefinedObjectives;
        }

        #endregion
    }
}
