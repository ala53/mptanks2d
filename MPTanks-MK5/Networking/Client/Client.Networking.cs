using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Client
{
    public partial class Client
    {
        private void SetupNetwork()
        {
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.Data);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.DebugMessage);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.Error);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.Receipt);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.StatusChanged);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            NetworkClient.Configuration.EnableMessageType(NetIncomingMessageType.WarningMessage);
        }
        private void ProcessMessages()
        {
            NetIncomingMessage msg;

            while ((msg = NetworkClient.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    case NetIncomingMessageType.Data:
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        break;
                }
            }
        }
    }
}
