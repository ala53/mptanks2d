using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    class SoundCache
    {
        private Dictionary<string, Sound> _soundCache = new Dictionary<string, Sound>();
        public IReadOnlyDictionary<string, Sound> Sounds => _soundCache;
        public Sound GetSound(string assetName)
        {
            return null;
        }
    }
}
