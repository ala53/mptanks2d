using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public abstract class Gamemode
    {
        public GameCore Game { get; private set; }
        public abstract bool GameEnded { get; }
        public virtual Team WinningTeam { get { return Team.Null; } }
        public abstract Team[] Teams { get; }

        virtual internal void SetGame(GameCore game)
        {
            Game = game;
        }

        public abstract bool HasValidPlayerCount(int tanksCount, int superTanksCount);

        /// <summary>
        /// Puts all of the tanks on teams
        /// </summary>
        /// <param name="tanks"></param>
        public abstract void MakeTeams(Tanks.Tank[] tanks, Tanks.SuperTank[] superTanks);

        /// <summary>
        /// Lets the game mode run its internal logic for players
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
    }
}
