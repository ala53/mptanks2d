using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Logging
{
    public class NullLogger : ILogger
    {
        public LogLevel Level => LogLevel.Fatal;
        public void Debug(string message)
        {
        }

        public void Error(Exception ex)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception ex)
        {
        }

        public void Fatal(Exception ex)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Fatal(string message, Exception ex)
        {
        }

        public void Info(object data)
        {
        }

        public void Info(string message)
        {
        }

        public void Trace(Exception ex)
        {
        }

        public void Trace(object data)
        {
        }

        public void Trace(string message)
        {
        }

        public void Trace(string message, Exception ex)
        {
        }

        public void Warning(object data)
        {
        }

        public void Warning(string message)
        {
        }
    }
}
