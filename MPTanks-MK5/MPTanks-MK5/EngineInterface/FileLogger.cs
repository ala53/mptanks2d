using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5.EngineInterface
{
    class FileLogger : Engine.Logging.ILogger
    {

        public void Log(string message)
        {
            Logger.Log(message);
        }

        public void Warning(string warn)
        {
            Logger.Warning(warn);
        }

        public void Error(string err)
        {
#if DEBUG
            throw new Exception(err);
#else
            Logger.Error(err);
#endif
        }

        public void Fatal(string message)
        {
#if DEBUG
            throw new Exception(message);
#else
            Logger.Fatal(message);
#endif
        }

        public void LogObjectCreated(Engine.GameObject obj, Engine.GameObject creator = null, string additionalData = "")
        {
            if (creator == null)
                Logger.Log(obj.GetType().Name + " created (ID " + obj.ObjectId + " - " + obj.ToString() + ")" +
                    (additionalData == "" ? "" : ", " + additionalData));
            else
                Logger.Log(obj.GetType().Name + " created (ID " + obj.ObjectId + " - " + obj.ToString() + ") by " +
                    creator.GetType().Name + " (ID " + creator.ObjectId + " - " + creator.ToString() + ")" + 
                    (additionalData == "" ? "" : ", " + additionalData));

        }

        public void LogObjectDamaged(Engine.GameObject damaged, Engine.GameObject damager = null, string additionalData = "")
        {
            if (damager == null)
                Logger.Log(damaged.GetType().Name + "(ID " + damaged.ObjectId + " - " +  damaged.ToString() +
                    ") damaged" + (additionalData == "" ? "" : ", " + additionalData));
            else
                Logger.Log(damaged.GetType().Name + "(ID " + damaged.ObjectId +
                    ") damaged by " + damager.GetType().Name + " (ID " + 
                    damager.ObjectId + " - " + damaged.ToString() +
                    (additionalData == "" ? ")" : "), " + additionalData));
        }

        public void LogObjectDestroyed(Engine.GameObject destroyed, Engine.GameObject destroyer = null, string additionalData = "")
        {
            if (destroyer == null)
                Logger.Log(destroyed.GetType().Name + "(ID " + destroyed.ObjectId + " - " + destroyed.ToString() +
                    ") destroyed" + (additionalData == "" ? "" : ", " + additionalData));
            else
                Logger.Log(destroyed.GetType().Name + "(ID " + destroyed.ObjectId + " - " + destroyed.ToString() +
                    ") destroyed by " + destroyer.GetType().Name + " (ID " + 
                    destroyer.ObjectId + " - " + destroyer.ToString() +
                    (additionalData == "" ? ")" : "), " + additionalData));
        }
    }
}
