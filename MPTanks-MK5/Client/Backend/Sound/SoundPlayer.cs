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

        public GameCore Game { get; private set; }

        public Vector2 PlayerPosition
        {
            get
            {
                FMOD.VECTOR pos, vel, forward, up;
                FMOD.Error.Check(_system.get3DListenerAttributes(0, out pos, out vel, out forward, out up));
                return pos;
            }
            set
            {
                FMOD.VECTOR pos = value;
                FMOD.VECTOR vel = PlayerVelocity;
                FMOD.VECTOR forward = new FMOD.VECTOR { y = 1 };
                FMOD.VECTOR up = new FMOD.VECTOR { z = 1 };
                FMOD.Error.Check(_system.set3DListenerAttributes(0, ref pos, ref vel, ref forward, ref up));
            }
        }
        public Vector2 PlayerVelocity
        {
            get
            {
                FMOD.VECTOR pos, vel, forward, up;
                FMOD.Error.Check(_system.get3DListenerAttributes(0, out pos, out vel, out forward, out up));
                return vel;
            }
            set
            {
                FMOD.VECTOR pos = value;
                FMOD.VECTOR vel = PlayerVelocity;
                FMOD.VECTOR forward = new FMOD.VECTOR { y = 1 };
                FMOD.VECTOR up = new FMOD.VECTOR { z = 1 };
                FMOD.Error.Check(_system.set3DListenerAttributes(0, ref pos, ref vel, ref forward, ref up));
            }
        }

        public int ActiveChannels
        {
            get
            {
                int activeChannels;
                FMOD.Error.Check(_system.getChannelsPlaying(out activeChannels));
                return activeChannels;
            }
        }

        public int MaxChannels { get; private set; }

        #region diagnostics
        public DiagnosticInfo Diagnostics
        {
            get
            {
                var diag = new DiagnosticInfo();
                _system.getCPUUsage(
                    out diag.DSPCPU, out diag.StreamCPU, out diag.GeometryCPU,
                    out diag.UpdateCPU, out diag.TotalCPU);
                _system.getSoundRAM(
                    out diag.CurrentBytesAllocated, out diag.MaximumBytesAllocated,
                    out diag.TotalBytesAvaliable);
                return diag;
            }
        }
        public struct DiagnosticInfo
        {
            public float DSPCPU, StreamCPU, GeometryCPU, UpdateCPU, TotalCPU;
            public int CurrentBytesAllocated, MaximumBytesAllocated, TotalBytesAvaliable;
        }
        #endregion

        internal ActiveGameEffectContainer ActiveSounds { get; set; }
        internal SoundCache Cache { get; private set; }
        public SoundPlayer(GameCore game, int maxChannels = 256)
        {
            MaxChannels = maxChannels;

            FMOD.Error.Check(FMOD.Factory.System_Create(out _system));
            FMOD.Error.Check(_system.init(maxChannels, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));
            FMOD.Error.Check(_system.set3DNumListeners(1));
            FMOD.Error.Check(_system.createChannelGroup("Background Sounds", out _backgroundGroup));
            FMOD.Error.Check(_system.createChannelGroup("Voice Chat", out _voiceGroup));
            FMOD.Error.Check(_system.createChannelGroup("Sound Effects", out _effectsGroup));
            Game = game;
            ActiveSounds = new ActiveGameEffectContainer(this);
            Cache = new SoundCache(this);
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
                sound.Value.Speed = (float)Game.Timescale.Fractional;
            }

            ActiveSounds.UpdateSounds(gameTime);

            FMOD.Error.Check(_system.update());
        }
    }
}
