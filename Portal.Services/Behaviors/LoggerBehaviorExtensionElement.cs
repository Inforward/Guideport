using Portal.Infrastructure.Logging;
using System;
using System.ServiceModel.Configuration;

namespace Portal.Services
{
    public class LoggerBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(LoggerBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new LoggerBehavior();
        }
    }
}
