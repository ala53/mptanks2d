using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Events.Types.Game
{
    public class Tick : EventArgs
    {
        public GameTime Time { get; set; }
        public GameCore Game { get; set; }
    }
}
