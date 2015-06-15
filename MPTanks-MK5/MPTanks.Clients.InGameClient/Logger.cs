using MPTanks.Engine.Settings;
using Newtonsoft.Json;
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

        public static NLog.Logger Instance { get { return logger; } }

        static Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var fileTarget =
                new NLog.Targets.Wrappers.AsyncTargetWrapper(
                    new NLog.Targets.FileTarget()
                    {
                        FileName = (string)GameSettings.Instance.GameLogLocation,
                        ArchiveOldFileOnStartup = true,
                        KeepFileOpen = true,
                        MaxArchiveFiles = 10,
                        EnableFileDelete = true,
                        CreateDirs = true
                    },
                    10000, NLog.Targets.Wrappers.AsyncTargetWrapperOverflowAction.Grow);

            config.AddTarget("logfile", fileTarget);

                config.LoggingRules.Add(new NLog.Config.LoggingRule("*", 
                    NLog.LogLevel.FromString(GlobalSettings.Instance.LogLevel), fileTarget));

            NLog.LogManager.Configuration = config;

            logger = NLog.LogManager.GetLogger("Client");
        }
        public static void Debug(string message)
        {
            Instance.Debug(message);
        }

        public static void Error(Exception ex)
        {
            Instance.ErrorException("Severe Error/Exception", ex);
            if (GlobalSettings.Debug)
                throw ex;
        }

        public static void Error(string message)
        {
            Error(message);
            if (GlobalSettings.Debug)
                throw new Exception(message);
        }

        public static void Error(string message, Exception ex)
        {
            Instance.ErrorException(message, ex);
            if (GlobalSettings.Debug)
                throw ex;
        }

        public static void Fatal(Exception ex)
        {
            Instance.FatalException("Fatal Exception", ex);
            throw ex;
        }

        public static void Fatal(string message)
        {
            Instance.Fatal(message);
        }

        public static void Fatal(string message, Exception ex)
        {
            Instance.FatalException(message, ex);
            if (GlobalSettings.Debug)
                throw ex;
        }

        public static void Info(object data)
        {
            Info("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void Info(string message)
        {
            Instance.Info(message);
        }

        public static void Trace(Exception ex)
        {
            Instance.TraceException("Code Trace", ex);
        }

        public static void Trace(string message, Exception ex)
        {
            Instance.TraceException(message, ex);
        }

        public static void Trace(object data)
        {
            Trace("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void Trace(string message)
        {
            Instance.Trace(message);
        }

        public static void Warning(string message)
        {
            Instance.Warn(message);
        }

        public static void Warning(object data)
        {
            Warning("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
