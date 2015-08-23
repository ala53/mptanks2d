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
        public ushort ObjectId { get; private set; }
        public byte[] PrivateData { get; private set; }
        public ObjectStateChangedAction(NetIncomingMessage message):base(message)
        {
            ObjectId = message.ReadUInt16();
            PrivateData = message.ReadBytes(message.ReadUInt16());
        }

        public ObjectStateChangedAction(GameObject obj, byte[] state)
        {
            ObjectId = obj.ObjectId;
            PrivateData = state;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(ObjectId);
            message.Write((ushort)PrivateData.Length);
            message.Write(PrivateData);
        }
    }
}
