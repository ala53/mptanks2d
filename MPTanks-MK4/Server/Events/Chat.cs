using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class Chat
    {
        public Engine.Players.Player PlayerFrom { get; set; }
        public Engine.Players.Player[] PlayersTo { get; set; }
        public string Message { get; set; }
    }
}
