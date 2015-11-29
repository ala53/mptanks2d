using Lidgren.Network;
using MPTanks.Networking.Common;
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
            foreach (var msgType in Enum.GetValues(typeof(NetIncomingMessageType)))
            {
                if (NetworkClient.Configuration.IsMessageTypeEnabled((NetIncomingMessageType)msgType))
                    Logger.Trace($"[CLIENT] Message type {Enum.GetName(typeof(NetIncomingMessageType), msgType)} enabled");
            }
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
                        if (msg.SequenceChannel == Channels.GameplayData)
                            MessageProcessor.ProcessMessages(msg);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Logger.Debug(msg.ReadString());
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Logger.Error(msg.ReadString());
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)msg.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                Status = ClientStatus.Connected;
                                Message = "Connected, waiting for server...";
                                break;
                            case NetConnectionStatus.Disconnected:
                                var reason = msg.ReadString();
                                Status = ClientStatus.Disconnected;
                                if (string.IsNullOrEmpty(reason))
                                    Message = "Disconnected (Unknown Error)";
                                else
                                    Message = "Disconnected: " + reason;

                                break;
                        }
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        Logger.Trace(msg.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Logger.Warning(msg.ReadString());
                        break;
                }
            }
        }
    }
}
