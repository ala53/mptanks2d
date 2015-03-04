using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class GameEnded : EventArgs
    {
        public Engine.Gamemodes.Teams.Team WinningTeam { get; set; }
        public Engine.Players.Player[] SurvivingPlayers { get; set; }
    }
}
