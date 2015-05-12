using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public class Server
    {
        private MPTanks.Engine.GameCore game = new MPTanks.Engine.GameCore(new Logger(), MPTanks.Engine.Gamemodes.Gamemode.ReflectiveInitialize("TeamDeathMatchGamemode"), "");
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
