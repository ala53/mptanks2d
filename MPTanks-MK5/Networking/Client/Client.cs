using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Client
{
    public class Client
    {
        public bool Connected { get; private set; }
        public NetworkedGame GameInstance { get; private set; }
        public bool GameRunning { get { return Connected && GameInstance != null; } }
        public Client(string connection, ushort port, string password = null, bool connectOnInit = true)
        {
            //connect to server
            if (connectOnInit)
                Connect();
        }

        public void Connect()
        {

        }

        public void Disconnect()
        {

        }
    }
}
