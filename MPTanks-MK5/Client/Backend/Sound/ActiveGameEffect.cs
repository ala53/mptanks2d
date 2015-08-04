using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    class ActiveGameEffect
    {
        public Sound.SoundInstance Instance { get; private set; }
        public Engine.Sound.Sound SoundObject { get; private set; }
        public Action<ActiveGameEffect> Ended { get; set; }

        public ActiveGameEffect(Engine.Sound.Sound sound, SoundPlayer player)
        {
            SoundObject = sound;
            Instance = player.Cache.GetSound(sound.AssetName).Play(SoundPlayer.ChannelGroup.Effects);
            Instance.Ended = EndedHook;
        }

        public void Update(GameTime gameTime)
        {
            Instance.Velocity = SoundObject.Velocity * (float)Instance.Sound.Player.Game.Timescale.Fractional;
            Instance.LoopCount = SoundObject.LoopCount;
            Instance.Pitch = SoundObject.Pitch;
            Instance.Playing = SoundObject.Playing;
            Instance.Volume = SoundObject.Volume;
            Instance.Playing = SoundObject.Playing;
            if (SoundObject.Positional)
                Instance.Position = SoundObject.Position;
            if (SoundObject.TimeDirty)
                Instance.Time = SoundObject.Time;
            SoundObject.UnsafeSetTime(Instance.Time); 
        }

        public void EndedHook(Sound.SoundInstance instance)
        {
            SoundObject.Engine.MarkSoundCompleted(SoundObject);

            if (Ended != null) Ended(this);
        }
    }
}
