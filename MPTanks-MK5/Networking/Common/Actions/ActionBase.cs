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
        static ActionBase()
        {
            foreach (var type in
                GetTypesInNamespace(typeof(ActionBase).Assembly, "MPTanks.Networking.Common.Actions.ToClient")
                .OrderBy(a => a.Name))
                RegisterToClientActionType(type);
            foreach (var type in
                GetTypesInNamespace(typeof(ActionBase).Assembly, "MPTanks.Networking.Common.Actions.ToServer")
                .OrderBy(a => a.Name))
                RegisterToServerActionType(type);
        }
        private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }
        
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
