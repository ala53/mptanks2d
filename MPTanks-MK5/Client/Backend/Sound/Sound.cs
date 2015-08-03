using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public SoundEffect Effect { get; private set; }
        private __EffectInstance[] _instances;
        private class __EffectInstance
        {
            public bool InUse { get; set; }
            public SoundEffectInstance InstanceObject { get; set; }
            public AudioEmitter Emitter { get; set; }
            public DateTime ActivationTime { get; set; }
            public ActiveSoundObject User { get; set; }
        }
        public IEnumerable<SoundEffectInstance> Instances
        {
            get { foreach (var inst in _instances) yield return inst.InstanceObject; }
        }

        public Sound(int maxInstances, SoundEffect effect)
        {
            Effect = effect;
            _instances = new __EffectInstance[maxInstances];
            for (var i = 0; i < _instances.Length; i++)
            {
                _instances[i] = new __EffectInstance
                {
                    InstanceObject = Effect.CreateInstance(),
                    Emitter = new AudioEmitter() { Forward = Vector3.UnitY, Up = Vector3.UnitZ }
                };
            }
        }

        public void ReleaseInstance(SoundEffectInstance instance)
        {
            foreach (var inst in _instances)
                if (inst.InstanceObject == instance)
                {
                    inst.InUse = false;
                    return;
                }
        }

        public SoundInstance GetInstance(ActiveSoundObject consumer)
        {
            foreach (var instance in _instances)
                if (!instance.InUse)
                {
                    instance.InUse = true;
                    instance.User = consumer;
                    instance.ActivationTime = DateTime.Now;
                    return new SoundInstance { Instance = instance.InstanceObject, Emitter = instance.Emitter };
                }
            __EffectInstance oldest = null;
            foreach (var instance in _instances)
            {
                if (oldest == null) oldest = instance;
                else
                {
                    if ((DateTime.Now - instance.ActivationTime) > (DateTime.Now - oldest.ActivationTime))
                        oldest = instance;
                }
            }

            oldest.User.ForceDestroyBecauseSoundEffectInstanceReclaimed();
            oldest.InUse = true;
            oldest.User = consumer;
            oldest.ActivationTime = DateTime.Now;
            return new SoundInstance { Instance = oldest.InstanceObject, Emitter = oldest.Emitter };
        }
    }
}
