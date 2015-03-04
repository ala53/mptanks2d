using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManager
{
    class Settings
    {
        /// <summary>
        /// The maximum number of servers that the server manager can have running at once.
        /// </summary>
        public static readonly int MaxServers = 32;

        /// <summary>
        /// The amount of time allowed to wait for players before putting someone in a game.
        /// E.g. we can wait for players for 30 seconds to see if we can put them in a 30
        /// player battle but if not, we just put them in a smaller battle.
        /// If we go over the allotted time, all bets are off. We no longer guarantee the 
        /// type of tank you selected and choose a random game mode.
        /// 
        /// In fact, we may even match you against people of other skill levels if this happens.
        /// </summary>
        public static readonly int QueueAllowedTime = 60;

        /// <summary>
        /// How many percent of the players in a match can be AI controlled before we start
        /// agressively dropping down the size of battles.
        /// </summary>
        public static readonly float MaxAIAmount = 0.50f;

        /// <summary>
        /// The smallest battle size to have before using AI in the game.
        /// </summary>
        public static readonly int SmallestBattleSizeAIAllowed = 8;
    }
}
