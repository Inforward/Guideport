using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Model.Interfaces;
using RefactorThis.GraphDiff;

namespace Portal.Data.Sql.EntityFramework
{
    public class BusinessPlanRepository : EntityRepository<MasterContext>, IBusinessPlanRepository
    {
        public BusinessPlan GetBusinessPlan(int userId, int year)
        {
            return FindBy<BusinessPlan>(b => b.UserID == userId && b.Year == year && b.DeleteDate == null)
                    .AsNoTracking()
                    .IncludeAll()
                    .FirstOrDefault();
        }

        public BusinessPlan GetBusinessPlanById(int id)
        {
            return FindBy<BusinessPlan>(b => b.BusinessPlanID == id)
                    .AsNoTracking()
                    .IncludeAll()
                    .FirstOrDefault();
        }

        public IQueryable<BusinessPlan> GetBusinessPlans(int userId)
        {
            return FindBy<BusinessPlan>(b => b.UserID == userId && b.DeleteDate == null)
                    .IncludeAll()
                    .OrderByDescending(b => b.Year);
        }

        public IEnumerable<T> GetPreDefinedEntity<T>() where T : class, IBusinessPlanEntity
        {
            return FindBy<T>(r => r.BusinessPlanID == null).OrderBy(r => r.SortOrder).ThenBy(r => r.Name).ToList();
        }

        public void AddBusinessPlan(BusinessPlan businessPlan)
        {
            var saveBusinessPlan = Context.UpdateGraph(businessPlan);
            Save();

            foreach (var tactic in businessPlan.Tactics)
            {
                tactic.BusinessPlanID = saveBusinessPlan.BusinessPlanID;
                Add(tactic);
            }

            foreach (var strategy in businessPlan.Strategies)
            {
                strategy.BusinessPlanID = saveBusinessPlan.BusinessPlanID;
                Add(strategy);
            }
 
            Save();
        }

        public void UpdateBusinessPlan(BusinessPlan businessPlan)
        {
            Update(businessPlan);
            Save();
        }

        #region Objectives

        public void AddObjective(Objective objective)
        {
            // Update Strategy / Tactic relationship
            foreach (var strategy in objective.Strategies)
            {
                var existingStrategy = FindBy<Strategy>(s => s.StrategyID == strategy.StrategyID).Include("Tactics").FirstOrDefault();
                var oldTactics = existingStrategy != null ? existingStrategy.Tactics.Select(t => t.TacticID).ToList() : null;

                Audit<Strategy>("Tactics", AuditTypes.Insert, null, strategy.StrategyID, objective.ModifyUserID,
                    new { Tactics = oldTactics },
                    new { Tactics = strategy.Tactics.Select(t => t.TacticID) },
                    () =>
                    {
                        Context.UpdateGraph(strategy, s => s.AssociatedCollection(t => t.Tactics));
                        return Context.SaveChanges();
                    });
            }

            // Update objective and it's strategy relationship
            var existingObjective = FindBy<Objective>(o => o.ObjectiveID == objective.ObjectiveID).Include("Strategies").FirstOrDefault();
            var oldStrategies = existingObjective != null ? existingObjective.Strategies.Select(s => s.StrategyID).ToList() : null;

            var auditType = objective.ObjectiveID > 0 ? AuditTypes.Insert : AuditTypes.Update;
            Audit<Objective>("Strategies", auditType, null, objective.ObjectiveID, objective.ModifyUserID, 
                new { Strategies = oldStrategies },
                new { Strategies = objective.Strategies.Select(s => s.StrategyID) },
                () =>
                {
                    var saveObjective = Context.UpdateGraph(objective, er => er.AssociatedCollection(o => o.Strategies));
                    Save();

                    return saveObjective.ObjectiveID;
                });
        }

        public void UpdateObjective(Objective objective)
        {
            AddObjective(objective);
        }

        public void DeleteObjective(Objective objective)
        {
            var objectiveToDelete = FindBy<Objective>(o => o.ObjectiveID == objective.ObjectiveID).FirstOrDefault();

            if (objectiveToDelete == null)
                return;

            Delete(objectiveToDelete);
            Save();
        }

        #endregion

        #region Strategies

        public void AddStrategy(Strategy strategy)
        {
            Add(strategy);
            Save();
        }

        public void UpdateStrategy(Strategy strategy)
        {
            Context.UpdateGraph(strategy);
            Save();
        }

        public void DeleteStrategy(Strategy strategy)
        {
            var strategyToDelete = FindBy<Strategy>(s => s.StrategyID == strategy.StrategyID).FirstOrDefault();

            if (strategyToDelete == null)
                return;

            // Remove this strategy from any associated objectives
            var objectives = FindBy<Objective>(o => o.Strategies.Any(os => os.StrategyID == strategy.StrategyID))
                                   .AsNoTracking()
                                   .Include("Strategies")
                                   .ToList();

            foreach (var objective in objectives)
            {
                objective.Strategies.Remove(strategyToDelete);
                Context.UpdateGraph(objective, o => o.AssociatedCollection(s => s.Strategies));
            }

            // Delete it.  This will cascade delete any tactic associations
            Delete(strategyToDelete);
            Save();
        }

        #endregion

        #region Tactics

        public void AddTactic(Tactic tactic)
        {
            Add(tactic);
            Save();
        }

        public void UpdateTactic(Tactic tactic)
        {
            Context.UpdateGraph(tactic);
            Save();
        }

        public void DeleteTactic(Tactic tactic)
        {
            var tacticToDelete = FindBy<Tactic>(t => t.TacticID == tactic.TacticID).FirstOrDefault();

            if (tacticToDelete == null)
                return;

            // Remove this tactic from any associated strategies
            var strategies = FindBy<Strategy>(s => s.Tactics.Any(t => t.TacticID == tactic.TacticID))
                                   .AsNoTracking()
                                   .Include("Tactics")
                                   .ToList();

            foreach (var strategy in strategies)
            {
                strategy.Tactics.Remove(tacticToDelete);
                Context.UpdateGraph(strategy, s => s.AssociatedCollection(t => t.Tactics));
            }

            Delete(tacticToDelete);
            Save();
        }

        #endregion

        #region Employees

        public void AddEmployee(Employee employee)
        {
            Add(employee);
            Save();
        }

        public void UpdateEmployee(Employee employee)
        {
            Context.UpdateGraph(employee);
            Save();
        }

        public void DeleteEmployee(Employee employee)
        {
            var employeeToDelete = FindBy<Employee>(e => e.EmployeeID == employee.EmployeeID).FirstOrDefault();

            if (employeeToDelete == null)
                return;

            // If this employee has any children, make them orphans
            var childEmployees = FindBy<Employee>(e => e.EmployeeParentID == employeeToDelete.EmployeeID).ToList();

            foreach (var child in childEmployees)
            {
                child.EmployeeParentID = null;
                Update(child);
            }

            // Delete the employee.  This will cascade delete any roles the employee is a member of
            Delete(employeeToDelete);
            Save();
        }

        #endregion

        #region Employee Roles

        public void AddEmployeeRole(EmployeeRole employeeRole)
        {
            var saveEmployeeRole = Context.UpdateGraph(employeeRole);
            Save();

            Audit<EmployeeRole>("Employees", AuditTypes.Insert, null, saveEmployeeRole.EmployeeRoleID, saveEmployeeRole.ModifyUserID, null,
                new { Employees = employeeRole.Employees.Select(e => e.EmployeeID) }, 
                () =>
                {
                    // Add role
                    foreach (var employee in employeeRole.Employees.Where(employee => employeeRole.Employees.Exists(e => e.EmployeeID == employee.EmployeeID)))
                    {
                        employee.EmployeeRoles.Add(saveEmployeeRole);
                        Context.UpdateGraph(employee, map => map.AssociatedCollection(e => e.EmployeeRoles));
                    }

                    return Context.SaveChanges();
                });
        }

        public void UpdateEmployeeRole(EmployeeRole employeeRole)
        {
            var existingRole = FindBy<EmployeeRole>(er => er.EmployeeRoleID == employeeRole.EmployeeRoleID).Include("Employees").FirstOrDefault();

            if (existingRole == null)
                throw new ArgumentException("Invalid Employee Role");

            // Update role itself
            existingRole.Name = employeeRole.Name;
            existingRole.Description = employeeRole.Description;

            Update(existingRole);
            Save();

            // Save Employee associations
            Audit<EmployeeRole>("Employees", AuditTypes.Update, null, employeeRole.EmployeeRoleID, employeeRole.ModifyUserID,
                new { Employees = existingRole.Employees.Select(e => e.EmployeeID) },
                new { Employees = employeeRole.Employees.Select(e => e.EmployeeID) },
                () =>
                {
                    var recordsAffected = 0;
                    var employeesToRemoveFrom = FindBy<Employee>(e => e.BusinessPlanID == employeeRole.BusinessPlanID)
                                                        .Where(e => e.EmployeeRoles.Any(er => er.EmployeeRoleID == employeeRole.EmployeeRoleID))
                                                        .Include("EmployeeRoles")
                                                        .ToList();

                    // Remove role 
                    foreach (var employee in employeesToRemoveFrom.Where(employee => !employeeRole.Employees.Contains(employee)))
                    {
                        employee.EmployeeRoles.Remove(employeeRole);
                        Update(employee);
                    }

                    recordsAffected += Context.SaveChanges();

                    var employeesToAddTo = FindBy<Employee>(e => e.BusinessPlanID == employeeRole.BusinessPlanID)
                                                        .Where(e => e.EmployeeRoles.All(er => er.EmployeeRoleID != employeeRole.EmployeeRoleID))
                                                        .Include("EmployeeRoles")
                                                        .ToList();

                    // Add role
                    foreach (var employee in employeesToAddTo.Where(employee => employeeRole.Employees.Exists(e => e.EmployeeID == employee.EmployeeID)))
                    {
                        employee.EmployeeRoles.Add(existingRole);
                        Update(employee);
                    }

                    recordsAffected += Context.SaveChanges();

                    return recordsAffected;
                });
        }

        public void DeleteEmployeeRole(EmployeeRole employeeRole)
        {
            var roleToDelete = FindBy<EmployeeRole>(er => er.EmployeeRoleID == employeeRole.EmployeeRoleID).FirstOrDefault();

            if (roleToDelete == null)
                return;

            var employees = FindBy<Employee>(e => e.EmployeeRoles.Any(er => er.EmployeeRoleID == employeeRole.EmployeeRoleID))
                                   .AsNoTracking()
                                   .Include("EmployeeRoles")
                                   .ToList();

            foreach (var employee in employees)
            {
                employee.EmployeeRoles.Remove(roleToDelete);
                Context.UpdateGraph(employee, e => e.AssociatedCollection(er => er.EmployeeRoles));
            }

            Delete(roleToDelete);

            Save();
        }

        #endregion

        #region Swots

        public void AddSwot(Swot swot)
        {
            Add(swot);
            Save();
        }

        public void UpdateSwot(Swot swot)
        {
            Context.UpdateGraph(swot);
            Save();
        }

        public void DeleteSwot(Swot swot)
        {
            var swotToDelete = FindBy<Swot>(s => s.SwotID == swot.SwotID).FirstOrDefault();

            if (swotToDelete == null)
                return;

            Delete(swotToDelete);
            Save();
        }

        #endregion
    }

    public static class BusinessPlanExtensions
    {
        public static IQueryable<BusinessPlan> IncludeAll(this IQueryable<BusinessPlan> businessPlan)
        {
            return businessPlan
                    .Include("Employees")
                    .Include("Employees.EmployeeRoles")
                    .Include("EmployeeRoles.Employees")
                    .Include("Swots")
                    .Include("Objectives")
                    .Include("Objectives.Strategies")
                    .Include("Objectives.Strategies.Tactics")
                    .Include("Strategies")
                    .Include("Tactics");
        }
    }
}