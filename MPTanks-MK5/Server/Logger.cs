using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Logger : Engine.Logging.ILogger
    {
        public void Log(string message)
        {
        }

        public void Warning(string warn)
        {
        }

        public void Error(string err)
        {
        }

        public void Fatal(string message)
        {
        }

        public void LogObjectCreated(Engine.GameObject obj, Engine.GameObject creator = null, string additionalData = "")
        {
        }

        public void LogObjectDamaged(Engine.GameObject damaged, Engine.GameObject damager = null, string additionalData = "")
        {
        }

        public void LogObjectDestroyed(Engine.GameObject destroyed, Engine.GameObject destroyer = null, string additionalData = "")
        {
        }
    }
}
