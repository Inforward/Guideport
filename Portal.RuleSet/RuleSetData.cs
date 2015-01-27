using System;
using System.Globalization;
using System.IO;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;

namespace Portal.RuleSet
{
    public class RuleSetData : IComparable<RuleSetData>
    {
        #region Variables and constructor

        private string name;
        private int majorVersion;
        private int minorVersion;
        private string ruleSetDefinition;
        private System.Workflow.Activities.Rules.RuleSet ruleSet;
        private string activityName;
        private DateTime modifiedDate;
        private Type activity;
        private readonly WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (RuleSet != null) RuleSet.Name = name;
            }
        }

        public string OriginalName { get; set; }

        public int MajorVersion
        {
            get { return majorVersion; }
            set { majorVersion = value; }
        }

        public int OriginalMajorVersion { get; set; }

        public int MinorVersion
        {
            get { return minorVersion; }
            set { minorVersion = value; }
        }

        public int OriginalMinorVersion { get; set; }

        public string RuleSetDefinition
        {
            get { return ruleSetDefinition; }
            set { ruleSetDefinition = value; }
        }

        public System.Workflow.Activities.Rules.RuleSet RuleSet
        {
            get
            {
                return ruleSet ?? (ruleSet = DeserializeRuleSet(ruleSetDefinition));
            }
            set
            {
                ruleSet = value;
                name = ruleSet.Name;
            }
        }

        public short Status { get; set; }

        public string AssemblyPath { get; set; }

        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        public DateTime ModifiedDate
        {
            get { return modifiedDate; }
            set { modifiedDate = value; }
        }

        public string ModifiedBy { get; set; }

        public bool Dirty { get; set; }

        public Type Activity
        {
            get { return activity; }
            set
            {
                activity = value;
                if (activity != null)
                    activityName = activity.ToString();
            }
        }

        #endregion

        #region Methods

        private System.Workflow.Activities.Rules.RuleSet DeserializeRuleSet(string ruleSetXmlDefinition)
        {
            if (!String.IsNullOrEmpty(ruleSetXmlDefinition))
            {
                var stringReader = new StringReader(ruleSetXmlDefinition);
                var reader = new XmlTextReader(stringReader);
                return serializer.Deserialize(reader) as System.Workflow.Activities.Rules.RuleSet;
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1}.{2}", name, majorVersion, minorVersion);
        }

        public RuleSetData Clone()
        {
            var newData = new RuleSetData
            {
                Activity = Activity,
                AssemblyPath = AssemblyPath,
                Dirty = true,
                MajorVersion = MajorVersion,
                MinorVersion = MinorVersion,
                Name = name,
                RuleSet = RuleSet.Clone(),
                Status = 0
            };
            //newData.ActivityName = activityName; //Set by setting Activity

            return newData;
        }

        public RuleSetInfo GetRuleSetInfo()
        {
            return new RuleSetInfo(name, majorVersion, minorVersion);
        }

        #endregion

        #region IComparable<RuleSetData> Members

        public int CompareTo(RuleSetData other)
        {
            if (other != null)
            {
                var nameComparison = String.CompareOrdinal(Name, other.Name);
                if (nameComparison != 0)
                    return nameComparison;

                var majorVersionComparison = MajorVersion - other.MajorVersion;
                if (majorVersionComparison != 0)
                    return majorVersionComparison;

                var minorVersionComparison = MinorVersion - other.MinorVersion;
                if (minorVersionComparison != 0)
                    return minorVersionComparison;

                return 0;
            }

            return 1;
        }

        #endregion
    }
}
