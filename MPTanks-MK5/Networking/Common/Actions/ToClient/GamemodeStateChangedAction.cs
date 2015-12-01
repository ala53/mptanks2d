using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GamemodeStateChangedAction : ActionBase
    {
        public byte[] NewState { get; set; }
        public GamemodeStateChangedAction()
        {
        }

        public GamemodeStateChangedAction(byte[] newState)
        {
            NewState = newState;
        }

        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            NewState = message.ReadBytes(message.ReadUInt16());
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write((ushort)NewState.Length);
            message.Write(NewState);
        }
    }
}
