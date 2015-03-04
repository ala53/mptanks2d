using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GameStates
{
    public class GameState
    {

        public bool IsDelta { get; set; }
        /// <summary>
        /// Reads a game state from a network stream.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static GameState ReadFromNetworkStream(Lidgren.Network.NetIncomingMessage message)
        {
            //TODO
            return null;   
        }

        /// <summary>
        /// Writes the current game state data to the network stream.
        /// </summary>
        /// <param name="message"></param>
        public void WriteToNetworkStream(Lidgren.Network.NetOutgoingMessage message)
        {
            //TODO
        }

        /// <summary>
        /// Takes the previous known game state and immutably applies changes to create a new full
        /// game state from a partial one.
        /// </summary>
        /// <param name="previous">The current game state.</param>
        /// <returns></returns>
        public GameState MakeFullState(GameState previous)
        {
            //TODO
            var newGs = new GameState();

            return newGs;
        }

        /// <summary>
        /// Takes 2 full game states and immutably creates a third partial state for network transmission.
        /// </summary>
        /// <param name="previous">The previous known game state.</param>
        /// <returns></returns>
        public GameState MakeDelta(GameState previous)
        {
            //TODO
            var newGs = new GameState();

            return newGs;
        }
    }
}
