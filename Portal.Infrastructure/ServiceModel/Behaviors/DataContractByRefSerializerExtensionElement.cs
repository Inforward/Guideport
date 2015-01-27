using System;
using System.ServiceModel.Configuration;

namespace Portal.Infrastructure.ServiceModel
{
    public class DataContractByRefSerializerExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(DataContractByRefSerializerBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new DataContractByRefSerializerBehavior();
        }
    }
}
