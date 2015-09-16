using MPTanks.Engine.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox
{
    public static class Logger
    {
        private static NLog.Logger logger;

        public static NLog.Logger Instance { get { return logger; } }

        static Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var fileTarget = new NLog.Targets.FileTarget()
            {
                FileName = (string)GameSettings.Instance.GameLogLocation,
                ArchiveOldFileOnStartup = true,
                KeepFileOpen = true,
                MaxArchiveFiles = 10,
                EnableFileDelete = true,
                CreateDirs = true,
                Layout = "${date:format:HH:mm:ss}|${level:uppercase=true}|${logger}|${message}|${exception:innerFormat=ToString:maxInnerExceptionLevel=128:innerExceptionSeparator=String:separator = String:format = ToString}"
            };
            config.AddTarget("logfile", fileTarget);

            config.LoggingRules.Add(new NLog.Config.LoggingRule("*",
                NLog.LogLevel.FromString(GlobalSettings.Instance.LogLevel), fileTarget));

            NLog.LogManager.Configuration = config;

            logger = NLog.LogManager.GetLogger("Client");
            Info("Initialized on " + DateTime.Now.ToShortDateString());
        }
        public static void Debug(string message)
        {
            Instance.Debug(message);
        }

        public static void Error(Exception ex)
        {
            Instance.Error(ex, "Severe Error/Exception");
        }

        public static void Error(string message)
        {
            Instance.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            Instance.Error(ex, message);
        }

        public static void Fatal(Exception ex)
        {
            Instance.Fatal(ex, "Fatal Exception");
            throw ex;
        }

        public static void Fatal(string message)
        {
            Instance.Fatal(message);
        }

        public static void Fatal(string message, Exception ex)
        {
            Instance.Fatal(ex, message);
        }

        public static void Info(object data)
        {
            Info("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                }));
        }

        public static void Info(string message)
        {
            Instance.Info(message);
        }

        public static void Trace(Exception ex)
        {
            Instance.Trace(ex, "Code Trace");
        }

        public static void Trace(string message, Exception ex)
        {
            Instance.Trace(ex, message);
        }

        public static void Trace(object data)
        {
            Trace("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                }));
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
                JsonConvert.SerializeObject(data, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                }));
        }
    }
}
