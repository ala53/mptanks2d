using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public class Platoon
    {
        public Tanks.Tank[] Members { get; set; }
        public int Size { get { return Members.Length; } }
        public string PlatoonName { get; set; }
    }
}
