using Sitecore.Diagnostics;
using log4net;
using System;

namespace Helixbase.Foundation.Logging.Repositories
{
    public class LogRepository: ILogRepository
    {
        private static readonly ILog _logger = log4net.LogManager.GetLogger("Sitecore.Diagnostics.Integration");
        private static ILog log;
        public static ILog Log
        {
            get
            {
                return log ?? (log = LogManager.GetLogger("Sitecore.Diagnostics.Integration"));
            }
        }
        public void Debug(string message,Exception t) => Log.Debug(message,t);

        public void Debug(string message, params object[] args) => Log.Debug(string.Format(message, args));

        public void Error(string message, Exception t) => Log.Error(message, t);

 

 

        public void Info(string message,Exception t) => Log.Info(message,t);

        public void Info(string message, params object[] args) => Log.Debug(string.Format(message, args));

        public void Warn(string message, Exception t) => Log.Warn(message, t);

        public void Fatal(string message, Exception t) => Log.Fatal(message, t);

        public void Debug(string message) => Log.Debug(message);


        public void Error(string message) => Log.Error(message);


        public void Info(string message) => Log.Info(message);


        public void Warn(string message) => Log.Warn(message);

    }
}