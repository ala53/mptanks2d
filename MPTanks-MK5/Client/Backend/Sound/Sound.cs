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
                FMOD.MODE._3D_WORLDRELATIVE | FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE,
                out _sound));
            
        }

        private static Queue<SoundInstance> _activeInstances = new Queue<SoundInstance>();
        public SoundInstance Play(SoundPlayer.ChannelGroup group, bool paused = false)
        {

            if (Player.ActiveChannels >= Player.MaxChannels - 1)
            {
                var oldest = _activeInstances.Dequeue();
                oldest.Channel.stop();
                if (oldest != null)
                    oldest.Ended(oldest);
            }

            FMOD.ChannelGroup playGroup;
            if (group == SoundPlayer.ChannelGroup.Background)
                playGroup = Player.BackgroundGroup;
            else if (group == SoundPlayer.ChannelGroup.Voice)
                playGroup = Player.VoiceGroup;
            else playGroup = Player.EffectsGroup;

            FMOD.Channel channel;
            FMOD.Error.Check(Player.System.playSound(SoundEffect, playGroup, paused, out channel));
            var result = new SoundInstance(channel, this);
            _activeInstances.Enqueue(result);
            return result;
        }
    }
}
