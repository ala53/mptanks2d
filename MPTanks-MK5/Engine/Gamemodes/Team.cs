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
        private static Team _tied = new Team();
        /// <summary>
        /// The team to flag as a tie or when no one wins.
        /// </summary>
        public static Team Indeterminate { get { return _tied; } }
        public Tanks.Tank[] Tanks { get; internal set; }
        public string TeamName { get; internal set; }
        public Color TeamColor { get; internal set; }
        /// <summary>
        /// The team's goal, for an explanation to the players.
        /// </summary>
        public string Objective { get; internal set; }
    }
}
