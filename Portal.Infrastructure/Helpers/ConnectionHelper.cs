using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using DataHelper;

namespace Portal.Infrastructure.Helpers
{
    public class ConnectionHelper
    {
        private static Dictionary<string, ConnectionManager> _cm;
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Singleton dictionary pattern for database connection manager objects
        /// Because of connection pooling, you shouldn't need more than 1 connection manager
        /// object serving up any particular set of database strings. This helper method
        /// creates and caches the object in the application context.
        /// </summary>
        /// <param name="name">Connection Name as defined in the web.config connectionStrings section</param>
        /// <returns>ConnectionManager object</returns>
        public static ConnectionManager GetConnection(string name)
        {
            try
            {
                _lock.EnterUpgradeableReadLock();

                if (_cm == null)
                    _cm = new Dictionary<string, ConnectionManager>();

                if (_cm.ContainsKey(name))
                    return _cm[name];

                else
                {
                    if (ConfigurationManager.ConnectionStrings[name] == null)
                    {
                        if (_cm.ContainsKey("default"))
                            return _cm["default"];
                        else
                        {
                            ConnectionManager c = SetupConnection("default");

                            try
                            {
                                _lock.EnterWriteLock();
                                _cm.Add("default", c);
                            }
                            finally
                            {
                                _lock.ExitWriteLock();
                            }

                            return c;
                        }
                    }
                    else
                    {
                        ConnectionManager c = SetupConnection(name);

                        try
                        {
                            _lock.EnterWriteLock();
                            _cm.Add(name, c);
                        }
                        finally
                        {
                            _lock.ExitWriteLock();
                        }
                        return c;
                    }
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public static ConnectionManager SetupConnection(string name)
        {
            var connectionName = name;

            if (ConfigurationManager.ConnectionStrings[name] == null)
                connectionName = "default";

            var settings = ConfigurationManager.ConnectionStrings[connectionName];

            return new ConnectionManager()
            {
                ConfigurationPath = settings.ConnectionString,
                ConfigurationFile = settings.ProviderName
            };
        }
    }
}
