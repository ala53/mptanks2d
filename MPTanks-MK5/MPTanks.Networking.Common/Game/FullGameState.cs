using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Logging;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Game
{
    public class FullGameState
    {
        public List<FullObjectState> ObjectStates { get; set; }
        /// <summary>
        /// The raw map data for the current map
        /// </summary>
        public string MapData { get; set; }
        public string GamemodeReflectionName { get; set; }
        public float CurrentGameTimeMilliseconds { get; set; }
        public byte[] GamemodeState { get; set; }
        public List<FullStatePlayer> Players { get; set; }

        public GameCore CreateGameFromState(ILogger logger = null, EngineSettings settings = null, float latency = 0)
        {
            var game = new GameCore(logger ?? new NullLogger(), GamemodeReflectionName, MapData, true, settings);
            game.Gamemode.FullState = GamemodeState;

            //Add all of the game objects
            foreach (var fullState in ObjectStates)
                GameObject.CreateAndAddFromSerializationInformation(game, fullState.Data, true);

            //Do this with reflection because we want to keep the api private (set gaame time)
            typeof(GameCore).GetProperty(nameof(GameCore.TimeMilliseconds),
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.SetProperty).SetValue(game, CurrentGameTimeMilliseconds);

            foreach (var player in Players)
            {
                var nwPlayer = new NetworkPlayer();
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
                nwPlayer.Tank = (Engine.Tanks.Tank)(player.HasTank ? game.GameObjectsById[player.TankObjectId] : null);
                nwPlayer.Team = (player.TeamId != -3 ? FindTeam(game.Gamemode.Teams, player.TeamId) : null);

                game.AddPlayer(nwPlayer);
            }

            if (latency > 0)
                game.Update(new Microsoft.Xna.Framework.GameTime(TimeSpan.Zero, TimeSpan.FromMilliseconds(latency)));

            return game;
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

            state.SetPlayers((IEnumerable<NetworkPlayer>)game.Players);
            state.SetObjects(game);

            state.MapData = game.Map.ToString();
            state.CurrentGameTimeMilliseconds = game.TimeMilliseconds;
            state.GamemodeReflectionName = game.Gamemode.ReflectionName;
            state.GamemodeState = game.Gamemode.FullState;

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
                serialized.Username = plr.DisplayName;
                serialized.UsernameDisplayColor = plr.DisplayNameDrawColor;

                Players.Add(serialized);
            }
        }

        private void SetObjects(GameCore game)
        {
            foreach (var obj in game.GameObjects)
                ObjectStates.Add(new FullObjectState(obj.FullState));
        }
    }
}
