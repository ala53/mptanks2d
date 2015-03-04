using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public abstract class Gamemode
    {
        public static Gamemode[] AvailableGamemodes = { 
            //1v1 game modes
            new Gamemodes._1v1_5v5._1v1._1v1MeMateIRL() 
        };

        public abstract string Name { get; }

        public abstract string Description { get; }

        public int PlayerCount
        {
            get
            {
                int players = 0;
                foreach (var team in Teams)
                    players += team.PlayersCount;
                return players;
            }
        }

        public abstract Teams.Team[] Teams { get; }

        public abstract void Update(float deltaMs);

        /// <summary>
        /// Checks if the win conditions for the current game have been met.
        /// </summary>
        /// <param name="game">The game that us running this gamemode.</param>
        /// <returns></returns>
        public abstract bool HasGameEnded(Game game);

        /// <summary>
        /// If the game has been completed, this will be called to get which team won.
        /// </summary>
        /// <param name="game">The game that is running this gamemode.</param>
        /// <returns></returns>
        public abstract Teams.Team GetWinningTeam(Game game);
    }
}
