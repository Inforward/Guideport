using System.Collections.Generic;
using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;

namespace Portal.Services.Clients
{
    public class BusinessPlanServiceClient : IBusinessPlanService
    {
        private readonly ServiceClient<IBusinessPlanServiceChannel> _businessPlanService = new ServiceClient<IBusinessPlanServiceChannel>();

        public IEnumerable<BusinessPlan> GetBusinessPlans(BusinessPlanRequest criteria)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetBusinessPlans(criteria);
        }

        public BusinessPlan GetBusinessPlan(int userId, int year)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetBusinessPlan(userId, year);
        }

        public BusinessPlan GetLatestBusinessPlan(int userId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetLatestBusinessPlan(userId);
        }

        public BusinessPlan CopyBusinessPlan(int year, int copyFromYear, int userId, int auditUserId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.CopyBusinessPlan(year, copyFromYear, userId, auditUserId);
        }

        public void UpdateBusinessPlan(ref BusinessPlan businessPlan)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateBusinessPlan(ref businessPlan);
        }

        public void DeleteBusinessPlan(int businessPlanId, int userId, int deleteUserId)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.DeleteBusinessPlan(businessPlanId, userId, deleteUserId);
        }

        public IEnumerable<int> GetBusinessPlanYears(int? userId = null)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetBusinessPlanYears(userId);
        }

        public IEnumerable<Swot> GetSwots(int businessPlanId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetSwots(businessPlanId);
        }

        public void CreateSwot(ref Swot swot)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.CreateSwot(ref swot);
        }

        public void UpdateSwot(ref Swot swot)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateSwot(ref swot);
        }

        public void DeleteSwot(Swot swot)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.DeleteSwot(swot);
        }

        public IEnumerable<Tactic> GetTactics(int businessPlanId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetTactics(businessPlanId);
        }

        public void CreateTactic(ref Tactic tactic)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.CreateTactic(ref tactic);
        }

        public void UpdateTactic(ref Tactic tactic)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateTactic(ref tactic);
        }

        public void UpdateTacticStatus(int tacticId, bool isComplete, int userId, int modifyUserId)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateTacticStatus(tacticId, isComplete, userId, modifyUserId);
        }

        public void DeleteTactic(Tactic tactic)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.DeleteTactic(tactic);
        }

        public IEnumerable<Strategy> GetStrategies(int businessPlanId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetStrategies(businessPlanId);
        }

        public void CreateStrategy(ref Strategy strategy)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.CreateStrategy(ref strategy);
        }

        public void UpdateStrategy(ref Strategy strategy)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateStrategy(ref strategy);
        }

        public void DeleteStrategy(Strategy strategy)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.DeleteStrategy(strategy);
        }

        public IEnumerable<Employee> GetEmployees(int businessPlanId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetEmployees(businessPlanId);
        }

        public void CreateEmployee(ref Employee employee)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.CreateEmployee(ref employee);
        }

        public void UpdateEmployee(ref Employee employee)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateEmployee(ref employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.DeleteEmployee(employee);
        }

        public IEnumerable<Objective> GetObjectives(int businessPlanId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetObjectives(businessPlanId);
        }

        public IEnumerable<Objective> GetPreDefinedObjectives()
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetPreDefinedObjectives();
        }

        public void CreateObjective(ref Objective objective, int userId)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.CreateObjective(ref objective, userId);
        }

        public void UpdateObjective(ref Objective objective)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateObjective(ref objective);
        }

        public void UpdateObjectiveStatus(int objectiveId, int percentComplete, int userId, int modifyUserId)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateObjectiveStatus(objectiveId, percentComplete, userId, modifyUserId);
        }

        public void DeleteObjective(Objective objective)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.DeleteObjective(objective);
        }

        public IEnumerable<EmployeeRole> GetEmployeeRoles(int businessPlanId)
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetEmployeeRoles(businessPlanId);
        }

        public IEnumerable<EmployeeRole> GetPreDefinedEmployeeRoles()
        {
            var proxy = _businessPlanService.CreateProxy();
            return proxy.GetPreDefinedEmployeeRoles();
        }

        public void CreateEmployeeRole(ref EmployeeRole employeeRole)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.CreateEmployeeRole(ref employeeRole);
        }

        public void UpdateEmployeeRole(ref EmployeeRole employeeRole)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.UpdateEmployeeRole(ref employeeRole);
        }

        public void DeleteEmployeeRole(EmployeeRole employeeRole)
        {
            var proxy = _businessPlanService.CreateProxy();
            proxy.DeleteEmployeeRole(employeeRole);
        }
    }
}