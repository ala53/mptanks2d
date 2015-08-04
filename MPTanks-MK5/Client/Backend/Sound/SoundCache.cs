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
        private SoundPlayer _player;
        public SoundCache(SoundPlayer player)
        {
            _player = player;
        }

        public Sound GetSound(string assetName)
        {
            if (!_soundCache.ContainsKey(assetName))
                _soundCache.Add(assetName, new Sound(_player, assetName));
            return _soundCache[assetName];
        }
    }
}
