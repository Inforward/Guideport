using System;
using Castle.DynamicProxy;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Portal.Services.Clients.ServiceModel
{
    internal sealed class WcfInterceptor<T> : IInterceptor where T : IClientChannel
    {
        private readonly ChannelFactory<T> _factory = null;

        public WcfInterceptor()
        {
            _factory = new ChannelFactory<T>("*");
        }

        public WcfInterceptor(string endpointConfigName)
        {
            _factory = new ChannelFactory<T>(endpointConfigName);
        }

        public WcfInterceptor(Binding binding, EndpointAddress address)
        {
            _factory = new ChannelFactory<T>(binding, address);
        }

        public void Intercept(IInvocation invocation)
        {
            T channel = _factory.CreateChannel();

            try
            {
                invocation.ReturnValue = invocation.Method.Invoke(channel, invocation.Arguments);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Service Call Error.  ServiceUri: {0}, Method: {1}.",
                                                    channel.RemoteAddress.Uri.AbsoluteUri,
                                                    invocation.Method.Name), ex);
            }
            finally
            {
                CloseChannel(channel);
            }
        }

        private void CloseChannel(T channel)
        {
            if (channel == null) return;

            try
            {
                if ((channel.State != CommunicationState.Faulted) && (channel.State != CommunicationState.Closed))
                {
                    channel.Close();
                }
                else
                {
                    channel.Abort();
                }
            }
            catch
            {
                channel.Abort();
            }
        }
    }
}
