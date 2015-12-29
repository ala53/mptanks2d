using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Logging
{
    public class NLogLogger : ILogger
    {
        public LogLevel Level
        {
            get
            {
                if (LoggerInstance.IsTraceEnabled)
                    return LogLevel.Trace;
                if (LoggerInstance.IsDebugEnabled)
                    return LogLevel.Debug;
                if (LoggerInstance.IsInfoEnabled)
                    return LogLevel.Info;
                if (LoggerInstance.IsWarnEnabled)
                    return LogLevel.Warn;
                if (LoggerInstance.IsErrorEnabled)
                    return LogLevel.Error;

                return LogLevel.Fatal;
            }
        }
        public NLog.Logger LoggerInstance { get; private set; }
        public NLogLogger(NLog.Logger instance)
        {
            LoggerInstance = instance;
        }
        public void Debug(string message)
        {
            LoggerInstance.Debug(message);
        }

        public void Error(Exception ex)
        {
            LoggerInstance.Error(ex, "Engine Error");
        }
        public void Error(string message, Exception ex)
        {
            LoggerInstance.Error(ex, message);
        }

        public void Error(string message)
        {
            LoggerInstance.Error(message);
        }

        public void Fatal(Exception ex)
        {
            LoggerInstance.Fatal(ex, "Engine Fatal");
            throw ex;
        }
        public void Fatal(string message, Exception ex)
        {
            LoggerInstance.Fatal(ex, message);
            throw ex;
        }


        public void Fatal(string message)
        {
            LoggerInstance.Fatal(message);
        }

        public void Info(object data)
        {
            Info("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Info(string message)
        {
            LoggerInstance.Info(message);
        }

        public void Trace(Exception ex)
        {
            LoggerInstance.Trace(ex, "Engine Trace()");
        }
        public void Trace(string message, Exception ex)
        {
            LoggerInstance.Trace(ex, message);
        }

        public void Trace(object data)
        {
            Trace("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Trace(string message)
        {
            LoggerInstance.Trace(message);
        }

        public void Warning(string message)
        {
            LoggerInstance.Warn(message);
        }

        public void Warning(object data)
        {
            Warning("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
