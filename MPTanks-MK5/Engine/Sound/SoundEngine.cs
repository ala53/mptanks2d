using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Sound
{
    public class SoundEngine
    {
        public Song Music { get; set; }
        private LinkedList<Sound> _sounds = new LinkedList<Sound>();
        public IEnumerable<Sound> Sounds { get { return _sounds; } }

        public Sound PlaySound(string soundName,
            float lengthMs = Sound.PlayToEnd,
            bool loop = false,
            Vector2 position = default(Vector2))
        {
            return null;
        }
    }
}
