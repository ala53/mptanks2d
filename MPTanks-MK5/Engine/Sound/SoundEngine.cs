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
        private LinkedList<Sound> _sounds = new LinkedList<Sound>();
        public IEnumerable<Sound> Sounds { get { return _sounds; } }

        public Sound PlaySound(string soundName, Sound.SoundPositioning positioning,
            float beginningOffsetMs = Sound.Beginning, float endOffsetMs = Sound.PlayToEnd,
            Vector2 position = default(Vector2), Sound.SoundRepeat repeat = Sound.SoundRepeat.NoRepeat)
        {
            return null;
        }
    }
}
