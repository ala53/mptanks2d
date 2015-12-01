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
        public GameObjectDestroyedAction()
        {
        }

        public GameObjectDestroyedAction(GameObject obj)
        {
            ObjectId = obj.ObjectId;
        }

        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            ObjectId = message.ReadUInt16();
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(ObjectId);
        }
    }
}
