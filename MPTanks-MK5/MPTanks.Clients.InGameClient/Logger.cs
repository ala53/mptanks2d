using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient
{
    public static class Logger
    {
        private static NLog.Logger logger;
        static Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var fileTarget =
                new NLog.Targets.Wrappers.AsyncTargetWrapper(
                    new NLog.Targets.FileTarget()
                    {
                        FileName = (string)ClientSettings.Instance.LogLocation,
                        ArchiveOldFileOnStartup = true,
                        KeepFileOpen = true,
                        MaxArchiveFiles = 10,
                        EnableFileDelete = true,
                        CreateDirs = true
                    },
                    10000, NLog.Targets.Wrappers.AsyncTargetWrapperOverflowAction.Grow);

            config.AddTarget("logfile", fileTarget);
#if DEBUG
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, fileTarget));
#else
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Info, fileTarget));
#endif

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

        public static void Debug(string dbg)
        {
#if DEBUG //Don't remove, compiler optimization. If we're not in debug mode, we don't need this info
            //so we have it as dead code which the compiler (hopefully) removes
            logger.Debug(dbg);
#endif
        }


        private static string GetStackTrace()
        {
            return Environment.StackTrace;
        }
    }
}
