using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Sound
{
    struct SoundInstance
    {
        public Sound Sound { get; set; }
        public SoundEffectInstance Instance { get; set; }
        public AudioEmitter Emitter { get; set; }

        public void Apply(Vector2 position, AudioListener listener)
        {
            Emitter.Position = new Vector3(position, 0);
            Emitter.Velocity = Vector3.Zero;
            Instance.Apply3D(listener, Emitter);
            var ins = new SoundEffect
        }
    }
}
