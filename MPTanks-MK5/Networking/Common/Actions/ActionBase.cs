using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lidgren.Network;

namespace MPTanks.Networking.Common.Actions
{
    public abstract class ActionBase : MessageBase
    {
        
        public ActionBase() { }

        public static void RegisterToClientActionType(Type actionType)
        {
            NetworkProcessorBase.RegisterToClientActionType(actionType);
        }

        public static void RegisterToServerActionType(Type actionType)
        {
            NetworkProcessorBase.RegisterToServerActionType(actionType);
        }
    }
}
