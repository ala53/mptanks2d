using Lidgren.Network;
using MPTanks.Engine.Settings;
using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public partial class Server
    {
        private NetPeerConfiguration SetupNetwork(NetPeerConfiguration configuration)
        {
            configuration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            configuration.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);
            configuration.EnableMessageType(NetIncomingMessageType.Data);
            configuration.EnableMessageType(NetIncomingMessageType.DebugMessage);
            configuration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            configuration.EnableMessageType(NetIncomingMessageType.Error);
            configuration.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            configuration.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            configuration.EnableMessageType(NetIncomingMessageType.Receipt);
            configuration.EnableMessageType(NetIncomingMessageType.StatusChanged);
            configuration.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            configuration.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            configuration.EnableMessageType(NetIncomingMessageType.WarningMessage);

            foreach (var msgType in Enum.GetValues(typeof(NetIncomingMessageType)))
            {
                if (configuration.IsMessageTypeEnabled((NetIncomingMessageType)msgType))
                    Logger.Trace($"[SERVER] Message type {Enum.GetName(typeof(NetIncomingMessageType), msgType)} enabled");
            }

            return configuration;
        }
        private void ProcessMessages()
        {
            NetIncomingMessage msg;
            while ((msg = NetworkServer.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        Login.HandleConnection(msg); break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        var discovery = NetworkServer.CreateMessage();
                        Connections.WriteServerInfo(discovery);
                        NetworkServer.SendDiscoveryResponse(discovery, msg.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Connections.UpdateConnectionStatus(msg.SenderConnection);
                        break;
                    case NetIncomingMessageType.Data:
                        if (msg.SequenceChannel == Channels.GameplayData)
                            MessageProcessor.ProcessMessages(msg);
                        else if (msg.SequenceChannel == Channels.Login)
                            Login.ProcessMessage(msg);
                        break;
                    //Error handling
                    case NetIncomingMessageType.ErrorMessage:
                        Logger.Error(msg.ReadString()); break;
                    case NetIncomingMessageType.WarningMessage:
                        Logger.Warning(msg.ReadString()); break;
                    case NetIncomingMessageType.DebugMessage:
                        if (GlobalSettings.Debug) Logger.Debug(msg.ReadString()); break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        if (GlobalSettings.Debug) Logger.Trace(msg.ReadString()); break;
                }

                NetworkServer.Recycle(msg);
            }
        }

        bool flushedMessagesLastTick = false;
        public void FlushMessages()
        {
            if (flushedMessagesLastTick)
            {
                flushedMessagesLastTick = false;
                //   return; //only every other frame
            }
            NetworkServer.FlushSendQueue();
        }
    }
}
