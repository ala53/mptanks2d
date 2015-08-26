using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GameObjectDestructionEndedAction : ActionBase
    {
        public ushort ObjectId { get; private set; }
        public GameObjectDestructionEndedAction(NetIncomingMessage message) :base(message)
        {
            ObjectId = message.ReadUInt16();
        }
        public GameObjectDestructionEndedAction(GameObject obj)
        {
            ObjectId = obj.ObjectId;
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(ObjectId);
        }
    }
}
