using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FastEngSite.Controllers
{
    public class Logger
    {
        private static NLog.Logger _log;

        static Logger()
        {
            try
            {
                string path = Path.Combine(PathUtility.App_Data, "Log.log");

                var config = new LoggingConfiguration();

                var target =
                    new FileTarget
                    {
                        FileName = path
                    };
                config.AddTarget("logfile", target);

                var rule = new LoggingRule("*", LogLevel.Debug, target);
                config.LoggingRules.Add(rule);

                LogManager.Configuration = config;

                _log = LogManager.GetCurrentClassLogger();
            }
            catch { }
        }

        public static void Error(Exception ex, bool showUser = true)
        {
            if (ex == null)
            {
                return;
            }
            string exStr = ex.ToString();

            Console.WriteLine(exStr);
            _log.Error(String.Format("Version({0})", PathUtility.AssemblyFileVersion));
            _log.Error(ex);
        }
        
        public static void Error(string message, bool showUser = true)
        {
            if (message == null)
            {
                return;
            }
            Console.WriteLine(message);
            if (_log != null)
            {
                _log.Error(String.Format("Version({0}) {1}", PathUtility.AssemblyFileVersion, message));
            }
        }

        public static void Info(string message, bool showUser = true)
        {
            if (message == null)
            {
                return;
            }
            Console.WriteLine(message);
            if (_log != null)
            {
                _log.Info(message);
            }
        }


       
    }
}