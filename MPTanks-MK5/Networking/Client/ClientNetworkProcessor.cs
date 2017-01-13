using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Networking.Common.Actions;
using MPTanks.Networking.Common.Actions.ToClient;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Tanks;
using MPTanks.Engine.Helpers;
using MPTanks.Engine.Logging;

namespace MPTanks.Networking.Client
{
    public class ClientNetworkProcessor : NetworkProcessorBase
    {
        public NetClient Client { get; private set; }
        public override ILogger Logger => Client.Logger;
        public ClientNetworkProcessor(NetClient client)
        {
            Client = client;
        }

        private bool _shouldMakeNewGameOnFullGameState = true;
        public override void ProcessToClientAction(NetConnection client, ActionBase action)
        {
            if (action is CountdownStartedAction)
            {
                var act = action as CountdownStartedAction;
                Client.IsInCountdown = true;
                Client.RemainingCountdownTime = act.CountdownTime;
            }
            else if (action is FullGameStateSentAction)
            {
                var act = action as FullGameStateSentAction;
                Client._interpolator.ElapsedTime = TimeSpan.MaxValue;
                if (_shouldMakeNewGameOnFullGameState)
                {
                    Client.GameInstance.EngineSettings = act.EngineSettings;
                    Client.GameInstance.FullGameState = act.State;
                    Client.NeedsToSelectTank = true;
                }
                else
                {
                    act.State.Apply(Client.GameInstance.Game);
                    //Client.Game.UnsafeTickGameWorld(Client.NetworkClient.ServerConnection.AverageRoundtripTime / 2);
                }
                _shouldMakeNewGameOnFullGameState = false;
            }
            else if (action is GameCreatedAction)
            {
                _shouldMakeNewGameOnFullGameState = true;
            }
            else if (action is GameEndedAction)
            {
                var act = action as GameEndedAction;
                Team winningTeam = Client.Game.Gamemode.Teams.FirstOrDefault(
                    t => t.TeamId == act.WinningTeamId) ?? Team.Null;

                if (act.WinningTeamId == Team.Indeterminate.TeamId)
                    winningTeam = Team.Indeterminate;

                typeof(Gamemode).GetProperty(nameof(Gamemode.WinningTeam))
                    .SetValue(Client.Game.Gamemode, winningTeam);
                typeof(Gamemode).GetProperty(nameof(Gamemode.GameEnded))
                    .SetValue(Client.Game.Gamemode, true);
            }
            else if (action is GamemodeStateChangedAction)
            {
                Client.Game.Gamemode.ReceiveStateData(((GamemodeStateChangedAction)action).NewState);
            }
            else if (action is GameObjectCreatedAction)
            {
                var reader = ByteArrayReader.Get(((GameObjectCreatedAction)action).State.Data);
                GameObject.CreateAndAddFromSerializationInformation(
                    Client.Game, reader);
                reader.Release();
            }
            else if (action is GameObjectDestroyedAction)
            {
                var act = action as GameObjectDestroyedAction;
                if (Client.Game.GameObjectsById.ContainsKey(act.ObjectId))
                    Client.Game.RemoveGameObject(
                        Client.Game.GameObjectsById[act.ObjectId], null, true);
            }
            else if (action is GameObjectDestructionEndedAction)
            {
                var act = action as GameObjectDestructionEndedAction;
                if (Client.Game.GameObjectsById.ContainsKey(act.ObjectId))
                    Client.Game.ImmediatelyForceObjectDestruction(
                        Client.Game.GameObjectsById[act.ObjectId]);
            }
            else if (action is GameStartedAction)
            {
                Client.Game.BeginGame(false);
            }
            else if (action is ObjectBasicPropertyChangedAction)
            {
                var act = action as ObjectBasicPropertyChangedAction;
                if (!Client.Game.GameObjectsById.ContainsKey(act.ObjectId))
                    return;

                var obj = Client.Game.GameObjectsById[act.ObjectId];

                switch (act.EventType)
                {
                    case GameObject.BasicPropertyChangeEventType.AngularVelocity:
                        obj.AngularVelocity = act.FloatValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.Health:
                        obj.Health = act.FloatValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.IsSensor:
                        obj.IsSensor = act.BoolValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.IsStatic:
                        obj.IsStatic = act.BoolValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.LinearVelocity:
                        obj.LinearVelocity = act.VectorValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.Position:
                        obj.Position = act.VectorValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.Restitution:
                        obj.Restitution = act.FloatValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.Rotation:
                        obj.Rotation = act.FloatValue;
                        break;
                    case GameObject.BasicPropertyChangeEventType.Size:
                        obj.Size = act.VectorValue;
                        break;
                }
            }
            else if (action is ObjectStateChangedAction)
            {
                var act = action as ObjectStateChangedAction;
                if (!Client.Game.GameObjectsById.ContainsKey(
                    act.ObjectId))
                    return;

                Client.Game.GameObjectsById[act.ObjectId]
                    .ReceiveStateData(act.PrivateData);

            }
            else if (action is PartialGameStateUpdateAction)
            {
                var act = action as PartialGameStateUpdateAction;
                Client._interpolator.Compute(Client.Game, act.StatePartial);
                //Client.NetworkClient.ServerConnection.AverageRoundtripTime / 2);
            }
            else if (action is PlayerAllowedTankTypesSentAction)
            {
                var act = action as PlayerAllowedTankTypesSentAction;
                Client.Player.AllowedTankTypes = act.AllowedTankTypeReflectionNames;
            }
            else if (action is PlayerInputChangedAction)
            {
                var act = action as PlayerInputChangedAction;
                if (Client.Game.FindPlayer(act.PlayerId) == null) return; //disregard: player not found
                //if (act.PlayerId == Client.PlayerId) return;
                Client.Game.InjectPlayerInput(act.PlayerId, act.InputState);
            }
            else if (action is PlayerUpdateAction)
            {
                return;
                var act = action as PlayerUpdateAction;
                if (Client.Game.FindPlayer(act.Player.Id) != null)
                {
                    act.Player.Apply(Client.Game.FindPlayer<NetworkPlayer>(act.Player.Id));
                    Client.Game.Gamemode.FullState = act.GamemodeState;
                    act.Player.ApplySecondPass(Client.Game.FindPlayer<NetworkPlayer>(act.Player.Id), Client.Game);
                    return; //update: already exists
                }
                Client.Game.AddPlayer(act.Player.ToNetworkPlayer());
                Client.Game.Gamemode.FullState = act.GamemodeState;
                act.Player.ApplySecondPass(
                    (NetworkPlayer)Client.Game.PlayersById[act.Player.Id], Client.Game);
            }
            else if (action is PlayerLeftAction)
            {
                var act = action as PlayerLeftAction;
                if (Client.Game.FindPlayer(act.PlayerId) == null) return;

                Client.Game.Gamemode.FullState = act.GamemodeState;
                Client.Game.RemovePlayer(Client.Game.FindPlayer(act.PlayerId));
            }
            else if (action is PlayerReadyToStartChangedAction)
            {
                var act = action as PlayerReadyToStartChangedAction;
                if (Client.Game.FindPlayer(act.PlayerId) == null) return;
                Client.Game.FindPlayer<NetworkPlayer>(act.PlayerId)
                    .IsReady = act.IsReadyToStart;
            }
            else if (action is PlayerSelectedTankAction)
            {
                var act = action as PlayerSelectedTankAction;
                if (Client.Game.FindPlayer(act.PlayerId) == null) return;
                Client.Game.FindPlayer(act.PlayerId)
                    .SelectedTankReflectionName = act.TankType;
            }
            else if (action is PlayerTankSelectionAcknowledgedAction)
            {
                var act = action as PlayerTankSelectionAcknowledgedAction;
                Client.NeedsToSelectTank = !act.WasAccepted;
            }
            else if (action is ReceivedChatMessageAction)
            {
                var act = action as ReceivedChatMessageAction;
                Client.Chat.AddMessage(new Chat.ChatMessage()
                {
                    Message = act.Message,
                    SenderId = act.SenderId
                });
            }
            else if (action is TimescaleChangedAction)
            {
                var act = action as TimescaleChangedAction;
                Client.Game.Timescale = act.Timescale;
            }
        }

        public override void ProcessToClientMessage(NetConnection client, MessageBase message)
        {
        }
    }
}
