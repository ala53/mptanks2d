using Microsoft.Xna.Framework;
using NetworkingCommon.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingCommon
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
        public GameState CurrentGameState { get { return _pooledGameState; } }
        public Engine.GameCore Game { get; private set; }
        public Engine.Diagnostics Diagnostics { get { return Game.Diagnostics; } }
        #endregion
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
        private GameState _pooledGameState = new GameState();
        private GameState _lastGameState = new GameState();
        private GameState _nextGameState = new GameState();
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
