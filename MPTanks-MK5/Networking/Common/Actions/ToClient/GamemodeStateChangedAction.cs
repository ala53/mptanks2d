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
        public byte[] NewState { get; private set; }
        public GamemodeStateChangedAction(NetIncomingMessage message) : base(message)
        {
            NewState = message.ReadBytes(message.ReadUInt16());
        }

        public GamemodeStateChangedAction(byte[] newState)
        {
            NewState = newState;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write((ushort)NewState.Length);
            message.Write(NewState);
        }
    }
}
