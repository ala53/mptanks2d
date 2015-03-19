using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5.Screens
{
    abstract class Screen
    {
        public Game Game { get; private set; }
        public Screen(Game _game)
        {
            Game =_game;
        }
    }
}
