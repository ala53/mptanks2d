using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    public class Map
    {
        private GameCore _game;
        public static Map LoadMap(string mapData, GameCore game)
        {
            return new Map() { _game = game };
        }

        /// <summary>
        /// Gets a spawn position, by team
        /// </summary>
        /// <param name="teamIndex"></param>
        /// <returns></returns>
        public Vector2 GetSpawnPosition(int teamIndex)
        {
            return new Vector2(_game.SharedRandom.Next(0, 100), _game.SharedRandom.Next(0, 100));
        }
    }
}
