using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using MPTanks.Modding;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Sound;
using MPTanks.Engine;
using MPTanks.Engine.Tanks;

namespace MPTanks.CoreAssets.Gamemodes
{
    [Gamemode("DeathMatchGamemode", DisplayName = "Deathmatch (no teams)",
        Description = "2 team deathmatch", MinPlayersCount = 2, HotJoinEnabled = true)]
    public class DeathMatchGamemode : Gamemode
    {
        public DeathMatchGamemode()
            : base()
        {
            AllowRespawn = false;
            RespawnTimeMs = 0;
        }

        public override void Create()
        {
            MusicHelper.PlaySongs(Game, Assets.GetSongNames());
        }

        public override void MakeTeams(Engine.GamePlayer[] players)
        {
            var rnd = new Random();
            var teams = new List<Team>();
            short teamId = 1;

            foreach (var p in players)
            {
                var team = new Team
                {
                    Objective = "Kill all other players.",
                    Players = new[] { p },
                    TeamColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255)),
                    TeamId = teamId++,
                    TeamName = p.Username
                };

                teams.Add(team);
            }

            Teams = teams.ToArray();
        }

        public override string[] GetPlayerAllowedTankTypes(Engine.GamePlayer player)
        {
            return Engine.Tanks.Tank.GetAllTankTypes().ToArray();
        }

        public override bool CheckPlayerTankSelectionValid(Engine.GamePlayer player, string tankType)
        {
            if (Engine.Tanks.Tank.GetAllTankTypes().Contains(tankType))
            {
                player.SelectedTankReflectionName = tankType;
                return true;
            }
            return false;
        }

        private bool IsAlive(Team team)
        {
            if (team.Players.Length == 0) return false;
            if (team.Players[0].Tank == null) return false;
            return team.Players[0].Tank.Alive;
        }
        public override void Update(GameTime gameTime)
        {
            int pCountAlive = Teams.Count((t) => IsAlive(t));

            if (pCountAlive > 1)
                return; //still running

            if (pCountAlive == 1)
            {
                GameEnded = true;
                WinningTeam = Teams.First((t) => IsAlive(t));
                return;
            }

            if (pCountAlive == 0)
            {
                GameEnded = true;
                WinningTeam = Team.Indeterminate;
                return;
            }
        }
        #region Hot join implementation
        public override bool HotJoinCanPlayerJoin(GamePlayer player)
        {
            return true;
        }
        public override Team HotJoinGetPlayerTeam(GamePlayer player)
        {
            var teams = Teams.ToList();
            teams.Add(new Team());
            Teams = teams.ToArray();
            return Teams.Last();
        }
        public override string[] HotJoinGetAllowedTankTypes(GamePlayer player)
        {
            return Tank.GetAllTankTypes().ToArray();
        }

        public override bool HotJoinCheckPlayerSelectionValid(GamePlayer player, string tankType)
        {
            return (Tank.GetAllTankTypes().Contains(tankType));
        }
        #endregion
    }
}
