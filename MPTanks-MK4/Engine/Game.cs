using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Game
    {
        /// <summary>
        /// The framerate to run the simulation at.
        /// </summary>
        public static double Framerate { get; set; }

        /// <summary>
        /// The number of milliseconds between ticks.
        /// </summary>
        public static double TimePerFrame
        {
            get { return Framerate / 1000; }
            set { Framerate = 1000 / TimePerFrame; }
        }

        private GameStates.GameState previousState;
        private GameStates.GameState nextState;
        public GameStates.GameState CurrentState { get; private set; }

        /// <summary>
        /// Updates the game to use the provided game state data.
        /// </summary>
        /// <param name="state"></param>
        public void ApplyGameState(GameStates.GameState state)
        {

        }

        /// <summary>
        /// Ticks the game to the next frame. This method does some
        /// framerate compensation, skipping ticks and jumping time a
        /// bit to handle lag of excessive calls.
        /// </summary>
        public void Tick()
        {

        }
    }
}
