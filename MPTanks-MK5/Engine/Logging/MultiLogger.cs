using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Logging
{
    public class MultiLogger : ILogger
    {
        private List<ILogger> _loggers = new List<ILogger>();
        public MultiLogger(params ILogger[] loggers)
        {
            _loggers.AddRange(loggers);
        }
        public void AddLogger(params ILogger[] loggers) =>
            _loggers.AddRange(loggers);

        public void Debug(string message)
        {
            _loggers.ForEach(a => a.Debug(message));
        }

        public void Error(Exception ex)
        {
            _loggers.ForEach(a => a.Error(ex));
        }

        public void Error(string message)
        {
            _loggers.ForEach(a => a.Error(message));
        }

        public void Error(string message, Exception ex)
        {
            _loggers.ForEach(a => a.Error(message, ex));
        }

        public void Fatal(Exception ex)
        {
            _loggers.ForEach(a => a.Fatal(ex));
        }

        public void Fatal(string message)
        {
            _loggers.ForEach(a => a.Fatal(message));
        }

        public void Fatal(string message, Exception ex)
        {
            _loggers.ForEach(a => a.Fatal(message, ex));
        }

        public void Info(object data)
        {
            _loggers.ForEach(a => a.Info(data));
        }

        public void Info(string message)
        {
            _loggers.ForEach(a => a.Info(message));
        }

        public void Trace(Exception ex)
        {
            _loggers.ForEach(a => a.Trace(ex));
        }

        public void Trace(object data)
        {
            _loggers.ForEach(a => a.Trace(data));
        }

        public void Trace(string message)
        {
            _loggers.ForEach(a => a.Trace(message));
        }

        public void Trace(string message, Exception ex)
        {
            _loggers.ForEach(a => a.Trace(message, ex));
        }

        public void Warning(object data)
        {
            _loggers.ForEach(a => a.Warning(data));
        }

        public void Warning(string message)
        {
            _loggers.ForEach(a => a.Warning(message));
        }
    }
}
