using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    public class SoundPlayer : IDisposable
    {
        private FMOD.System _system;
        public FMOD.System System => _system;

        private FMOD.ChannelGroup _backgroundGroup;
        public FMOD.ChannelGroup BackgroundGroup => _backgroundGroup;

        private FMOD.ChannelGroup _voiceGroup;
        public FMOD.ChannelGroup VoiceGroup => _voiceGroup;

        private FMOD.ChannelGroup _effectsGroup;
        public FMOD.ChannelGroup EffectsGroup => _effectsGroup;

        public float EffectVolume
        {
            get
            {
                float vol;
                FMOD.Error.Check(_effectsGroup.getVolume(out vol));
                return (float)Math.Pow(vol, 1d / 2);
            }
            set
            {
                FMOD.Error.Check(_effectsGroup.setVolume((float)Math.Pow(value, 2)));
            }
        }

        public float BackgroundVolume
        {
            get
            {
                float vol;
                FMOD.Error.Check(_backgroundGroup.getVolume(out vol));
                return (float)Math.Pow(vol, 1d / 2);
            }
            set
            {
                FMOD.Error.Check(_backgroundGroup.setVolume((float)Math.Pow(value, 2)));
            }
        }

        public float VoiceVolume
        {
            get
            {
                float vol;
                FMOD.Error.Check(_voiceGroup.getVolume(out vol));
                return (float)Math.Pow(vol, 1d / 2);
            }
            set
            {
                FMOD.Error.Check(_voiceGroup.setVolume((float)Math.Pow(value, 2)));
            }
        }

        internal MusicHelper MusicPlayer { get; set; }

        private GameCore _game;
        public GameCore Game
        {
            get { return _game; }
            set
            {
                _game = value;
                MusicPlayer.SetGame(_game);
            }
        }

        internal List<SoundInstance> ActiveSoundInstanceQueue
        { get; private set; }
        = new List<SoundInstance>();
        public int ActiveSoundCount => ActiveSoundInstanceQueue.Count;

        public Vector2 PlayerPosition
        {
            [MethodImpl(MethodImplOptions.NoOptimization)]
            get
            {
                FMOD.VECTOR pos, vel, forward, up;
                FMOD.Error.Check(_system.get3DListenerAttributes(0, out pos, out vel, out forward, out up));
                return pos;
            }
            [MethodImpl(MethodImplOptions.NoOptimization)]
            set
            {
                FMOD.VECTOR pos;
                FMOD.VECTOR vel;
                FMOD.VECTOR forward;
                FMOD.VECTOR up;
                FMOD.Error.Check(_system.get3DListenerAttributes(0, out pos, out vel, out forward, out up));
                pos = value;
                up = Vector3.UnitZ;
                forward = -Vector2.UnitY;
                //No error checking because there are bugs that don't affect operation
                //Sometimes rounding errors cause the call to crash due to denormalization of the
                //forward and up vectors.
                _system.set3DListenerAttributes(0, ref pos, ref vel, ref forward, ref up);
            }
        }
        public Vector2 PlayerVelocity
        {
            [MethodImpl(MethodImplOptions.NoOptimization)]
            get
            {
                FMOD.VECTOR pos, vel, forward, up;
                FMOD.Error.Check(_system.get3DListenerAttributes(0, out pos, out vel, out forward, out up));
                return vel;
            }
            [MethodImpl(MethodImplOptions.NoOptimization)]
            set
            {
                FMOD.VECTOR pos;
                FMOD.VECTOR vel;
                FMOD.VECTOR forward;
                FMOD.VECTOR up;
                FMOD.Error.Check(_system.get3DListenerAttributes(0, out pos, out vel, out forward, out up));
                vel = value;
                up = Vector3.UnitZ;
                forward = -Vector2.UnitY;
                //No error checking because there are errors that don't affect operation
                //Sometimes rounding errors cause the call to crash due to denormalization of the
                //forward and up vectors.
                _system.set3DListenerAttributes(0, ref pos, ref vel, ref forward, ref up);
            }
        }

        public float SoundDistanceScale
        {
            get
            {
                float doppler, distance, rolloff;
                _system.get3DSettings(out doppler, out distance, out rolloff);
                return 1 / rolloff;
            }
            set
            {
                float doppler, distance, rolloff;
                _system.get3DSettings(out doppler, out distance, out rolloff);
                _system.set3DSettings(0, distance, 1 / value);
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
                FMOD.Error.Check(_system.getCPUUsage(
                    out diag.DSPCPU, out diag.StreamCPU, out diag.GeometryCPU,
                    out diag.UpdateCPU, out diag.TotalCPU));
                return diag;
            }
        }
        public struct DiagnosticInfo
        {
            public float DSPCPU, StreamCPU, GeometryCPU, UpdateCPU, TotalCPU;
        }
        #endregion

        internal ActiveGameEffectContainer ActiveSounds { get; set; }
        internal SoundCache Cache { get; private set; }
        public SoundPlayer(int maxChannels = 256)
        {
            MaxChannels = maxChannels;

            FMOD.Error.Check(FMOD.Factory.System_Create(out _system));
            FMOD.Error.Check(_system.init(maxChannels,
                FMOD.INITFLAGS.NORMAL | FMOD.INITFLAGS.PROFILE_METER_ALL |
                FMOD.INITFLAGS.PROFILE_ENABLE, IntPtr.Zero));
            FMOD.Error.Check(_system.set3DNumListeners(1));
            var settings = new FMOD.ADVANCEDSETTINGS();
            FMOD.Error.Check(_system.createChannelGroup("Background Sounds", out _backgroundGroup));
            FMOD.Error.Check(_system.createChannelGroup("Voice Chat", out _voiceGroup));
            FMOD.Error.Check(_system.createChannelGroup("Sound Effects", out _effectsGroup));
            SoundDistanceScale = 20;
            Cache = new SoundCache(this);
            ActiveSounds = new ActiveGameEffectContainer(this);
            MusicPlayer = new MusicHelper(this);
        }


        public enum ChannelGroup
        {
            Background,
            Voice,
            Effects
        }

        public void Update(GameTime gameTime)
        {
            ActiveSounds.UpdateSounds(gameTime);
            MusicPlayer.Update();
            FMOD.Error.Check(_system.update());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                foreach (var snd in Cache.Sounds)
                    FMOD.Error.Check(snd.Value.SoundEffect.release());

                _system.release();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~SoundPlayer()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
