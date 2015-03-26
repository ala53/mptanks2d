using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Gamemodes
{
    public class Team
    {
        private static Team _null = new Team() { TeamName = "Null" };
        public static Team Null { get { return _null; } }
        private static Team _tied = new Team() { TeamName = "Indeterminate" };
        /// <summary>
        /// The team to flag as a tie or when no one wins.
        /// </summary>
        public static Team Indeterminate { get { return _tied; } }
        public Player[] Players { get; internal set; }
        public string TeamName { get; internal set; }
        public Color TeamColor { get; internal set; }
        /// <summary>
        /// The team's goal, for an explanation to the players.
        /// </summary>
        public string Objective { get; internal set; }

        public struct Player
        {
            public Guid PlayerId;
            public Tanks.Tank Tank;
        }
    }
}
