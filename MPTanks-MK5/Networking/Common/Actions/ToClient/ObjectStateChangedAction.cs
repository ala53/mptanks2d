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
    /// Marks a change to a private value for a game object derived object.
    /// </summary>
    public class ObjectStateChangedAction : ActionBase
    {
        public ushort ObjectId { get; set; }
        public byte[] PrivateData { get; set; }
        public ObjectStateChangedAction()
        {
        }

        public ObjectStateChangedAction(GameObject obj, byte[] state)
        {
            ObjectId = obj.ObjectId;
            PrivateData = state;
        }

        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            ObjectId = message.ReadUInt16();
            PrivateData = message.ReadBytes(message.ReadUInt16());
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(ObjectId);
            message.Write((ushort)PrivateData.Length);
            message.Write(PrivateData);
        }
    }
}
