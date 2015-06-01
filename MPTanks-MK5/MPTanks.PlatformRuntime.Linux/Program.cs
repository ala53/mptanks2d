using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.PlatformRuntime.Linux
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Clients.GameClient.Menus.ClientCore())
                game.Run();
        }
    }
}
