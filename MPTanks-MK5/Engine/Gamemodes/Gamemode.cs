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
        private GameCore _game;
        public Gamemode(GameCore game)
        {
            _game = game;
        }
        public abstract bool GameEnded { get; }
        public abstract Team WinningTeam { get; }
        public abstract Team[] Teams { get; }

        public abstract bool HasValidPlayerCount(int count);

        /// <summary>
        /// Puts all of the tanks on teams
        /// </summary>
        /// <param name="tanks"></param>
        public abstract void MakeTeams(Tanks.Tank[] tanks);

        /// <summary>
        /// Lets the game mode run its internal logic for players
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
    }
}
