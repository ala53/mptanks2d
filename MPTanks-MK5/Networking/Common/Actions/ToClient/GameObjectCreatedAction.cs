using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Engine;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Called on GameObject creation to send the full state to the client
    /// </summary>
    public class GameObjectCreatedAction : ActionBase
    {
        public Game.FullObjectState State { get; private set; }
        public GameObjectCreatedAction(NetIncomingMessage message) : base(message)
        {
            message.ReadBytes(message.ReadUInt16());
        }

        public GameObjectCreatedAction(GameObject obj)
        {
            State = new Game.FullObjectState(obj.FullState);
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write((ushort)State.Data.Length);
            message.Write(State.Data);
        }
    }
}
