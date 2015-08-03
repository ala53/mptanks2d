using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    class ActiveSoundObject
    {
        public SoundInstance Instance { get; private set; }
        public Engine.Sound.Sound SoundObject { get; private set; }
        public void ForceDestroyBecauseSoundEffectInstanceReclaimed()
        {
            if (SoundObject.CompletionCallback != null)
                SoundObject.CompletionCallback(SoundObject);

        }

        public ActiveSoundObject(Engine.Sound.Sound sound, SoundPlayer player, AudioListener listener)
        {
            SoundObject = sound;
            Instance = player.Cache.GetSound(sound.AssetName).GetInstance(this);

        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
