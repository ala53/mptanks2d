using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToServer
{
    public class SentChatMessageAction : ActionBase
    {
        public string Message { get; private set; }
        public ushort[] Targets { get; private set; }
        public SentChatMessageAction()
        {
        }

        public SentChatMessageAction(string msg, params NetworkPlayer[] players)
        {
            Message = msg;
            if (players == null)
                Targets = new ushort[0];
            else
            {
                Targets = new ushort[players.Length];
                for (var i = 0; i < players.Length; i++) Targets[i] = players[i].Id;
            }
        }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            Message = message.ReadString();
            var tgtCount = message.ReadInt32();
            Targets = new ushort[tgtCount];
            for (var i = 0; i < tgtCount; i++)
                Targets[i] = (ushort)message.ReadUInt32(GameCore.PlayerIdNumberOfBits);
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(Message);
            message.Write(Targets.Length);
            foreach (var tgt in Targets)
                message.Write(tgt, GameCore.PlayerIdNumberOfBits);
        }
    }
}
