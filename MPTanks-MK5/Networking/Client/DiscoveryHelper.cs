using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Client
{
    public static class DiscoveryHelper
    {
        public class DiscoveryResponse
        {
            public string Address;
            public ushort Port;
            public bool AllowsHotJoin;
            public bool HasPassword;
            public string ServerName;
            public string GamemodeName;
            public string GamemodeDescription;
            public int PlayerCount;
            public int MaxPlayers;
            public string MapName;
            public Modding.ModInfo[] Mods;
        }
        /// <summary>
        /// Queries the current network for all servers running on their default ports, waiting
        /// 1 second to aggregate responses.
        /// </summary>
        /// <returns></returns>
        public static Task<DiscoveryResponse[]> DoDiscoveryAsync()
        {
            return Task.Run(async () =>
            {
                var responses = new List<DiscoveryResponse>();

                var client = new Lidgren.Network.NetClient(new NetPeerConfiguration("MPTANKS"));
                client.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                client.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

                client.DiscoverLocalPeers(33132); //DEFAULT PORT

                await Task.Delay(1000); // 5 second timeout

                NetIncomingMessage msg;
                while ((msg = client.ReadMessage()) != null)
                    if (msg.MessageType == NetIncomingMessageType.DiscoveryResponse)
                        try { responses.Add(Read(msg)); } catch { }

                responses.Add(new DiscoveryResponse
                {
                    Address = "HELP",
                    Port = 555,
                    AllowsHotJoin = false,
                    GamemodeDescription = "BEEP",
                    GamemodeName = "JEEP",
                    HasPassword = false,
                    MapName = "MEEP",
                    MaxPlayers = 44,
                    Mods = new Modding.ModInfo[]
                    {
                        new Modding.ModInfo()
                        {
                            ModName = "COREASSETS"
                        }
                    },
                    PlayerCount = 5,
                    ServerName = "RIP"
                });
                return responses.ToArray();
            });
        }

        /// <summary>
        /// Requests the information from a server with a 10 second timeout. If a response is not 
        /// received, it returns null. Otherwise, it returns the information provided by the server.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Task<DiscoveryResponse> RequestServerInfo(string server, ushort port)
        {
            return Task.Run(() =>
            {
                var client = new Lidgren.Network.NetClient(new NetPeerConfiguration("MPTANKS"));
                client.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                client.Configuration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

                client.DiscoverKnownPeer(server, port);

                var msg = client.WaitMessage(10000);

                if (msg == null) return null;
                if (msg.MessageType == NetIncomingMessageType.DiscoveryResponse)
                    try { return Read(msg); } catch { }

                return null;
            });
        }

        private static DiscoveryResponse Read(NetIncomingMessage msg)
        {
            var resp = new DiscoveryResponse();
            resp.Address = msg.SenderEndPoint.Address.ToString();
            resp.Port = (ushort)msg.SenderEndPoint.Port;

            resp.AllowsHotJoin = msg.ReadBoolean();
            resp.HasPassword = msg.ReadBoolean();
            msg.SkipPadBits();
            resp.ServerName = msg.ReadString();
            resp.GamemodeName = msg.ReadString();
            resp.GamemodeDescription = msg.ReadString();
            resp.PlayerCount = msg.ReadInt32();
            resp.MaxPlayers = msg.ReadInt32();
            resp.MapName = msg.ReadString();

            var modCount = msg.ReadInt32();
            var modList = new List<Modding.ModInfo>();

            for (var i = 0; i < modCount; i++)
            {
                var inf = new Modding.ModInfo();
                inf.ModName = msg.ReadString();
                inf.ModMajor = msg.ReadInt32();
                inf.ModMinor = msg.ReadInt32();
            }

            return resp;
        }
    }
}
