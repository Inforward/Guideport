namespace Portal.Model.Rules
{
    public class RulesetRequest
    {
        public string Name { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public RulesetRequestEntityType EntityType { get; set; }
        public Survey Survey { get; set; }

        public object GetEntity()
        {
            switch (EntityType)
            {
                case RulesetRequestEntityType.Survey:
                    return Survey;
                default:
                    return null;
            }
        }

    }

    public enum RulesetRequestEntityType
    {
        Survey
    }

}
