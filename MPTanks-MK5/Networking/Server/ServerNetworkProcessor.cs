﻿using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Networking.Common.Actions;
using MPTanks.Networking.Common.Actions.ToServer;
using MPTanks.Networking.Common.Actions.ToClient;

namespace MPTanks.Networking.Server
{
    public class ServerNetworkProcessor : NetworkProcessorBase
    {
        public Server Server { get; private set; }

        public ServerNetworkProcessor(Server server)
        {
            Server = server;
        }

        public override void ProcessToServerAction(ActionBase action)
        {
            if (action is InputChangedAction)
            {
                Server.MessageProcessor.SendMessage(
                    new PlayerInputChangedAction(Server.Connections.PlayerTable[action.MessageFrom.SenderConnection].Player,
                    ((InputChangedAction)action).InputState));
            }

            if (action is PlayerTankTypeSelectedAction)
            {
                var player = Server.Connections.PlayerTable[action.MessageFrom.SenderConnection];

                player.Player.SelectTank(((PlayerTankTypeSelectedAction)action).SelectedTypeReflectionName);

                Server.MessageProcessor.SendMessage(
                    new OtherPlayerSelectedTankAction(player.Player,
                    ((PlayerTankTypeSelectedAction)action).SelectedTypeReflectionName));
            }

            if (action is RequestFullGameStateAction)
            {
                Server.MessageProcessor.SendPrivateMessage(
                    Server.Connections.PlayerTable[action.MessageFrom.SenderConnection],
                    new FullGameStateSentAction(Server.GameInstance.Game));
            }

            if (action is SentChatMessageAction)
            {
                var act = action as SentChatMessageAction;
                Server.ChatHandler.ForwardMessage(act.Message,
                    Server.Connections.PlayerTable[action.MessageFrom.SenderConnection],
                    act.Targets.Select(a=>Server.GetPlayer(a)).ToArray());
            }

            if (action is PlayerReadyChangedAction)
            {

            }
        }


        public override void ProcessToServerMessage(MessageBase message)
        {
        }

        public override void OnProcessingError(Exception error)
        {
            Server.Logger.Error("Message processing from client", error);
        }

        private Dictionary<ServerPlayer, List<MessageBase>> _privateQueue =
            new Dictionary<ServerPlayer, List<MessageBase>>();
        public IReadOnlyDictionary<ServerPlayer, List<MessageBase>> PrivateMessageQueues => _privateQueue;

        public void SendPrivateMessage(ServerPlayer player, MessageBase message)
        {
            if (!_privateQueue.ContainsKey(player))
                _privateQueue.Add(player, new List<MessageBase>());

            _privateQueue[player].Add(message);
        }

        public void WritePrivateMessages(ServerPlayer player, NetOutgoingMessage message)
        {
            if (!_privateQueue.ContainsKey(player))
                _privateQueue.Add(player, new List<MessageBase>());

            var queue = _privateQueue[player];
            message.Write((ushort)queue.Count);
            foreach (var msg in queue)
                message.Write(TypeIndexTable[msg.GetType()]);
            foreach (var msg in queue)
            {
                msg.Serialize(message);
            }

            queue.Clear();
        }

        public bool HasPrivateMessages(ServerPlayer player)
        {
            if (!_privateQueue.ContainsKey(player))
                _privateQueue.Add(player, new List<MessageBase>());

            return _privateQueue[player].Count > 0;
        }

        public void ClearPrivateQueues()
        {
            _privateQueue.Clear();
        }
    }
}
