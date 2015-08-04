using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    public class SoundPlayer
    {
        private FMOD.System _system;
        public FMOD.System System => _system;

        private FMOD.ChannelGroup _backgroundGroup;
        public FMOD.ChannelGroup BackgroundGroup => _backgroundGroup;

        private FMOD.ChannelGroup _voiceGroup;
        public FMOD.ChannelGroup VoiceGroup => _voiceGroup;

        private FMOD.ChannelGroup _effectsGroup;
        public FMOD.ChannelGroup EffectsGroup => _effectsGroup;

        internal SoundCache Cache { get; private set; }
        public SoundPlayer(GameCore game)
        {
            FMOD.Error.Check(FMOD.Factory.System_Create(out _system));
            FMOD.Error.Check(_system.createChannelGroup("Background Sounds", out _backgroundGroup));
            FMOD.Error.Check(_system.createChannelGroup("Voice Chat", out _voiceGroup));
            FMOD.Error.Check(_system.createChannelGroup("Sound Effects", out _effectsGroup));
        }

        public enum ChannelGroup
        {
            Background,
            Voice,
            Effects
        }

        public void Update(GameTime gameTime)
        {
            foreach (var sound in Cache.Sounds)
            {
            }
        }
    }
}
