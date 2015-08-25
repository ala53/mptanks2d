using Microsoft.Xna.Framework;
using MPTanks.Engine.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Modding;
using MPTanks.Engine.Sound;

namespace MPTanks.CoreAssets.Gamemodes
{
    [Gamemode("TeamDeathMatchGamemode", DisplayName = "Team Deathmatch",
        Description = "A free-for-all deathmatch", MinPlayersCount = 2)]
    public class TeamDeathMatchGamemode : Gamemode
    {
        public TeamDeathMatchGamemode()
            : base()
        {
            AllowRespawn = false;
            RespawnTimeMs = 0;
        }

        public override void MakeTeams(Engine.GamePlayer[] players)
        {
            players = ShufflePlayers(players);

            var team1 = new Team() { TeamColor = Color.Red, TeamId = 1, TeamName = "Red Team" };
            var team2 = new Team() { TeamColor = Color.Blue, TeamId = 2, TeamName = "Blue Team" };

            team1.Objective = "Kill all members of Blue Team";
            team2.Objective = "Kill all members of Red Team";

            team1.Players = players.Take(players.Length / 2).ToArray();
            team2.Players = players.Skip(players.Length / 2).Take(players.Length - (players.Length / 2)).ToArray();

            Teams = new[] { team1, team2 };
        }

        private Engine.GamePlayer[] ShufflePlayers(Engine.GamePlayer[] players)
        {
            var arrNew = new Engine.GamePlayer[players.Length];

            Random rnd = new Random();

            foreach (var p in players)
            {
                var saved = false;

                while (!saved)
                {
                    var index = rnd.Next(0, players.Length);
                    if (arrNew[index] == null)
                    {
                        arrNew[index] = p;
                        saved = true;
                    }
                }
            }

            return arrNew;
        }

        public override string[] GetPlayerAllowedTankTypes(Engine.GamePlayer player)
        {
            return Engine.Tanks.Tank.GetAllTankTypes().ToArray();
        }

        public override bool VerifyPlayerTankSelection(Engine.GamePlayer player, string tankType)
        {
            if (Engine.Tanks.Tank.GetAllTankTypes().Contains(tankType))
            {
                player.SelectedTankReflectionName = tankType;
                return true;
            }
            return false;
        }

        public override void Create()
        {
            MusicHelper.PlaySongs(Game, Assets.GetSongNames());
        }

        public override void Update(GameTime gameTime)
        {
            int pCountAliveOnTeamRed = Teams[0].Players.Count((p) => (p.Tank?.Alive).HasValue ? (p.Tank?.Alive).Value : false);
            int pCountAliveOnTeamBlue = Teams[1].Players.Count((p) => (p.Tank?.Alive).HasValue ? (p.Tank?.Alive).Value : false);

            if (pCountAliveOnTeamRed > 0 && pCountAliveOnTeamBlue > 0)
                return; //game still running

            //Red team wins
            if (pCountAliveOnTeamRed > 0 && pCountAliveOnTeamBlue == 0)
            {
                GameEnded = true;
                WinningTeam = Teams[0];
                return;
            }

            //Blue team wins
            if (pCountAliveOnTeamRed == 0 && pCountAliveOnTeamBlue > 0)
            {
                GameEnded = true;
                WinningTeam = Teams[1];
                return;
            }

            //Tie
            if (pCountAliveOnTeamRed == 0 && pCountAliveOnTeamBlue == 0)
            {
                GameEnded = true;
                WinningTeam = Team.Indeterminate;
                return;
            }
        }
    }
}
