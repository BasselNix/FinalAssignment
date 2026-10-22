using log4net;
using log4net.Config;
using System.Reflection;

namespace ParaBank.Utilities
{
    public static class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        static Logger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public static void Info(string message) => Log.Info(message);
        public static void Error(string message, Exception? ex = null) => Log.Error(message, ex);
        public static void Debug(string message) => Log.Debug(message);
    }
}