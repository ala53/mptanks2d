using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{

    class SoundInstance
    {
        private FMOD.CHANNEL_CALLBACK _callback;
        public SoundInstance(FMOD.Channel channel, Sound sound)
        {
            Sound = sound;
            Channel = channel;
            //We have to hold a reference or the GC will collect and crash everything
            _callback = (channelPtr, controlType, callbackType, data1, data2) =>
            {
                if (callbackType == FMOD.CHANNELCONTROL_CALLBACK_TYPE.END &&
                _endedCallback != null)
                {
                    _endedCallback(this);
                }
                return FMOD.RESULT.OK;
            };
            FMOD.Error.Check(Channel.setCallback(_callback));
        }

        public FMOD.Channel Channel { get; private set; }
        public Sound Sound { get; private set; }

        public FMOD.Sound SoundEffect => Sound.SoundEffect;

        private float? _startFrequency;
        public float Timescale
        {
            get
            {
                if (_startFrequency == null)
                {
                    float stFreq;
                    FMOD.Error.Check(Channel.getFrequency(out stFreq));
                    _startFrequency = stFreq;
                }

                float frequency;
                FMOD.Error.Check(Channel.getFrequency(out frequency));
                return frequency / _startFrequency.Value;
            }
            set
            {
                if (_startFrequency == null)
                {
                    float stFreq;
                    FMOD.Error.Check(Channel.getFrequency(out stFreq));
                    _startFrequency = stFreq;
                }
                FMOD.Error.Check(Channel.setFrequency(value * _startFrequency.Value));
            }
        }
        public bool Playing
        {
            get
            {
                bool paused;
                FMOD.Error.Check(Channel.getPaused(out paused));
                return !paused;
            }
            set
            {
                FMOD.Error.Check(Channel.setPaused(!value));
            }
        }

        public float Volume
        {
            get
            {
                float volume;
                FMOD.Error.Check(Channel.getVolume(out volume));
                return volume;
            }
            set
            {
                FMOD.Error.Check(Channel.setVolume(value));
            }
        }

        public Vector2 Position
        {
            get
            {
                FMOD.VECTOR position;
                FMOD.VECTOR velocity;
                FMOD.VECTOR alt_pan_pos;
                FMOD.Error.Check(Channel.get3DAttributes(out position, out velocity, out alt_pan_pos));
                return position;
            }
            set
            {
                FMOD.VECTOR position;
                FMOD.VECTOR velocity;
                FMOD.VECTOR alt_pan_pos;
                FMOD.Error.Check(Channel.get3DAttributes(out position, out velocity, out alt_pan_pos));
                position = value;
                FMOD.Error.Check(Channel.set3DAttributes(ref position, ref velocity, ref alt_pan_pos));
            }
        }

        public Vector2 Velocity
        {
            get
            {
                FMOD.VECTOR position;
                FMOD.VECTOR velocity;
                FMOD.VECTOR alt_pan_pos;
                FMOD.Error.Check(Channel.get3DAttributes(out position, out velocity, out alt_pan_pos));
                return velocity;
            }
            set
            {
                FMOD.VECTOR position;
                FMOD.VECTOR velocity;
                FMOD.VECTOR alt_pan_pos;
                FMOD.Error.Check(Channel.get3DAttributes(out position, out velocity, out alt_pan_pos));
                velocity = value;
                FMOD.Error.Check(Channel.set3DAttributes(ref position, ref velocity, ref alt_pan_pos));
            }
        }
        
        public int LoopCount
        {
            get
            {
                int loopCount;
                FMOD.Error.Check(Channel.getLoopCount(out loopCount));
                return loopCount;
            }
            set
            {
                FMOD.Error.Check(Channel.setLoopCount(value));
            }
        }

        public float Pitch
        {
            get
            {
                float pitch;
                FMOD.Error.Check(Channel.getPitch(out pitch));
                return pitch;
            }
            set
            {
                FMOD.Error.Check(Channel.setPitch(value));
            }
        }

        private Action<SoundInstance> _endedCallback;
        public Action<SoundInstance> Ended
        {
            get { return _endedCallback; }
            set { _endedCallback = value; }
        }

        public TimeSpan Time
        {
            get
            {
                uint pos;
                FMOD.Error.Check(Channel.getPosition(out pos, FMOD.TIMEUNIT.MS));
                return TimeSpan.FromMilliseconds(pos);
            }
            set
            {
                FMOD.Error.Check(Channel.setPosition((uint)value.TotalMilliseconds, FMOD.TIMEUNIT.MS));
            }
        }

    }
}
