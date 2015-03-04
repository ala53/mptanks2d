using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void Warning(string warn);
        void Error(string err);
        void Fatal(string message);
        void LogObjectCreated(GameObject obj, GameObject creator = null, string additionalData = "");
        void LogObjectDamaged(GameObject damaged, GameObject damager = null, string additionalData = "");
        void LogObjectDestroyed(GameObject destroyed, GameObject destroyer = null, string additionalData = "");
    }
}
