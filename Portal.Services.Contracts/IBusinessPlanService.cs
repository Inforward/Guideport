using System.Collections.Generic;
using Portal.Model;
using System.ServiceModel;

namespace Portal.Services.Contracts
{
    public interface IBusinessPlanServiceChannel : IBusinessPlanService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IBusinessPlanService
    {
        [OperationContract]
        IEnumerable<BusinessPlan> GetBusinessPlans(BusinessPlanRequest criteria);
        [OperationContract]
        BusinessPlan GetBusinessPlan(int userId, int year);
        [OperationContract]
        BusinessPlan GetLatestBusinessPlan(int userId);
        [OperationContract]
        BusinessPlan CopyBusinessPlan(int year, int copyFromYear, int userId, int auditUserId);
        [OperationContract]
        void UpdateBusinessPlan(ref BusinessPlan businessPlan);
        [OperationContract]
        void DeleteBusinessPlan(int businessPlanId, int userId, int deleteUserId);
        [OperationContract]
        IEnumerable<int> GetBusinessPlanYears(int? userId = null);

        [OperationContract]
        IEnumerable<Swot> GetSwots(int businessPlanId);
        [OperationContract]
        void CreateSwot(ref Swot swot);
        [OperationContract]
        void UpdateSwot(ref Swot swot);
        [OperationContract]
        void DeleteSwot(Swot swot);

        [OperationContract]
        IEnumerable<Tactic> GetTactics(int businessPlanId);
        [OperationContract]
        void CreateTactic(ref Tactic tactic);
        [OperationContract]
        void UpdateTactic(ref Tactic tactic);
        [OperationContract]
        void UpdateTacticStatus(int tacticId, bool isComplete, int userId, int auditUserId);
        [OperationContract]
        void DeleteTactic(Tactic tactic);

        [OperationContract]
        IEnumerable<Strategy> GetStrategies(int businessPlanId);
        [OperationContract]
        void CreateStrategy(ref Strategy strategy);
        [OperationContract]
        void UpdateStrategy(ref Strategy strategy);
        [OperationContract]
        void DeleteStrategy(Strategy strategy);

        [OperationContract]
        IEnumerable<Employee> GetEmployees(int businessPlanId);
        [OperationContract]
        void CreateEmployee(ref Employee employee);
        [OperationContract]
        void UpdateEmployee(ref Employee employee);
        [OperationContract]
        void DeleteEmployee(Employee employee);

        [OperationContract]
        IEnumerable<Objective> GetObjectives(int businessPlanId);
        [OperationContract]
        IEnumerable<Objective> GetPreDefinedObjectives();
        [OperationContract]
        void CreateObjective(ref Objective objective, int userId);
        [OperationContract]
        void UpdateObjective(ref Objective objective);
        [OperationContract]
        void UpdateObjectiveStatus(int objectiveId, int percentComplete, int userId, int auditUserId);
        [OperationContract]
        void DeleteObjective(Objective objective);

        [OperationContract]
        IEnumerable<EmployeeRole> GetEmployeeRoles(int businessPlanId);
        [OperationContract]
        IEnumerable<EmployeeRole> GetPreDefinedEmployeeRoles();
        [OperationContract]
        void CreateEmployeeRole(ref EmployeeRole employeeRole);
        [OperationContract]
        void UpdateEmployeeRole(ref EmployeeRole employeeRole);
        [OperationContract]
        void DeleteEmployeeRole(EmployeeRole employeeRole);
    }
}
