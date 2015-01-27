using Portal.Data;
using Portal.Data.Sql.EntityFramework;
using Portal.Model;
using Portal.Model.Attributes;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Portal.Domain.Services
{
    public class BusinessPlanService : IBusinessPlanService
    {
        private readonly IBusinessPlanRepository _businessPlanRepository;
        private readonly IUserService _userService;

        public BusinessPlanService(IBusinessPlanRepository businessPlanRepository, IUserService userService)
        {
            _businessPlanRepository = businessPlanRepository;
            _userService = userService;
        }

        public IEnumerable<BusinessPlan> GetBusinessPlans(BusinessPlanRequest criteria)
        {
            var plans = _businessPlanRepository.GetAll<BusinessPlan>()
                                    .Where(b => criteria.Year == 0 || b.Year == criteria.Year)
                                    .Where(b => criteria.UserID == 0 || b.UserID == criteria.UserID)
                                    .Where(b => b.DeleteDate == null)
                                    .AsNoTracking()
                                    .IncludeAll()
                                    .ToList();

            if (!plans.Any())
                return plans;

            var users = _userService.GetUsers(new UserRequest() { UserIDs = plans.Select(b => b.UserID).ToList() }).Users;

            foreach (var plan in plans.Where(p => p.Objectives.Any()))
            {
                var user = users.FirstOrDefault(u => u.UserID == plan.UserID);

                if (user == null)
                    continue;

                foreach (var objective in plan.Objectives)
                {
                    var value = user.GetMappedObjectiveProperty(objective.Name);

                    if (value != null)
                    {
                        objective.CurrentValue = value.ToString();
                    }

                    if (user.MetricsUpdateDate != DateTime.MinValue)
                    {
                        objective.CurrentValueDate = user.MetricsUpdateDate;
                    }
                }
            }

            return plans;
        }

        public BusinessPlan GetBusinessPlan(int userId, int year)
        {
            var businessPlan = _businessPlanRepository.FindBy<BusinessPlan>(b => b.UserID == userId && b.Year == year && b.DeleteDate == null)
                                    .AsNoTracking()
                                    .IncludeAll()
                                    .FirstOrDefault();

            if (businessPlan == null)
            {
                businessPlan = CreateBusinessPlan(userId, year);
            }
            else
            {
                businessPlan = SyncPredefinedStrategiesAndTactics(businessPlan);
            }

            PopulateObjectiveProgress(businessPlan);

            return businessPlan;
        }

        public BusinessPlan GetLatestBusinessPlan(int userId)
        {
            return _businessPlanRepository.FindBy<BusinessPlan>(b => b.UserID == userId && b.DeleteDate == null)
                                                                .OrderByDescending(b => b.Year)
                                                                .FirstOrDefault();
        }

        public BusinessPlan CopyBusinessPlan(int year, int copyFromYear, int userId, int auditUserId)
        {
            var existingPlan = _businessPlanRepository.FindBy<BusinessPlan>(b => b.UserID == userId && b.Year == copyFromYear && b.DeleteDate == null)
                                    .AsNoTracking()
                                    .IncludeAll()
                                    .FirstOrDefault();

            if (existingPlan != null)
            {
                // Copy core plan
                var plan = new BusinessPlan()
                {
                    Year = year,
                    UserID = existingPlan.UserID,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    CreateUserID = auditUserId,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    ModifyUserID = auditUserId,
                    MissionWhat = existingPlan.MissionWhat,
                    MissionWhy = existingPlan.MissionWhy,
                    MissionHow = existingPlan.MissionHow,
                    VisionOneYear = existingPlan.VisionOneYear,
                    VisionFiveYear = existingPlan.VisionFiveYear
                };

                using (var context = _businessPlanRepository.BeginTransaction())
                {
                    try
                    {
                        _businessPlanRepository.Add(plan);
                        _businessPlanRepository.Save();

                        // Copy Flat Data
                        CopySwots(plan, existingPlan);
                        CopyEmployees(plan, existingPlan);
                        CopyEmployeeRoles(plan, existingPlan);
                        CopyTactics(plan, existingPlan);
                        CopyStrategies(plan, existingPlan);
                        CopyObjectives(plan, existingPlan);

                        // Get a fresh plan back that has all the PKs populated
                        plan = _businessPlanRepository.GetBusinessPlanById(plan.BusinessPlanID);

                        // Now, establish the relationships
                        CopyEmployeeRoleAssocations(plan, existingPlan);
                        CopyObjectiveAssociations(plan, existingPlan);

                        context.Commit();
                    }
                    catch (Exception)
                    {
                        context.Rollback();
                        throw;
                    }
                }
            }

            // Return fresh copy
            return GetBusinessPlan(userId, year);
        }

        public void UpdateBusinessPlan(ref BusinessPlan businessPlan)
        {
            _businessPlanRepository.UpdateBusinessPlan(businessPlan);
        }

        public void DeleteBusinessPlan(int businessPlanId, int userId, int deleteUserId)
        {
            var plan = _businessPlanRepository.FindBy<BusinessPlan>(b => b.BusinessPlanID == businessPlanId && b.UserID == userId)
                                              .FirstOrDefault();

            if (plan != null)
            {
                plan.DeleteUserID = deleteUserId;
                plan.DeleteDate = DateTime.Now;
                plan.DeleteDateUtc = DateTime.UtcNow;

                _businessPlanRepository.Update(plan);
                _businessPlanRepository.Save();
            }
        }

        public IEnumerable<int> GetBusinessPlanYears(int? userId = null)
        {
            if (userId.HasValue)
                return _businessPlanRepository.GetBusinessPlans(userId.Value).Select(b => b.Year).ToList();

            return _businessPlanRepository.GetAll<BusinessPlan>().Select(b => b.Year).ToList();
        }

        public IEnumerable<Swot> GetSwots(int businessPlanId)
        {
            return _businessPlanRepository.FindBy<Swot>(s => s.BusinessPlanID == businessPlanId)
                                            .AsNoTracking()
                                            .OrderBy(s => s.Type)
                                            .ThenBy(s => s.Description)
                                            .ToList();
        }

        public void CreateSwot(ref Swot swot)
        {
            _businessPlanRepository.AddSwot(swot);
        }

        public void UpdateSwot(ref Swot swot)
        {
            _businessPlanRepository.UpdateSwot(swot);
        }

        public void DeleteSwot(Swot swot)
        {
            _businessPlanRepository.DeleteSwot(swot);
        }

        public IEnumerable<Tactic> GetTactics(int businessPlanId)
        {
            return _businessPlanRepository.FindBy<Tactic>(er => er.BusinessPlanID == businessPlanId)
                                            .AsNoTracking()
                                            .OrderBy(er => er.Name)
                                            .ToList();
        }

        public void CreateTactic(ref Tactic tactic)
        {
            _businessPlanRepository.AddTactic(tactic);
        }

        public void UpdateTactic(ref Tactic tactic)
        {
            _businessPlanRepository.UpdateTactic(tactic);
        }

        public void UpdateTacticStatus(int tacticId, bool isComplete, int userId, int modifyUserId)
        {
            var tactic = _businessPlanRepository.FindBy<Tactic>(t =>
                                t.BusinessPlan.UserID == userId &&
                                t.TacticID == tacticId).FirstOrDefault();

            if (tactic == null)
                throw new Exception("Could not find tactic to update");

            tactic.CompletedDate = isComplete ? DateTime.Now : (DateTime?)null;
            tactic.ModifyUserID = modifyUserId;
            tactic.ModifyDate = DateTime.Now;
            tactic.ModifyDateUtc = DateTime.UtcNow;

            _businessPlanRepository.Update(tactic);
            _businessPlanRepository.Save();
        }

        public void DeleteTactic(Tactic tactic)
        {
            _businessPlanRepository.DeleteTactic(tactic);
        }

        public IEnumerable<Strategy> GetStrategies(int businessPlanId)
        {
            return _businessPlanRepository.FindBy<Strategy>(er => er.BusinessPlanID == businessPlanId)
                                            .AsNoTracking()
                                            .Include("Tactics")
                                            .OrderBy(er => er.Name)
                                            .ToList();
        }

        public void CreateStrategy(ref Strategy strategy)
        {
            _businessPlanRepository.AddStrategy(strategy);
        }

        public void UpdateStrategy(ref Strategy strategy)
        {
            _businessPlanRepository.UpdateStrategy(strategy);
        }

        public void DeleteStrategy(Strategy strategy)
        {
            _businessPlanRepository.DeleteStrategy(strategy);
        }

        public IEnumerable<Employee> GetEmployees(int businessPlanId)
        {
            return _businessPlanRepository.FindBy<Employee>(e => e.BusinessPlanID == businessPlanId)
                                            .AsNoTracking()
                                            .Include("EmployeeRoles")
                                            .OrderBy(e => e.FirstName)
                                            .ThenBy(e => e.LastName)
                                            .ToList();
        }

        public void CreateEmployee(ref Employee employee)
        {
            _businessPlanRepository.AddEmployee(employee);
        }

        public void UpdateEmployee(ref Employee employee)
        {
            _businessPlanRepository.UpdateEmployee(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            _businessPlanRepository.DeleteEmployee(employee);
        }

        public IEnumerable<Objective> GetObjectives(int businessPlanId)
        {
            return _businessPlanRepository.FindBy<Objective>(er => er.BusinessPlanID == businessPlanId)
                                            .AsNoTracking()
                                            .Include("Strategies")
                                            .Include("Strategies.Tactics")
                                            .OrderBy(er => er.Name)
                                            .ToList();
        }

        public IEnumerable<Objective> GetPreDefinedObjectives()
        {
            return _businessPlanRepository.GetPreDefinedEntity<Objective>();
        }

        public void CreateObjective(ref Objective objective, int userId)
        {
            var businessPlanId = objective.BusinessPlanID;
            var objectiveName = objective.Name;

            var plan = _businessPlanRepository.FindBy<BusinessPlan>(b => b.BusinessPlanID == businessPlanId.Value).First();

            if (plan.Year == DateTime.Today.Year)
            {
                var user = _userService.GetUserByUserId(userId);

                var value = user.GetMappedObjectiveProperty(objective.Name);

                if (value != null)
                {
                    objective.BaselineValue = value.ToString();
                    objective.BaselineValueDate = user.MetricsUpdateDate;
                }
            }

            _businessPlanRepository.AddObjective(objective);

            objective = _businessPlanRepository.FindBy<Objective>(o => o.BusinessPlanID == businessPlanId.Value)
                                               .Where(o => o.Name == objectiveName)
                                               .Include("Strategies")
                                               .Include("Strategies.Tactics")
                                               .FirstOrDefault();
        }

        public void UpdateObjective(ref Objective objective)
        {
            _businessPlanRepository.UpdateObjective(objective);
        }

        public void UpdateObjectiveStatus(int objectiveId, int percentComplete, int userId, int modifyUserId)
        {
            var objective = _businessPlanRepository.FindBy<Objective>(o =>
                                o.BusinessPlan.UserID == userId &&
                                o.ObjectiveID == objectiveId).FirstOrDefault();

            if (objective == null)
                throw new Exception(string.Format("Could not find objective to update.  UserID {0}.  ObjectiveID:  {1}", userId, objectiveId));

            objective.PercentComplete = percentComplete;
            objective.ModifyUserID = modifyUserId;
            objective.ModifyDate = DateTime.Now;
            objective.ModifyDateUtc = DateTime.UtcNow;

            _businessPlanRepository.Update(objective);
            _businessPlanRepository.Save();
        }

        public void DeleteObjective(Objective objective)
        {
            _businessPlanRepository.DeleteObjective(objective);
        }

        public IEnumerable<EmployeeRole> GetEmployeeRoles(int businessPlanId)
        {
            return _businessPlanRepository.FindBy<EmployeeRole>(er => er.BusinessPlanID == businessPlanId)
                                            .AsNoTracking()
                                            .Include("Employees")
                                            .OrderBy(er => er.Name)
                                            .ToList();
        }

        public IEnumerable<EmployeeRole> GetPreDefinedEmployeeRoles()
        {
            return _businessPlanRepository.GetPreDefinedEntity<EmployeeRole>();
        }

        public void CreateEmployeeRole(ref EmployeeRole employeeRole)
        {
            var businessPlanId = employeeRole.BusinessPlanID;
            var employeeRoleName = employeeRole.Name;

            _businessPlanRepository.AddEmployeeRole(employeeRole);

            employeeRole = _businessPlanRepository.FindBy<EmployeeRole>(er => er.BusinessPlanID == businessPlanId)
                                                   .Where(er => er.Name == employeeRoleName)
                                                   .Include("Employees")
                                                   .FirstOrDefault();
        }

        public void UpdateEmployeeRole(ref EmployeeRole employeeRole)
        {
            _businessPlanRepository.UpdateEmployeeRole(employeeRole);
        }

        public void DeleteEmployeeRole(EmployeeRole employeeRole)
        {
            _businessPlanRepository.DeleteEmployeeRole(employeeRole);
        }

        #region Private Methods

        private void CopySwots(BusinessPlan plan, BusinessPlan existingPlan)
        {
            foreach (var existingSwot in existingPlan.Swots)
            {
                var swot = new Swot()
                {
                    BusinessPlanID = plan.BusinessPlanID,
                    Type = existingSwot.Type,
                    Description = existingSwot.Description,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    CreateUserID = plan.CreateUserID,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    ModifyUserID = plan.ModifyUserID
                };

                _businessPlanRepository.AddSwot(swot);
            }
        }

        private void CopyEmployees(BusinessPlan plan, BusinessPlan existingPlan)
        {
            var list = new Dictionary<int, Employee>();

            foreach (var existingEmployee in existingPlan.Employees)
            {
                var employee = new Employee()
                {
                    BusinessPlanID = plan.BusinessPlanID,
                    FirstName = existingEmployee.FirstName,
                    MiddleInitial = existingEmployee.MiddleInitial,
                    LastName = existingEmployee.LastName,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    CreateUserID = plan.CreateUserID,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    ModifyUserID = plan.ModifyUserID
                };

                _businessPlanRepository.AddEmployee(employee);

                // Keep track of mapping
                list.Add(existingEmployee.EmployeeID, employee);
            }

            // Re-map the parents now that we have new IDs
            foreach (var existingEmployee in existingPlan.Employees.Where(e => e.EmployeeParentID.HasValue))
            {
                var newParentEmployee = list[existingEmployee.EmployeeParentID.Value];
                var newEmployee = list[existingEmployee.EmployeeID];

                newEmployee.EmployeeParentID = newParentEmployee.EmployeeID;

                _businessPlanRepository.Update(newEmployee);
                _businessPlanRepository.Save();
            }
        }

        private void CopyEmployeeRoles(BusinessPlan plan, BusinessPlan existingPlan)
        {
            foreach (var existingRole in existingPlan.EmployeeRoles)
            {
                var role = new EmployeeRole()
                {
                    BusinessPlanID = plan.BusinessPlanID,
                    Name = existingRole.Name,
                    Description = existingRole.Description,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    CreateUserID = plan.CreateUserID,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    ModifyUserID = plan.ModifyUserID
                };

                _businessPlanRepository.AddEmployeeRole(role);
            }
        }

        private void CopyTactics(BusinessPlan plan, BusinessPlan existingPlan)
        {
            foreach (var existingTactic in existingPlan.Tactics)
            {
                var tactic = new Tactic()
                {
                    BusinessPlanID = plan.BusinessPlanID,
                    Name = existingTactic.Name,
                    Description = existingTactic.Description,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    CreateUserID = plan.CreateUserID,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    ModifyUserID = plan.ModifyUserID
                };

                _businessPlanRepository.AddTactic(tactic);
            }
        }

        private void CopyStrategies(BusinessPlan plan, BusinessPlan existingPlan)
        {
            foreach (var existingStrategy in existingPlan.Strategies)
            {
                var strategy = new Strategy()
                {
                    BusinessPlanID = plan.BusinessPlanID,
                    Name = existingStrategy.Name,
                    Description = existingStrategy.Description,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    CreateUserID = plan.CreateUserID,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    ModifyUserID = plan.ModifyUserID
                };

                _businessPlanRepository.AddStrategy(strategy);
            }
        }

        private void CopyObjectives(BusinessPlan plan, BusinessPlan existingPlan)
        {
            foreach (var existingObjective in existingPlan.Objectives)
            {
                var objective = new Objective()
                {
                    BusinessPlanID = plan.BusinessPlanID,
                    Name = existingObjective.Name,
                    Value = existingObjective.Value,
                    BaselineValue = null,
                    DataType = existingObjective.DataType,
                    PercentComplete = 0,
                    AutoTrackingEnabled = existingObjective.AutoTrackingEnabled,
                    CreateDate = DateTime.Now,
                    CreateDateUtc = DateTime.UtcNow,
                    CreateUserID = plan.CreateUserID,
                    ModifyDate = DateTime.Now,
                    ModifyDateUtc = DateTime.UtcNow,
                    ModifyUserID = plan.ModifyUserID
                };

                if (existingObjective.EstimatedCompletionDate.HasValue)
                {
                    objective.EstimatedCompletionDate = new DateTime(plan.Year, existingObjective.EstimatedCompletionDate.Value.Month, existingObjective.EstimatedCompletionDate.Value.Day);
                }

                _businessPlanRepository.AddObjective(objective);
            }
        }

        private void CopyEmployeeRoleAssocations(BusinessPlan plan, BusinessPlan existingPlan)
        {
            foreach (var existingRole in existingPlan.EmployeeRoles)
            {
                var role = plan.EmployeeRoles.Find(r => r.Name == existingRole.Name);

                foreach (var existingEmployee in existingRole.Employees)
                {
                    var employee = plan.Employees.FirstOrDefault(e => e.FullName == existingEmployee.FullName);

                    if (employee != null)
                    {
                        role.Employees.Add(employee);
                    }
                }

                _businessPlanRepository.UpdateEmployeeRole(role);
            }
        }

        private void CopyObjectiveAssociations(BusinessPlan plan, BusinessPlan existingPlan)
        {
            foreach (var existingObjective in existingPlan.Objectives)
            {
                var objective = plan.Objectives.Find(o => o.Name == existingObjective.Name);

                foreach (var existingStrategy in existingObjective.Strategies)
                {
                    var strategy = plan.Strategies.Find(s => s.Name == existingStrategy.Name);

                    foreach (var existingTactic in existingStrategy.Tactics)
                    {
                        var tactic = plan.Tactics.Find(t => t.Name == existingTactic.Name);

                        if (!strategy.Tactics.Contains(tactic))
                        {
                            strategy.Tactics.Add(tactic);
                        }
                    }

                    objective.Strategies.Add(strategy);
                }

                _businessPlanRepository.UpdateObjective(objective);
            }
        }

        private BusinessPlan CreateBusinessPlan(int userId, int year)
        {
            var existingPlan = _businessPlanRepository.FindBy<BusinessPlan>(b => b.UserID == userId && b.Year == year && b.DeleteDate == null).FirstOrDefault();

            if(existingPlan != null)
                throw new Exception("A business plan for the given year already exists for this user.");

            var plan = new BusinessPlan()
            {
                Year = year,
                UserID = userId,
                CreateUserID = userId,
                CreateDate = DateTime.Now,
                CreateDateUtc = DateTime.UtcNow,
                ModifyUserID = userId,
                ModifyDate = DateTime.Now,
                ModifyDateUtc = DateTime.UtcNow
            };

            // Add pre-defined Tactics to plan
            plan.Tactics.AddRange(GetPredefinedTactics(userId));

            // Add pre-defined Strategies to plan
            plan.Strategies.AddRange(GetPredefinedStrategies(userId));

            // Save it
            _businessPlanRepository.AddBusinessPlan(plan);

            // Get it back (with all the new IDs and such)
            return _businessPlanRepository.GetBusinessPlan(userId, year);
        }

        private BusinessPlan SyncPredefinedStrategiesAndTactics(BusinessPlan plan)
        {
            var missingStrategies = GetPredefinedStrategies(plan.UserID).Where(s => plan.Strategies.All(bs => !bs.Name.Equals(s.Name, StringComparison.InvariantCultureIgnoreCase))).ToList();
            var missingTactics = GetPredefinedTactics(plan.UserID).Where(t => plan.Tactics.All(bs => !bs.Name.Equals(t.Name, StringComparison.InvariantCultureIgnoreCase))).ToList();

            if (missingStrategies.Any() || missingTactics.Any())
            {
                foreach (var strategy in missingStrategies)
                {
                    strategy.BusinessPlanID = plan.BusinessPlanID;

                    _businessPlanRepository.AddStrategy(strategy);
                }

                foreach (var tactic in missingTactics)
                {
                    tactic.BusinessPlanID = plan.BusinessPlanID;

                    _businessPlanRepository.AddTactic(tactic);
                }

                plan = _businessPlanRepository.GetBusinessPlanById(plan.BusinessPlanID);
            }

            return plan;
        }

        private IEnumerable<Tactic> GetPredefinedTactics(int userId)
        {
            return _businessPlanRepository.GetPreDefinedEntity<Tactic>().Select(t => new Tactic()
            {
                Name = t.Name,
                Description = t.Description,
                TacticID = 0,
                BusinessPlanID = t.BusinessPlanID,
                Editable = false,
                CreateUserID = userId,
                CreateDate = DateTime.Now,
                CreateDateUtc = DateTime.UtcNow,
                ModifyUserID = userId,
                ModifyDate = DateTime.Now,
                ModifyDateUtc = DateTime.UtcNow
            });
        }

        private IEnumerable<Strategy> GetPredefinedStrategies(int userId)
        {
            return _businessPlanRepository.GetPreDefinedEntity<Strategy>().Select(s => new Strategy()
            {
                Name = s.Name,
                Description = s.Description,
                StrategyID = 0,
                BusinessPlanID = s.BusinessPlanID,
                Editable = false,
                CreateUserID = userId,
                CreateDate = DateTime.Now,
                CreateDateUtc = DateTime.UtcNow,
                ModifyUserID = userId,
                ModifyDate = DateTime.Now,
                ModifyDateUtc = DateTime.UtcNow
            });
        }

        private void PopulateObjectiveProgress(BusinessPlan plan)
        {
            if (plan.Objectives.Any())
            {
                var user = _userService.GetUserByUserId(plan.UserID);

                foreach (var objective in plan.Objectives)
                {
                    var value = user.GetMappedObjectiveProperty(objective.Name);

                    if (value != null)
                    {
                        objective.CurrentValue = value.ToString();
                    }

                    if (user.MetricsUpdateDate != DateTime.MinValue)
                    {
                        objective.CurrentValueDate = user.MetricsUpdateDate;
                    }
                }
            }
        }

        #endregion
    }
}