using System;

namespace Portal.Model.Rules
{
    public partial class Ruleset
    {
        public string Name { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string RuleSetDefinition { get; set; }
        public short? Status { get; set; }
        public string AssemblyPath { get; set; }
        public string ActivityName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
