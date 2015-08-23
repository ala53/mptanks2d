using Lidgren.Network;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Logging;
using MPTanks.Engine.Settings;
using MPTanks.Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Tanks;
using MPTanks.Modding;

namespace MPTanks.Networking.Common.Game
{
    public class FullGameState
    {
        public List<FullObjectState> ObjectStates { get; set; } = new List<FullObjectState>();
        public List<ModInfo> GameLoadedMods { get; set; } = new List<ModInfo>();
        public ModAssetInfo MapInfo { get; set; }
        public string GamemodeReflectionName { get; set; }
        public double CurrentGameTimeMilliseconds { get; set; }
        public string TimescaleString { get; set; }
        public double TimescaleValue { get; set; }
        public bool FriendlyFireEnabled { get; set; }
        public GameCore.CurrentGameStatus Status { get; set; }
        public byte[] GamemodeState { get; set; }
        public List<FullStatePlayer> Players { get; set; } = new List<FullStatePlayer>();
        public ushort NextObjectId { get; set; }

        public GameCore CreateGameFromState(ILogger logger = null, EngineSettings settings = null, float latency = 0)
        {
            bool skipInit = false;
            if (Status == GameCore.CurrentGameStatus.GameRunning || Status == GameCore.CurrentGameStatus.GameEndedStillRunning
                || Status == GameCore.CurrentGameStatus.GameEnded)
                skipInit = true;
            var game = new GameCore(logger ?? new NullLogger(), GamemodeReflectionName, MapInfo, skipInit, settings);

            Apply(game);

            game.UnsafeTickGameWorld(latency);

            return game;
        }

        public void Apply(GameCore game)
        {
            game.Gamemode.FullState = GamemodeState;

            GameCore.TimescaleValue timescale = new GameCore.TimescaleValue(TimescaleValue, TimescaleString);
            foreach (var ts in GameCore.TimescaleValue.Values)
                if (ts.DisplayString == TimescaleString) timescale = ts;

            game.Timescale = timescale;
            game.FriendlyFireEnabled = FriendlyFireEnabled;

            //Do it via reflection to keep api private
            var statusProp = typeof(GameCore).GetProperty(nameof(GameCore.GameStatus));
            statusProp.SetValue(game, Status);
            //Do this with reflection because we want to keep the api private (set game time)
            var timeProp = typeof(GameCore).GetProperty(nameof(GameCore.Time));
            timeProp.SetValue(game, TimeSpan.FromMilliseconds(CurrentGameTimeMilliseconds));
            //Once again, keep the API private
            typeof(GameCore).GetField("_nextObjectId").SetValue(game, NextObjectId);

            foreach (var player in Players)
            {
                var nwPlayer = new NetworkPlayer();
                if (game.PlayersById.ContainsKey(player.Id) && game.PlayersById[player.Id] is NetworkPlayer)
                    nwPlayer = (NetworkPlayer)game.PlayersById[player.Id];
                else if (game.PlayersById.ContainsKey(player.Id))
                    game.RemovePlayer(player.Id);

                nwPlayer.AllowedTankTypes = player.AllowedTankTypes;
                nwPlayer.ClanName = player.ClanName;
                nwPlayer.DisplayName = player.Username;
                nwPlayer.DisplayNameDrawColor = player.UsernameDisplayColor;
                nwPlayer.Game = game;
                nwPlayer.HasCustomTankStyle = player.TankHasCustomStyle;
                nwPlayer.HasSelectedTankYet = player.HasSelectedTank;
                nwPlayer.Id = player.Id;
                nwPlayer.IsAdmin = player.IsAdmin;
                nwPlayer.IsPremium = player.IsPremium;
                nwPlayer.IsSpectator = player.IsSpectator;
                nwPlayer.SelectedTankReflectionName = player.TankReflectionName;
                nwPlayer.SpawnPoint = player.SpawnPoint;
                nwPlayer.Team = (player.TeamId != -3 ? FindTeam(game.Gamemode.Teams, player.TeamId) : null);

                player.PlayerObject = nwPlayer;

                if (!game.PlayersById.ContainsKey(player.Id))
                    game.AddPlayer(nwPlayer);
            }

            //Add all of the game objects
            foreach (var fullState in ObjectStates)
            {
                if (!game.GameObjectsById.ContainsKey(fullState.ObjectId))
                    GameObject.CreateAndAddFromSerializationInformation(game, fullState.Data, true);
                else game.GameObjectsById[fullState.ObjectId].FullState = fullState.Data;
            }

            var objectsToRemove = new List<GameObject>();
            foreach (var obj in game.GameObjects)
            {
                bool found = false;
                foreach (var state in ObjectStates)
                    if (state.ObjectId == obj.ObjectId) found = true;
                if (!found) objectsToRemove.Add(obj);
            }

            foreach (var obj in objectsToRemove)
                game.RemoveGameObject(obj, null, true);

            var playersByTeam = new Dictionary<short, List<NetworkPlayer>>();
            foreach (var player in Players)
            {
                if (!playersByTeam.ContainsKey(player.TeamId))
                    playersByTeam.Add(player.TeamId, new List<NetworkPlayer>());

                playersByTeam[player.TeamId].Add(player.PlayerObject);
            }

            foreach (var kvp in playersByTeam)
            {
                var team = FindTeam(game.Gamemode.Teams, kvp.Key);
                team.Players = kvp.Value.ToArray();
            }

            foreach (var player in Players)
                if (player.PlayerObject.Tank != null)
                    player.PlayerObject.Tank.InputState = player.Input;

            game.Gamemode.DeferredSetFullState();
            foreach (var obj in game.GameObjects)
                obj.SetPostInitStateData();

        }

        private Team FindTeam(Team[] teams, int id)
        {
            foreach (var t in teams)
                if (t.TeamId == id) return t;

            return Team.Null;
        }

        public static FullGameState Create(GameCore game)
        {
            var state = new FullGameState();

            state.SetPlayers(game.Players.Select(x => (NetworkPlayer)x));
            state.SetObjects(game);

            foreach (var mod in ModDatabase.LoadedModules)
                state.GameLoadedMods.Add(new ModInfo
                {
                    ModName = mod.Name,
                    ModMajor = mod.Version.Major,
                    ModMinor = mod.Version.Minor
                });

            state.MapInfo = game.Map.AssetInfo;

            state.CurrentGameTimeMilliseconds = game.Time.TotalMilliseconds;
            state.GamemodeReflectionName = game.Gamemode.ReflectionName;
            state.GamemodeState = game.Gamemode.FullState;
            state.Status = game.GameStatus;
            state.TimescaleString = game.Timescale.DisplayString;
            state.TimescaleValue = game.Timescale.Fractional;
            state.FriendlyFireEnabled = game.FriendlyFireEnabled;

            return state;
        }

        private void SetPlayers(IEnumerable<NetworkPlayer> players)
        {
            foreach (var plr in players)
            {
                var serialized = new FullStatePlayer();
                serialized.AllowedTankTypes = plr.AllowedTankTypes;
                serialized.ClanName = plr.ClanName;
                serialized.HasSelectedTank = plr.HasSelectedTankYet;
                serialized.HasTank = plr.Tank != null;
                serialized.Id = plr.Id;
                serialized.IsAdmin = plr.IsAdmin;
                serialized.IsPremium = plr.IsPremium;
                serialized.IsSpectator = plr.IsSpectator;
                serialized.SpawnPoint = plr.SpawnPoint;
                serialized.TankHasCustomStyle = plr.HasCustomTankStyle;
                serialized.TankObjectId = (plr.Tank != null) ? plr.Tank.ObjectId : (ushort)0;
                serialized.TankReflectionName = (plr.Tank != null) ? plr.Tank.ReflectionName : "";
                serialized.TeamId = (plr.Team != null) ? plr.Team.TeamId : (short)-3;
                serialized.Input = (plr.Tank != null ? plr.Tank.InputState : default(InputState));
                serialized.Username = plr.DisplayName;
                serialized.UsernameDisplayColor = plr.DisplayNameDrawColor;
                serialized.PlayerObject = plr;

                Players.Add(serialized);
            }
        }

        private void SetObjects(GameCore game)
        {
            foreach (var obj in game.GameObjects)
                ObjectStates.Add(new FullObjectState(obj.FullState));
        }


        public static FullGameState Read(NetIncomingMessage message)
        {
            var state = new FullGameState();
            state.MapInfo = ModAssetInfo.Decode(message.ReadBytes(message.ReadUInt16()));
            state.GamemodeReflectionName = message.ReadString();
            state.FriendlyFireEnabled = message.ReadBoolean();
            message.ReadPadBits();
            state.GamemodeState = message.ReadBytes(message.ReadInt32());
            state.Status = (GameCore.CurrentGameStatus)message.ReadByte();
            state.TimescaleString = message.ReadString();
            state.TimescaleValue = message.ReadDouble();
            state.CurrentGameTimeMilliseconds = message.ReadDouble();
            state.NextObjectId = message.ReadUInt16();

            var objCount = message.ReadInt32();
            for (var i = 0; i < objCount; i++)
            {
                state.ObjectStates.Add(new FullObjectState(message.ReadBytes(message.ReadInt32())));
            }

            var playersCount = message.ReadInt32();

            for (var i = 0; i < playersCount; i++)
            {
                var input = new InputState();
                var fsPlayer = new FullStatePlayer();
                fsPlayer.ClanName = message.ReadString();
                fsPlayer.Id = new Guid(message.ReadBytes(16));

                fsPlayer.HasSelectedTank = message.ReadBoolean();
                fsPlayer.HasTank = message.ReadBoolean();
                fsPlayer.IsAdmin = message.ReadBoolean();
                fsPlayer.IsPremium = message.ReadBoolean();
                fsPlayer.IsSpectator = message.ReadBoolean();
                fsPlayer.TankHasCustomStyle = message.ReadBoolean();
                input.FirePressed = message.ReadBoolean();
                message.ReadPadBits();

                input.LookDirection = message.ReadFloat();
                input.MovementSpeed = message.ReadFloat();
                input.RotationSpeed = message.ReadFloat();
                input.WeaponNumber = message.ReadByte();

                fsPlayer.SpawnPoint = new Microsoft.Xna.Framework.Vector2(
                    message.ReadFloat(), message.ReadFloat());

                fsPlayer.TankObjectId = message.ReadUInt16();
                fsPlayer.TankReflectionName = message.ReadString();

                fsPlayer.TeamId = message.ReadInt16();
                fsPlayer.Username = message.ReadString();
                fsPlayer.UsernameDisplayColor = new Microsoft.Xna.Framework.Color
                { PackedValue = message.ReadUInt32() };

                fsPlayer.Input = input;
            }

            var loadedModCount = message.ReadInt32();
            for (var i = 0; i < loadedModCount; i++)
            {
                state.GameLoadedMods.Add(ModInfo.Decode(message.ReadBytes(message.ReadUInt16())));
            }

            return state;
        }

        public void Write(NetOutgoingMessage message)
        {
            var encodedMapData = MapInfo.Encode();
            message.Write((ushort)encodedMapData.Length);
            message.Write(encodedMapData);
            message.Write(GamemodeReflectionName);
            message.Write(FriendlyFireEnabled);
            message.WritePadBits();
            message.Write(GamemodeState.Length);
            message.Write(GamemodeState);
            message.Write((byte)Status);
            message.Write(TimescaleString);
            message.Write(TimescaleValue);
            message.Write(CurrentGameTimeMilliseconds);
            message.Write(NextObjectId);

            message.Write(ObjectStates.Count);
            foreach (var obj in ObjectStates)
            {
                message.Write(obj.Data.Length);
                message.Write(obj.Data);
            }

            message.Write(Players.Count);
            foreach (var player in Players)
            {
                message.Write(player.ClanName);
                message.Write(player.Id.ToByteArray());

                message.Write(player.HasSelectedTank);
                message.Write(player.HasTank);
                message.Write(player.IsAdmin);
                message.Write(player.IsPremium);
                message.Write(player.IsSpectator);
                message.Write(player.TankHasCustomStyle);
                message.Write(player.Input.FirePressed); //input
                message.WritePadBits();

                //input
                message.Write(player.Input.LookDirection);
                message.Write(player.Input.MovementSpeed);
                message.Write(player.Input.RotationSpeed);

                message.Write((byte)player.Input.WeaponNumber);

                message.Write(player.SpawnPoint.X);
                message.Write(player.SpawnPoint.Y);

                message.Write(player.TankObjectId);
                message.Write(player.TankReflectionName);
                message.Write(player.TeamId);
                message.Write(player.Username);
                message.Write(player.UsernameDisplayColor.PackedValue);
            }

            message.Write(GameLoadedMods.Count);
            foreach (var mod in GameLoadedMods)
            {
                var encoded = mod.Encode();
                message.Write((ushort)encoded.Length);
                message.Write(encoded);
            }
        }
    }
}
