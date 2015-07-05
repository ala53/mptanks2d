using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Logging
{
    public interface ILogger
    {
        void Trace(string message);
        void Trace(object data);
        void Trace(Exception ex);
        void Trace(string message, Exception ex);
        void Debug(string message);
        void Info(string message);
        void Info(object data);
        void Warning(string message);
        void Warning(object data);
        void Error(string message);
        void Error(Exception ex);
        void Error(string message, Exception ex);
        void Fatal(string message);
        void Fatal(Exception ex);
        void Fatal(string message, Exception ex);
    }
}
