using Portal.Model.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class EmployeeRole : Auditable, IBusinessPlanEntity
    {
        public EmployeeRole()
        {
            Employees = new List<Employee>();
        }

        public int EmployeeRoleID { get; set; }

        [ForeignKey("BusinessPlan")]
        public int? BusinessPlanID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? SortOrder { get; set; }
        
        public virtual List<Employee> Employees { get; set; }

        [IgnoreDataMember]
        public BusinessPlan BusinessPlan { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is EmployeeRole)
            {
                return (obj as EmployeeRole).EmployeeRoleID == EmployeeRoleID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return EmployeeRoleID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
