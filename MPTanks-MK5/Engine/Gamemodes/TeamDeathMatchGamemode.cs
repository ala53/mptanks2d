using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public class TeamDeathMatchGamemode : Gamemode
    {
        private bool _gameEnded = false;
        public override bool GameEnded
        {
            get { return _gameEnded; }
        }
        private Team _winner = Team.Null;
        public override Team WinningTeam
        {
            get { return _winner; }
        }
        private Team[] _teams;
        public override Team[] Teams
        {
            get { return _teams; }
        }

        public TeamDeathMatchGamemode()
        {

        }
        public override bool HasValidPlayerCount(int tanksCount, int superTanksCount)
        {
            //Make sure we have players and that the number of them is even
            return (tanksCount > 0 && tanksCount % 2 == 0
                && superTanksCount % 2 == 0);
        }

        public override void MakeTeams(Tanks.Tank[] tanks, Tanks.SuperTank[] superTanks)
        {
            if (!HasValidPlayerCount(tanks.Length, superTanks.Length))
                throw new ArgumentException("number of tanks invalid");

            //We add teams like so:
            //First half of the players are on red team and second half are on team 2
            var tanksRed = tanks.Take(tanks.Length / 2)
                .Concat(superTanks.Take(superTanks.Length / 2))
                .ToArray();
            var tanksBlue = tanks.Skip(tanks.Length / 2).Take(tanks.Length / 2)
                .Concat(superTanks.Skip(superTanks.Length / 2).Take(superTanks.Length / 2))
                .ToArray();

            var redTeam = new Team();
            redTeam.Objective = "Kill all members of blue team";
            redTeam.TeamColor = Color.Red;
            redTeam.TeamName = "Red team";
            redTeam.Tanks = tanksRed;

            var blueTeam = new Team();
            redTeam.Objective = "Kill all members of red team";
            redTeam.TeamColor = Color.Blue;
            redTeam.TeamName = "Blue team";
            redTeam.Tanks = tanksBlue;

            //And place the teams in the publicly visible array
            _teams = new Team[2];
            _teams[0] = redTeam;
            _teams[1] = blueTeam;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_teams == null) return;

            var tanks = new HashSet<Tanks.Tank>(Game.AllTanks);
            bool blueAlive = false;
            bool redAlive = false;
            foreach (var member in _teams[0].Tanks) //Check if anyone on red team is alive
                if (tanks.Contains(member))
                {
                    redAlive = true;
                    break;
                }

            foreach (var member in _teams[1].Tanks) //Check if anyone on blue team is alive
                if (tanks.Contains(member))
                {
                    blueAlive = true;
                    break;
                }

            //Win condition possibilities
            if (blueAlive && redAlive)
                _winner = Team.Null;
            if (blueAlive && !redAlive)
                _winner = _teams[0];
            if (redAlive && !blueAlive)
                _winner = _teams[1];
            if (!redAlive && !blueAlive)
                _winner = Team.Indeterminate;

            //If either team is dead, end the game
            if (!blueAlive || !redAlive)
                _gameEnded = true;
        }
    }
}
