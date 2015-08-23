using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Sent to let the client know what types of tanks they are allowed to choose
    /// </summary>
    public class PlayerAllowedTankTypesSentAction : ActionBase
    {
        public string[] AllowedTankTypeReflectionNames { get; private set; }
        public PlayerAllowedTankTypesSentAction(NetIncomingMessage message) : base(message)
        {
            var arr = new string[message.ReadInt32()];
            for (var i = 0; i < arr.Length; i++)
                arr[i] = message.ReadString();
        }

        public PlayerAllowedTankTypesSentAction(string[] allowedTypeReflectionNames)
        {
            AllowedTankTypeReflectionNames = allowedTypeReflectionNames;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(AllowedTankTypeReflectionNames.Length);
            foreach (var name in AllowedTankTypeReflectionNames)
                message.Write(name);
        }
    }
}
