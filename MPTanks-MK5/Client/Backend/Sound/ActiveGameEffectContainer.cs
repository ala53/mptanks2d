using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    class ActiveGameEffectContainer
    {
        private Dictionary<int, ActiveGameEffect> _cache = new Dictionary<int, ActiveGameEffect>();
        private SoundPlayer _player;

        public ActiveGameEffectContainer(SoundPlayer player)
        {
            _player = player;
        }
        public bool IsSoundPlaying(int uid)
        {
            return (_cache.ContainsKey(uid));
        }

        private void AddSounds()
        {
            foreach (var sound in _player.Game.SoundEngine.Sounds)
            {
                if (!_cache.ContainsKey(sound.UniqueId))
                {
                    _cache.Add(sound.UniqueId, new ActiveGameEffect(sound, _player) { Ended = EndedHook });
                }
            }
        }

        private List<int> _removeList = new List<int>();
        private void EndedHook(ActiveGameEffect effect)
        {
            _removeList.Add(effect.SoundObject.UniqueId);
        }

        private void ProcessRemoveList()
        {
            foreach (var i in _removeList)
                _cache.Remove(i);
            _removeList.Clear();
        }

        public void UpdateSounds(GameTime gameTime)
        {
            AddSounds();
            ProcessRemoveList();
            foreach (var sound in _cache)
                sound.Value.Update(gameTime);

        }
    }
}
