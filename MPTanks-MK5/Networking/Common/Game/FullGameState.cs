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
        public bool HasStarted { get; set; }
        public double CurrentGameTimeMilliseconds { get; set; }
        public string TimescaleString { get; set; }
        public double TimescaleValue { get; set; }
        public bool FriendlyFireEnabled { get; set; }
        public GameCore.CurrentGameStatus Status { get; set; }
        public byte[] GamemodeState { get; set; }
        public List<FullStatePlayer> Players { get; set; } = new List<FullStatePlayer>();
        public ushort NextObjectId { get; set; }
        public double GameEndedTime { get; set; }

        public GameCore CreateGameFromState(ILogger logger = null, EngineSettings settings = null, float latency = 0)
        {
            var game = new GameCore(logger ?? new NullLogger(), GamemodeReflectionName, MapInfo, settings);

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
            typeof(GameCore).GetProperty(nameof(GameCore.Status))
                .SetValue(game, Status);
            //Do this with reflection again because we want to keep the api private (set game time)
            typeof(GameCore).GetProperty(nameof(GameCore.Time))
                .SetValue(game, TimeSpan.FromMilliseconds(CurrentGameTimeMilliseconds));
            //Once again
            typeof(GameCore).GetProperty(nameof(GameCore.HasStarted))
                .SetValue(game, HasStarted);
            //A fourth time, keep the API private
            typeof(GameCore).GetField("_nextObjectId",
                System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.SetField |
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(game, NextObjectId);
            //And a fifth
            typeof(GameCore).GetField("_gameEndedTime",
                System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.SetField |
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(game, TimeSpan.FromMilliseconds(GameEndedTime));

            foreach (var player in Players)
            {
                var nwPlayer = new NetworkPlayer();
                if (game.PlayersById.ContainsKey(player.Id) && game.PlayersById[player.Id] is NetworkPlayer)
                    nwPlayer = (NetworkPlayer)game.PlayersById[player.Id];
                else if (game.PlayersById.ContainsKey(player.Id)) //remove because it isn't a networkplayer
                    game.RemovePlayer(player.Id);

                player.Apply(nwPlayer);

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

            ApplyDestruction(game);

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
                player.ApplySecondPass(player.PlayerObject, game);

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

        public void ApplyDestruction(GameCore game)
        {
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
        }

        public static FullGameState Create(GameCore game)
        {
            var state = new FullGameState();

            state.SetPlayers(game.Players.Select(x => x as NetworkPlayer).Where(a => a != null).ToList());
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
            state.HasStarted = game.HasStarted;
            state.GamemodeState = game.Gamemode.FullState;
            state.Status = game.Status;
            state.TimescaleString = game.Timescale.DisplayString;
            state.TimescaleValue = game.Timescale.Fractional;
            state.FriendlyFireEnabled = game.FriendlyFireEnabled;
            state.NextObjectId = (ushort)typeof(GameCore).GetField("_nextObjectId",
                System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(game);
            state.GameEndedTime = ((TimeSpan)(typeof(GameCore).GetField("_gameEndedTime",
                System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(game)))
                .TotalMilliseconds;

            return state;
        }

        private void SetPlayers(IEnumerable<NetworkPlayer> players)
        {
            foreach (var plr in players)
                Players.Add(new FullStatePlayer(plr));
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
            state.HasStarted = message.ReadBoolean();
            message.ReadPadBits();
            state.GamemodeState = message.ReadBytes(message.ReadInt32());
            state.Status = (GameCore.CurrentGameStatus)message.ReadByte();
            state.TimescaleString = message.ReadString();
            state.TimescaleValue = message.ReadDouble();
            state.CurrentGameTimeMilliseconds = message.ReadDouble();
            state.NextObjectId = message.ReadUInt16();
            state.GameEndedTime = message.ReadDouble();

            var objCount = message.ReadInt32();
            for (var i = 0; i < objCount; i++)
            {
                state.ObjectStates.Add(new FullObjectState(message.ReadBytes(message.ReadInt32())));
            }

            var playersCount = message.ReadInt32();

            for (var i = 0; i < playersCount; i++)
            {
                state.Players.Add(FullStatePlayer.Read(message));
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
            message.Write(HasStarted);
            message.WritePadBits();
            message.Write(GamemodeState.Length);
            message.Write(GamemodeState);
            message.Write((byte)Status);
            message.Write(TimescaleString);
            message.Write(TimescaleValue);
            message.Write(CurrentGameTimeMilliseconds);
            message.Write(NextObjectId);
            message.Write(GameEndedTime);

            message.Write(ObjectStates.Count);
            foreach (var obj in ObjectStates)
            {
                message.Write(obj.Data.Length);
                message.Write(obj.Data);
            }

            message.Write(Players.Count);
            foreach (var player in Players)
                player.Write(message);

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
