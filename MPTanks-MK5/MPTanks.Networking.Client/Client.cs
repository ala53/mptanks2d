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
        public Client(string connection, bool connectOnInit = true)
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
