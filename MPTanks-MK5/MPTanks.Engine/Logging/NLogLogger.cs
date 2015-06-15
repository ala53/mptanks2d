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
            LoggerInstance.ErrorException("Engine Error", ex);
        }
        public void Error(string message, Exception ex)
        {
            LoggerInstance.ErrorException(message, ex);
        }

        public void Error(string message)
        {
            LoggerInstance.Error(message);
        }

        public void Fatal(Exception ex)
        {
            LoggerInstance.FatalException("Engine Fatal", ex);
            throw ex;
        }
        public void Fatal(string message, Exception ex)
        {
            LoggerInstance.FatalException(message, ex);
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
            LoggerInstance.TraceException("Engine Trace()", ex);
        }
        public void Trace(string message, Exception ex)
        {
            LoggerInstance.TraceException(message, ex);
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
