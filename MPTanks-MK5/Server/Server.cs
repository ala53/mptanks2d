using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private Engine.GameCore game = new Engine.GameCore(new Logger(), new Engine.Gamemodes.TeamDeathMatchGamemode(), "");
        public void Open()
        {
        }

        public void Update(GameTime gameTime)
        {

        }
        public void Close()
        {

        }
    }
}
