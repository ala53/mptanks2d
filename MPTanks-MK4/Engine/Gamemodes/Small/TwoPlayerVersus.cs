using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes.Small
{
    class TwoPlayerVersus : Gamemode
    {
        public override string Name
        {
            get { return "1v1 me M8 IRL No Aimbot"; }
        }

        public override string Description
        {
            get { return "Do a 1 versus 1 fight with another player."; }
        }

        private Teams.Team[] _teams = {
            new Teams.Team() {
                TeamColor = Color.Red,
                Tanks = new[] {
                    new Teams.Team.TankDescriptor() 
                    {
                        AllowAnyTank = true
                    }
                }
            },
            new Teams.Team() {},
        };
        public override Teams.Team[] Teams
        {
            get { return _teams; }
        }

        public override void Update(float deltaMs)
        {
            throw new NotImplementedException();
        }

        public override bool HasGameEnded(Game game)
        {
            throw new NotImplementedException();
        }

        public override Teams.Team GetWinningTeam(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
