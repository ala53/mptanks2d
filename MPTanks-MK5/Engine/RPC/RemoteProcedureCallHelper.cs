using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.RPC
{
    public class RemoteProcedureCallHelper
    {
        private GameCore _game;
        internal RemoteProcedureCallHelper(GameCore game)
        {
            _game = game;
        }
        public void Call(GameObject obj, Delegate call, params object[] args)
        {
            if (!call.Method.DeclaringType.IsSubclassOf(typeof(GameObject)))
                throw new Exception("The passed delegate MUST be a function from a GameObject");

            OnRPCCreated(obj, new RPC
            {
                Type = call.Method.DeclaringType.AssemblyQualifiedName,
                Method = call.Method.Name,
                TargetObject = obj.ObjectId,
                ArgumentsType = args.GetType().AssemblyQualifiedName,
                SerializedArgs = JsonConvert.SerializeObject(args, Formatting.None, _settings)
            });
        }

        private JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
            TypeNameHandling = TypeNameHandling.All
        };

        public void ReceiveCall(RPC call)
        {
            var method = Type.GetType(call.Type).GetMethod(call.Method,
                 System.Reflection.BindingFlags.Public |
                 System.Reflection.BindingFlags.NonPublic |
                 System.Reflection.BindingFlags.Instance);

            method.Invoke(_game.GameObjectsById[call.TargetObject],
                (object[])JsonConvert.DeserializeObject(
                call.SerializedArgs, Type.GetType(call.ArgumentsType), _settings));

            _game.GameObjectsById[call.TargetObject].OnReceiveRPC(call);
        }

        public event EventHandler<RPC> OnRPCCreated = delegate { };
    }
    public class RPC
    {
        public ushort TargetObject { get; set; }
        public string Type { get; set; }
        public string Method { get; set; }
        public string SerializedArgs { get; set; }
        public string ArgumentsType { get; set; }
    }
}
