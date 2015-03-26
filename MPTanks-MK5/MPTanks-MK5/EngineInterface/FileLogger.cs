using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.EngineInterface
{
    class FileLogger : MPTanks.Engine.Logging.ILogger
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

        public void LogObjectCreated(MPTanks.Engine.GameObject obj, MPTanks.Engine.GameObject creator = null, string additionalData = "")
        {
            if (creator == null)
                Logger.Debug(obj.GetType().Name + " created (ID " + obj.ObjectId + " - " + obj.ToString() + ")" +
                    (additionalData == "" ? "" : ", " + additionalData));
            else
                Logger.Debug(obj.GetType().Name + " created (ID " + obj.ObjectId + " - " + obj.ToString() + ") by " +
                    creator.GetType().Name + " (ID " + creator.ObjectId + " - " + creator.ToString() + ")" +
                    (additionalData == "" ? "" : ", " + additionalData));

        }

        public void LogObjectDamaged(MPTanks.Engine.GameObject damaged, MPTanks.Engine.GameObject damager = null, string additionalData = "")
        {
            if (damager == null)
                Logger.Debug(damaged.GetType().Name + "(ID " + damaged.ObjectId + " - " + damaged.ToString() +
                    ") damaged" + (additionalData == "" ? "" : ", " + additionalData));
            else
                Logger.Debug(damaged.GetType().Name + "(ID " + damaged.ObjectId +
                    ") damaged by " + damager.GetType().Name + " (ID " +
                    damager.ObjectId + " - " + damaged.ToString() +
                    (additionalData == "" ? ")" : "), " + additionalData));
        }

        public void LogObjectDestroyed(MPTanks.Engine.GameObject destroyed, MPTanks.Engine.GameObject destroyer = null, string additionalData = "")
        {
            if (destroyer == null)
                Logger.Debug(destroyed.GetType().Name + "(ID " + destroyed.ObjectId + " - " + destroyed.ToString() +
                    ") destroyed" + (additionalData == "" ? "" : ", " + additionalData));
            else
                Logger.Debug(destroyed.GetType().Name + "(ID " + destroyed.ObjectId + " - " + destroyed.ToString() +
                    ") destroyed by " + destroyer.GetType().Name + " (ID " +
                    destroyer.ObjectId + " - " + destroyer.ToString() +
                    (additionalData == "" ? ")" : "), " + additionalData));
        }
    }
}
