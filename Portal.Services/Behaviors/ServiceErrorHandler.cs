using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Infrastructure.ServiceModel;
using Portal.Model;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;

namespace Portal.Services
{
    public class ServiceErrorHandler : IErrorHandler
    {
        private string _url;
        private string _operationName;

        private static readonly List<Type> IgnoreTypes = new List<Type>()
        {
            typeof(Task),
            typeof(TaskFactory),
            typeof(ServiceErrorHandler),
            typeof(LoggerBehavior),
            typeof(LoggerBehaviorExtensionElement),
            typeof(DataContractByRefSerializerBehavior)
        };

        public bool HandleError(Exception error)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var logService = Global.Resolve<ILogService>();

                    logService.LogEvent(new Model.EventLog()
                    {
                        EventTypeID = (byte)EventTypes.Error,
                        EventDate = DateTime.Now,
                        Message = error.FullMessage(),
                        ErrorText = error.Message,
                        ServerName = Environment.MachineName,
                        StackTrace = error.StackTrace,
                        Source = Source,
                        ScriptName = _url,
                        RequestMethod = _operationName
                    });
                }
                catch (Exception ex)
                {
                    LogToEventLog(error.FullMessage() + Environment.NewLine + error.StackTrace);
                    LogToEventLog(ex);
                }
            });

            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            var action = OperationContext.Current.IncomingMessageHeaders.Action;
            _operationName = action.Substring(action.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
            _url = OperationContext.Current.IncomingMessageHeaders.To.AbsoluteUri;
        }

        private static string Source
        {
            get
            {
                var f = SourceStackFrame;
                var method = f.GetMethod();

                return string.Format("Type:{0}.  Method:{1}", method.ReflectedType, method);
            }
        }

        private static StackFrame SourceStackFrame
        {
            get
            {
                var retVal = new StackFrame();
                var st = new StackTrace(true);

                foreach (var frame in st.GetFrames())
                {
                    var method = frame.GetMethod();
                    var fullMethodName = method.DeclaringType.FullName + "." + method.Name;

                    if (!IgnoreTypes.Contains(method.DeclaringType)
                        && !method.GetCustomAttributes(typeof(CompilerGeneratedAttribute)).Any()) // Ignore anonymous methods
                    {
                        retVal = frame;
                        break;
                    }
                }

                return retVal;
            }
        }

        private void LogToEventLog(Exception ex)
        {
            LogToEventLog(ex.FullMessage());
        }

        private void LogToEventLog(string message)
        {
            try
            {
                const string source = "Portal.Services";
                const string log = "Application";

                if (!System.Diagnostics.EventLog.SourceExists(source))
                    System.Diagnostics.EventLog.CreateEventSource(source, log);

                System.Diagnostics.EventLog.WriteEntry(source, message, EventLogEntryType.Error);
            }
            catch { }
        }
    }
}
