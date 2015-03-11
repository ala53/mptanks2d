using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public class TeamDeathMatchGamemode : Gamemode
    {
        public override bool GameEnded
        {
            get { throw new NotImplementedException(); }
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
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
