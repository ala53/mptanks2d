using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    class Logger : MPTanks.Engine.Logging.ILogger
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

        public void LogObjectCreated(MPTanks.Engine.GameObject obj, MPTanks.Engine.GameObject creator = null, string additionalData = "")
        {
        }

        public void LogObjectDamaged(MPTanks.Engine.GameObject damaged, MPTanks.Engine.GameObject damager = null, string additionalData = "")
        {
        }

        public void LogObjectDestroyed(MPTanks.Engine.GameObject destroyed, MPTanks.Engine.GameObject destroyer = null, string additionalData = "")
        {
        }
    }
}
