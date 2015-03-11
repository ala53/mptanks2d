using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public class Team
    {
        private static Team _null = new Team();
        public static Team Null { get { return _null; } }
        public Tanks.Tank[] Tanks { get; internal set; }
        public string TeamName { get; internal set; }
        public Color TeamColor { get; internal set; }
        /// <summary>
        /// The team's goal, for an explanation to the players.
        /// </summary>
        public string Objective { get; internal set; }
    }
}
