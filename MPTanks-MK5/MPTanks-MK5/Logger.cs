using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5
{
    //TODO
    public static class Logger
    {
        private static NLog.Logger logger;
        static Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var fileTarget =
                new NLog.Targets.Wrappers.AsyncTargetWrapper(
                    new NLog.Targets.FileTarget() { FileName = ClientSettings.LogLocation },
                    10000, NLog.Targets.Wrappers.AsyncTargetWrapperOverflowAction.Grow);

            config.AddTarget("logfile", fileTarget);
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, fileTarget));

            NLog.LogManager.Configuration = config;

            logger = NLog.LogManager.GetLogger("Client");
        }
        public static void Log(string info)
        {
            logger.Info(info);
        }

        public static void Error(string err)
        {
            logger.Error(err);
            logger.Trace(GetStackTrace());
        }

        public static void Warning(string warn)
        {
            logger.Warn(warn);
        }

        public static void Fatal(string fatal)
        {
            logger.Fatal(fatal);
            logger.Trace(GetStackTrace());
        }


        private static string GetStackTrace()
        {
            return Environment.StackTrace;
        }
    }
}
