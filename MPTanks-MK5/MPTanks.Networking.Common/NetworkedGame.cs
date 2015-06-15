using Microsoft.Xna.Framework;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Logging;
using MPTanks.Networking.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    /// <summary>
    /// A networked game. It contains the core properties and methods that
    /// both the server host and the client have access to.
    /// </summary>
    public class NetworkedGame
    {
        #region Properties
        /// <summary>
        /// The max frame rate to run the internal tick counter at
        /// </summary>
        public int MaxFramesPerSecond { get; set; }
        public FullGameState CurrentGameState { get { return _currentGameState; } }
        public Engine.GameCore Game { get; private set; }
        public Engine.Diagnostics Diagnostics { get { return Game.Diagnostics; } }
        public bool Authoritative { get { return Game.Authoritative; } }
        #endregion

        public NetworkedGame(bool authoritative, Gamemode gamemode, string mapData, Engine.Settings.EngineSettings settings = null)
        {
            Game = new Engine.GameCore(new NullLogger(), gamemode, mapData, !authoritative, settings);
            Game.Authoritative = authoritative;
        }

        #region Timing Management
        private double totalMilliseconds;
        private GameTime _gt = new GameTime();
        public void Tick(float milliseconds)
        {
            totalMilliseconds += milliseconds;
            _gt.ElapsedGameTime = TimeSpan.FromMilliseconds(milliseconds);
            _gt.TotalGameTime = TimeSpan.FromMilliseconds(totalMilliseconds);
            Tick(_gt);
        }
        public virtual void Tick(GameTime gameTime)
        {
            totalMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
            TickGameState(gameTime);
        }
        #endregion
        #region Game state ticking
        private FullGameState _currentGameState = new FullGameState();
        private FullGameState _lastGameState = new FullGameState();
        private FullGameState _nextGameState = new FullGameState();
        private void TickGameState(GameTime gameTime)
        {
            Diagnostics.BeginMeasurement("TickGameState()", "Network Core");
            //We interpolate the game states for the *lovely* advantage of CPU usage 
            //and pool the game states for performance and memory allocations

            Diagnostics.EndMeasurement("TickGameState()", "Network Core");
        }
        #endregion
    }
}
