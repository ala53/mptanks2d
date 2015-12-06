using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Client.Chat
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public ushort SenderId { get; set; }
        public NetClient Client { get; set; }
        public NetworkPlayer Sender => Client.Game.FindPlayer<NetworkPlayer>(SenderId);
        public string SenderName
        {
            get
            {
                if (Sender != null) return "[" + Sender.ClanName + "] " + Sender.Username;
                return "";
            }
        }
        public DateTime SentTime { get; set; }
    }
}
