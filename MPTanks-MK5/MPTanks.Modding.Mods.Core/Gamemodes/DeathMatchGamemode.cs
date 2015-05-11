using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Gamemodes;

namespace MPTanks.Modding.Mods.Core
{
    [MPTanks.Modding.Gamemode("DeathMatchGamemode", AllowSuperTanks = true, DisplayName = "Deathmatch (no teams)", 
        Description = "2 team deathmatch", MinPlayersCount = 2)]
    public class DeathMatchGamemode : Gamemode
    {
        private bool _gameEnded = false;
        public override bool GameEnded
        {
            get { return _gameEnded; }
        }

        private Team[] _teams;
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

        /// <summary>
        /// The ratio of super tanks to normal tanks in this specific instance
        /// </summary>
        public float SuperTankRatio { get; set; }
        public override void MakeTeams(Guid[] playerIds)
        {
            _teams = new Team[playerIds.Length];

            for (var i = 0; i < _teams.Length; i++)
            {
                _teams[i] = new Team();
                _teams[i].Objective = "Kill all other players.";
                _teams[i].TeamColor = GetRandomColor();
                _teams[i].TeamName = "Player #" + i;

                _teams[i].Players = new[] { new Team.Player() { PlayerId = playerIds[i] } };
            }

        }

        private Random rand = new Random();
        private Color GetRandomColor()
        {
            var r = (float)(rand.NextDouble() / 2 + 0.5);
            var g = (float)(rand.NextDouble() / 2 + 0.5);
            var b = (float)(rand.NextDouble() / 2 + 0.5);

            return new Color(r, g, b, 1);
        }

        public override void StartGame()
        {
            //No complex initialization here
        }

        public override void Update(GameTime gameTime)
        {
            Team winner = null;
            int teamsAlive = 0;
            foreach (var team in Teams)
                if (team.Players[0].Tank.Alive)
                {
                    winner = team;
                    teamsAlive++;
                }

            if (teamsAlive == 0)
            {
                _gameEnded = true;
                _winner = Team.Indeterminate;
            }
            if (teamsAlive == 1)
            {
                _gameEnded = true;
                _winner = winner;
            }
        }

        public override void ReceiveStateData(byte[] data)
        {
            base.ReceiveStateData(data);
        }


        public override string[] GetPlayerAllowedTankTypes(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public override bool SetPlayerTankType(Guid playerId, string tankType)
        {
            throw new NotImplementedException();
        }
    }
}
