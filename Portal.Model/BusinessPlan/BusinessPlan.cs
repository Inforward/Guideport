using System;
using System.Collections.Generic;

namespace Portal.Model
{
    public partial class BusinessPlan : Auditable
    {
        public BusinessPlan()
        {
            Swots = new List<Swot>();
            Employees = new List<Employee>();
            EmployeeRoles = new List<EmployeeRole>();
            Objectives = new List<Objective>();
            Strategies = new List<Strategy>();
            Tactics = new List<Tactic>();
        }

        public int BusinessPlanID { get; set; }
        public int UserID { get; set; }
        public int Year { get; set; }
        public string MissionWhat { get; set; }
        public string MissionHow { get; set; }
        public string MissionWhy { get; set; }
        public string VisionOneYear { get; set; }
        public string VisionFiveYear { get; set; }
        public int? DeleteUserID { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? DeleteDateUtc { get; set; }
        public virtual List<Swot> Swots { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public virtual List<EmployeeRole> EmployeeRoles { get; set; }
        public virtual List<Objective> Objectives { get; set; }
        public virtual List<Strategy> Strategies { get; set; }
        public virtual List<Tactic> Tactics { get; set; }

        public static BusinessPlan Default()
        {
            return new BusinessPlan() { Year = DateTime.Now.Year };
        }
    }
}
