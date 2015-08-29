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
        private void SetupNetwork()
        {
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.Data);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.DebugMessage);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.Error);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.Receipt);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.StatusChanged);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            NetworkServer.Configuration.EnableMessageType(NetIncomingMessageType.WarningMessage);
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
                return; //only every other frame
            }
            NetworkServer.FlushSendQueue();
        }
    }
}
