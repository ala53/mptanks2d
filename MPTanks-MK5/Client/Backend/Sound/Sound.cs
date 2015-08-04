using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    class Sound
    {
        private FMOD.Sound _sound;
        public FMOD.Sound SoundEffect => _sound;
        public SoundPlayer Player { get; private set; }

        public float Speed
        {
            get
            {
                float speed;
                FMOD.Error.Check(SoundEffect.getMusicSpeed(out speed));
                return speed;
            }
            set
            {
                FMOD.Error.Check(SoundEffect.setMusicSpeed(value));
            }
        }

        public TimeSpan Length
        {
            get
            {
                uint length;
                FMOD.Error.Check(SoundEffect.getLength(out length, FMOD.TIMEUNIT.MS));
                return TimeSpan.FromMilliseconds(length);
            }
        }

        public int ChannelCount
        {
            get
            {
                FMOD.SOUND_TYPE type;
                FMOD.SOUND_FORMAT format;
                int channels;
                int bits;
                FMOD.Error.Check(SoundEffect.getFormat(out type, out format, out channels, out bits));
                return channels;
            }
        }
        public int BitsPerSample
        {
            get
            {
                FMOD.SOUND_TYPE type;
                FMOD.SOUND_FORMAT format;
                int channels;
                int bits;
                FMOD.Error.Check(SoundEffect.getFormat(out type, out format, out channels, out bits));
                return bits;
            }
        }

        public Sound(SoundPlayer player, string file)
        {
            Player = player;

            FMOD.Error.Check(player.System.createSound(file,
                FMOD.MODE.CREATESTREAM | FMOD.MODE._2D | FMOD.MODE.NONBLOCKING | FMOD.MODE.IGNORETAGS,
                out _sound));
        }

        public class SoundInstance
        {
            public FMOD.Channel Channel;
            public Sound Sound { get; set; }

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
                set
                {
                    _endedCallback = value;
                    FMOD.Error.Check(Channel.setCallback(new FMOD.CHANNEL_CALLBACK(
                        (channelPtr, controlType, callbackType, data1, data2) =>
                        {
                            if (callbackType == FMOD.CHANNELCONTROL_CALLBACK_TYPE.END)
                                value(this);
                            return FMOD.RESULT.OK;
                        })));
                }
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
        public SoundInstance Play(SoundPlayer.ChannelGroup group, bool paused = false)
        {
            var result = new SoundInstance();
            result.Sound = this;

            FMOD.ChannelGroup playGroup;
            if (group == SoundPlayer.ChannelGroup.Background)
                playGroup = Player.BackgroundGroup;
            else if (group == SoundPlayer.ChannelGroup.Voice)
                playGroup = Player.VoiceGroup;
            else playGroup = Player.EffectsGroup;

            FMOD.Error.Check(Player.System.playSound(SoundEffect, playGroup, paused, out result.Channel));
            return result;
        }
    }
}
