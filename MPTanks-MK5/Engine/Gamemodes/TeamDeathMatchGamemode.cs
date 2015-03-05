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

        public override Team WinningTeam
        {
            get { throw new NotImplementedException(); }
        }

        public override Team[] Teams
        {
            get { throw new NotImplementedException(); }
        }

        public override bool HasValidPlayerCount(int count)
        {
            //Make sure we have players and that the number of them is even
            return (count > 0 && count % 2 == 0);
        }

        public override void MakeTeams(Tanks.Tank[] tanks)
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
