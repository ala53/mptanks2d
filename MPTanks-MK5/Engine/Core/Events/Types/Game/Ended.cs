using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Events.Types.Game
{
    public class Ended
    {
        public Gamemodes.Team WinningTeam { get; set; }
        public bool GameIsDraw { get; set; }
        public DateTime EndTime { get; set; }
        public GameCore Game { get; set; }
    }
}
