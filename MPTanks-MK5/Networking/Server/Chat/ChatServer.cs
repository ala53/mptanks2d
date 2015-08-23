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

        public void SendMessage(ServerPlayer playerFrom, ServerPlayer playerTo, string message)
        {

        }

        public void LogMessage(string message)
        {

        }

        #region Commands
        public enum ChatCommandParameter
        {
            /// <summary>
            /// string1
            /// "string 2"
            /// "string \"3\""
            /// </summary>
            String,
            /// <summary>
            /// playername
            /// </summary>
            Player,
            /// <summary>
            /// 1
            /// 2
            /// 1.223
            /// 5.552E72
            /// </summary>
            Number,
            /// <summary>
            /// true
            /// false
            /// </summary>
            Boolean,
            /// <summary>
            /// [ "string 1", string2, "string \" 3" ] 
            /// </summary>
            ArrayOfString,
            /// <summary>
            /// [ player1, player2, player3 ]
            /// </summary>
            ArrayOfPlayer,
            /// <summary>
            /// [ 1, 2, 3 ]
            /// </summary>
            ArrayOfNumber,
            /// <summary>
            /// [ true, false, true ]
            /// </summary>
            ArrayOfBoolean
        }

        public void RegisterCommand(Delegate action, string name, params ChatCommandParameter[] parameters)
        {

        }
        #endregion
    }
}
