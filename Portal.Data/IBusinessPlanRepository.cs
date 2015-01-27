using System.Linq;
using Portal.Model;
using System.Collections.Generic;
using Portal.Model.Interfaces;

namespace Portal.Data
{
    public interface IBusinessPlanRepository : IEntityRepository
    {
        BusinessPlan GetBusinessPlanById(int id);
        BusinessPlan GetBusinessPlan(int userId, int year);
        IQueryable<BusinessPlan> GetBusinessPlans(int userId);

        void AddBusinessPlan(BusinessPlan businessPlan);
        void UpdateBusinessPlan(BusinessPlan businessPlan);

        IEnumerable<T> GetPreDefinedEntity<T>() where T : class, IBusinessPlanEntity;

        void UpdateEmployeeRole(EmployeeRole employeeRole);
        void AddEmployeeRole(EmployeeRole employeeRole);
        void DeleteEmployeeRole(EmployeeRole employeeRole);

        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);

        void AddSwot(Swot swot);
        void UpdateSwot(Swot swot);
        void DeleteSwot(Swot swot);

        void AddObjective(Objective objective);
        void UpdateObjective(Objective objective);
        void DeleteObjective(Objective objective);

        void AddStrategy(Strategy strategy);
        void UpdateStrategy(Strategy strategy);
        void DeleteStrategy(Strategy strategy);

        void AddTactic(Tactic tactic);
        void UpdateTactic(Tactic tactic);
        void DeleteTactic(Tactic tactic);
    }
}
