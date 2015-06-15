using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server.Chat
{
    public class ChatServer
    {
        /// <summary>
        /// The characters that a command must start with to be treated as a command
        /// </summary>
        public string CommandMarker { get; private set; }
    }
}
