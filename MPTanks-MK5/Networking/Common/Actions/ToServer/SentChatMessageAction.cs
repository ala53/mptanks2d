using Lidgren.Network;
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
        public Guid[] Targets { get; private set; }
        public SentChatMessageAction(NetIncomingMessage message) : base(message)
        {
            Message = message.ReadString();
            var tgtCount = message.ReadInt32();
            Targets = new Guid[tgtCount];
            for (var i = 0; i < tgtCount; i++)
                Targets[i] = new Guid(message.ReadBytes(16));
        }

        public SentChatMessageAction(string msg, params NetworkPlayer[] players)
        {
            Message = msg;
            if (players == null)
                Targets = new Guid[0];
            else
            {
                Targets = new Guid[players.Length];
                for (var i = 0; i < players.Length; i++) Targets[i] = players[i].Id;
            }
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(Message);
            message.Write(Targets.Length);
            foreach (var tgt in Targets)
                message.Write(tgt.ToByteArray());
        }
    }
}
