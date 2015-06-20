using MPTanks.Engine;
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
            //Add the players and teams

            //Add all of the game objects
            foreach (var fullState in ObjectStates)
                GameObject.CreateAndAddFromSerializationInformation(game, fullState.Data, true);

            //Do this with reflection because we want to keep the api private
            typeof(GameCore).GetProperty(nameof(GameCore.TimeMilliseconds),
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.SetProperty).SetValue(game, CurrentGameTimeMilliseconds);

            if (latency > 0)
                game.Update(new Microsoft.Xna.Framework.GameTime(TimeSpan.Zero, TimeSpan.FromMilliseconds(latency)));

            return game;
        }

        public static FullGameState Create(GameCore game)
        {
            var state = new FullGameState();

            return state;
        }
    }
}
