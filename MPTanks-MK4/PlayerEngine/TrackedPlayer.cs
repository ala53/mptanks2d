using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerEngine
{
    /// <summary>
    /// A class around basic player classes to allow for tracking win/loss ratios,
    /// kill/death ratios, etc.
    /// </summary>
    public class TrackedPlayer : Engine.Players.Player
    {
        public long KillsTotal { get; private set; }
        public long DeathsTotal { get; private set; }

        public long GamesWon { get; private set; }
        public long GamesLost { get; private set; }
    }
}
