using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public class DeathMatchGamemode : Gamemode
    {

        public override bool GameEnded
        {
            get { throw new NotImplementedException(); }
        }

        public override Team[] Teams
        {
            get { throw new NotImplementedException(); }
        }

        public override bool HasValidPlayerCount(int tanksCount, int superTanksCount)
        {
            throw new NotImplementedException();
        }

        public override void MakeTeams(Tanks.Tank[] tanks, Tanks.SuperTank[] superTanks)
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
