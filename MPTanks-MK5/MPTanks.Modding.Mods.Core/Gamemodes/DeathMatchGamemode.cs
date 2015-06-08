using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Gamemodes;

namespace MPTanks.Modding.Mods.Core
{
    [Gamemode("DeathMatchGamemode", DisplayName = "Deathmatch (no teams)",
        Description = "2 team deathmatch", MinPlayersCount = 2)]
    public class DeathMatchGamemode : Gamemode
    {
        public DeathMatchGamemode()
            : base()
        {
            AllowRespawn = false;
            RespawnTimeMs = 0;
        }

        private bool _gameEnded;
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

        public override void MakeTeams(Engine.GamePlayer[] players)
        {
            var rnd = new Random();
            var teams = new List<Team>();
            var teamId = 1;

            foreach (var p in players)
            {
                var team = new Team
                {
                    Objective = "Kill all other players.",
                    Players = new[] { p },
                    TeamColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255)),
                    TeamId = teamId++,
                    TeamName = p.Name
                };

                teams.Add(team);
            }

            _teams = teams.ToArray();
        }
        
        public override string[] GetPlayerAllowedTankTypes(Engine.GamePlayer player)
        {
            return Engine.Tanks.Tank.GetAllTankTypes().ToArray();
        }

        public override bool SetPlayerTankType(Engine.GamePlayer player, string tankType)
        {
            if (Engine.Tanks.Tank.GetAllTankTypes().Contains(tankType))
            {
                player.SelectedTankReflectionName = tankType;
                return true;
            }
            return false;
        }

        public override void StartGame()
        {
        }

        public override void Update(GameTime gameTime)
        {
            int pCountAliveOnTeamRed = Teams[0].Players.Count((p) => p.Tank.Alive);
            int pCountAliveOnTeamBlue = Teams[1].Players.Count((p) => p.Tank.Alive);

            if (pCountAliveOnTeamRed > 0 && pCountAliveOnTeamBlue > 0)
                return; //game still running

            //Red team wins
            if (pCountAliveOnTeamRed > 0 && pCountAliveOnTeamBlue == 0)
            {
                _gameEnded = true;
                _winner = Teams[0];
                return;
            }

            //Blue team wins
            if (pCountAliveOnTeamRed == 0 && pCountAliveOnTeamBlue > 0)
            {
                _gameEnded = true;
                _winner = Teams[1];
                return;
            }

            //Tie
            if (pCountAliveOnTeamRed == 0 && pCountAliveOnTeamBlue == 0)
            {
                _gameEnded = true;
                _winner = Team.Indeterminate;
                return;
            }
        }
    }
}
