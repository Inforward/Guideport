using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class Employee : Auditable
    {
        public Employee()
        {
            EmployeeRoles = new List<EmployeeRole>();
            Employees = new List<Employee>();
        }

        public int EmployeeID { get; set; }
        public int? EmployeeParentID { get; set; }

        [ForeignKey("BusinessPlan")]
        public int BusinessPlanID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public virtual Employee ParentEmployee { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public virtual List<EmployeeRole> EmployeeRoles { get; set; }

        [IgnoreDataMember]
        public BusinessPlan BusinessPlan { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                var name = string.Empty;

                if (!string.IsNullOrWhiteSpace(FirstName))
                    name = FirstName;

                if (!string.IsNullOrWhiteSpace(MiddleInitial))
                {
                    if (!string.IsNullOrEmpty(name))
                        name += " ";

                    name += MiddleInitial;
                }

                if (!string.IsNullOrWhiteSpace(LastName))
                {
                    if (!string.IsNullOrEmpty(name))
                        name += " ";

                    name += LastName;
                }

                return name;
            }
        }

        [NotMapped]
        public bool Selected { get; set; }

        [NotMapped]
        public string Roles
        {
            get { return string.Join(", ", EmployeeRoles); }
        }

        public override bool Equals(object obj)
        {
            if (obj is Employee)
            {
                return (obj as Employee).EmployeeID == EmployeeID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return EmployeeID;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
