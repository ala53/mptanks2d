using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server.Chat
{
    public struct ChatMessage
    {
        public ServerPlayer From { get; set; }
        public ServerPlayer To { get; set; }
    }
}
