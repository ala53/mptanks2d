using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Engine;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GameObjectDestroyedAction : ActionBase
    {
        public ushort ObjectId { get; set; }
        public GameObjectDestroyedAction(NetIncomingMessage message) : base(message)
        {
            ObjectId = message.ReadUInt16();
        }

        public GameObjectDestroyedAction(GameObject obj)
        {
            ObjectId = obj.ObjectId;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(ObjectId);
        }
    }
}
