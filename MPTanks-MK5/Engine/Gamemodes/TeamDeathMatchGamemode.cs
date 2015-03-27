using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Gamemodes
{
    [Modding.Gamemode(AllowSuperTanks = true, Name = "Team deathmatch", Description = "1v1 to 20v20 TDM gamemode")]
    public class TeamDeathMatchGamemode : Gamemode
    {
        private bool _gameEnded = false;
        public override bool GameEnded
        {
            get { return _gameEnded; }
        }

        private Team[] _teams = new Team[2];
        public override Team[] Teams
        {
            get { return _teams; }
        }

        private Team _winner = Team.Null;
        public override Team WinningTeam
        {
            get
            {
                return _winner;
            }
        }

        public override int MinPlayerCount
        {
            get { return 2; }
        }

        public static string ReflectionTypeName
        {
            get
            {
                return "TeamDeathMatchGamemode";
            }
        }

        static TeamDeathMatchGamemode()
        {
            Gamemode.RegisterType<TeamDeathMatchGamemode>();
        }

        /// <summary>
        /// The ratio of super tanks to normal tanks in this specific instance
        /// </summary>
        public float SuperTankRatio { get; set; }
        public override void MakeTeams(Guid[] playerIds)
        {
            _teams[0] = new Team(); //Blue team
            _teams[1] = new Team(); //Red team

            _teams[0].Objective = "Kill all members of red team";
            _teams[1].Objective = "Kill all members of blue team";

            _teams[0].TeamColor = Color.Blue;
            _teams[1].TeamColor = Color.Red;

            _teams[0].TeamName = "Blue team";
            _teams[1].TeamName = "Red team";

            var players = new List<Team.Player>();
            for (var i = 0; i < playerIds.Length / 2; i++)
                players.Add(new Team.Player() { PlayerId = playerIds[i] });

            _teams[0].Players = players.ToArray(); // blue team
            players.Clear();

            for (var i = playerIds.Length / 2; i < playerIds.Length; i++)
                players.Add(new Team.Player() { PlayerId = playerIds[i] });

            _teams[1].Players = players.ToArray(); // red team
        }
        public override PlayerTankType GetAssignedTankType(Guid playerId)
        {
            return PlayerTankType.BasicTank;
        }

        public override void StartGame()
        {
            //No complex initialization here
        }

        public override void Update(GameTime gameTime)
        {
            var blueTeamAlive = false;
            var redTeamAlive = false;
            foreach (var player in Teams[0].Players)
                if (player.Tank.Alive)
                {
                    blueTeamAlive = true;
                    break;
                }
            foreach (var player in Teams[1].Players)
                if (player.Tank.Alive)
                {
                    redTeamAlive = true;
                    break;
                }

            if (!redTeamAlive && !blueTeamAlive)
            {
                _gameEnded = true;
                _winner = Team.Indeterminate; //A tie
            }
            else if (!blueTeamAlive)
            {
                _gameEnded = true;
                _winner = _teams[1]; //Red team won
                return;
            }
            else if (!redTeamAlive)
            {
                _gameEnded = true;
                _winner = _teams[0]; //Blue team won
            }
        }

        public override void ReceiveStateData(byte[] data)
        {
            base.ReceiveStateData(data);
        }

    }
}
