using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Portal.Infrastructure.ServiceModel
{
    public class DataContractByRefSerializerBehavior : Attribute, IServiceBehavior, IEndpointBehavior
    {
        #region IServiceBehavior
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var endpoint in serviceDescription.Endpoints)
                RegisterContract(endpoint);
        }
        #endregion

        #region IEndpointBehavior
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            RegisterContract(endpoint);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }
        #endregion

        #region Support
        protected void RegisterContract(ServiceEndpoint endpoint)
        {
            foreach (var desc in endpoint.Contract.Operations)
            {
                var dcsOperationBehavior = desc.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsOperationBehavior != null)
                {
                    var idx = desc.Behaviors.IndexOf(dcsOperationBehavior);
                    desc.Behaviors.Remove(dcsOperationBehavior);
                    desc.Behaviors.Insert(idx, new DataContractByRefSerializerOperationBehavior(desc));
                }
            }
        }
        #endregion
    }
}
