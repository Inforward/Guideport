using Castle.DynamicProxy;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Portal.Services.Clients.ServiceModel
{
    public class ServiceClient<T> where T : class, IClientChannel
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        public T CreateProxy()
        {
            return _generator.CreateInterfaceProxyWithoutTarget<T>(new WcfInterceptor<T>());
        }

        public T CreateProxy(string endpointConfigName)
        {
            return _generator.CreateInterfaceProxyWithoutTarget<T>(new WcfInterceptor<T>(endpointConfigName));
        }

        public T CreateProxy(Binding binding, EndpointAddress address)
        {
            return _generator.CreateInterfaceProxyWithoutTarget<T>(new WcfInterceptor<T>(binding, address));
        }
    }
}
