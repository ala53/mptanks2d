using Microsoft.Xna.Framework;
using PlayerIO.GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [RoomType("gameroom")]
    public class MultiplayerGame : Game<Player>
    {
        public override void GameStarted()
        {
            AddTimer(Tick, Settings.MSPerFrame);
            base.GameStarted();
        }
        protected void Tick()
        {
            Console.WriteLine("Tick");
            for (var i = 0; i < 100000000; i++) ;
        }
    }
}
