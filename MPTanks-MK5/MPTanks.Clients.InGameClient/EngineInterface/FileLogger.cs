using MPTanks.Engine.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.EngineInterface
{
    public class FileLogger : ILogger
    {
        public void Debug(string message)
        {
            Logger.Instance.Debug(message);
        }

        public void Error(Exception ex)
        {
            Logger.Instance.ErrorException("Engine Error", ex);
        }
        public void Error(string message, Exception ex)
        {
            Logger.Instance.ErrorException(message, ex);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Fatal(Exception ex)
        {
            Logger.Instance.FatalException("Engine Fatal", ex);
            throw ex;
        }
        public void Fatal(string message, Exception ex)
        {
            Logger.Instance.FatalException(message, ex);
            throw ex;
        }


        public void Fatal(string message)
        {
            Logger.Instance.Fatal(message);
        }

        public void Info(object data)
        {
            Info("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Info(string message)
        {
            Logger.Instance.Info(message);
        }

        public void Trace(Exception ex)
        {
            Logger.Instance.TraceException("Engine Trace()", ex);
        }
        public void Trace(string message, Exception ex)
        {
            Logger.Instance.TraceException(message, ex);
        }

        public void Trace(object data)
        {
            Trace("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Trace(string message)
        {
            Logger.Instance.Trace(message);
        }

        public void Warning(string message)
        {
            Logger.Instance.Warn(message);
        }

        public void Warning(object data)
        {
            Warning("[" + data.GetType().AssemblyQualifiedName + "]\n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
