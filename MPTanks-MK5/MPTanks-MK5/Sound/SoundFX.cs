using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Sound
{
    class SoundFX
    {
        #region AudioEmitter Cache
        private Queue<AudioEmitter> _emitters;
        #endregion
        private SoundEffect _fx;
        #region SoundEffectInstance Cache
        private SoundEffectInstance[] _instances;
        private Queue<SoundEffectInstance> _instancesByActivationTime;
        #endregion

        public void CreateInstances()
        {
            if (_instances == null)
            {
                _instances = new SoundEffectInstance[ClientSettings.MaxInstancesOfOneSoundAllowed];
                _instancesByActivationTime = new Queue<SoundEffectInstance>(
                    ClientSettings.MaxInstancesOfOneSoundAllowed);

                for (var i = 0; i < ClientSettings.MaxInstancesOfOneSoundAllowed; i++)
                {
                    _instances[i] = _fx.CreateInstance();
                    _instancesByActivationTime.Enqueue(_instances[i]);
                }
                //And create emitters
                for (var i = 0; i < ClientSettings.MaxInstancesOfOneSoundAllowed; i++)
                    _emitters.Enqueue(new AudioEmitter());
            }
        }

        public SoundEffectInstance GetInstance()
        {
            //find the oldest sound (has had getinstance called least recently)
            SoundEffectInstance instance = _instancesByActivationTime.Dequeue();
            //Clear the state
            instance.Stop();
            //mark it as most recently used by requeueing
            _instancesByActivationTime.Enqueue(instance);
            //and return
            return instance;
        }

        public AudioEmitter GetEmitter()
        {
            //Get the emitter
            AudioEmitter emitter = _emitters.Dequeue();
            //Mark it as most recently used
            _emitters.Enqueue(emitter);
            //And clear the state
            emitter.Velocity = Vector3.Zero;
            emitter.Up = Vector3.UnitZ;
            emitter.Position = Vector3.Zero;
            emitter.Forward = Vector3.UnitY;
            //And return
            return emitter;
        }

        public SoundEffectInstance Play(Vector2 position, AudioListener listener)
        {
            //Get the necessary instances
            var instance = GetInstance();
            var emitter = GetEmitter();
            //Set up positioning
            emitter.Position = new Vector3(position, 0);
            //Apply
            instance.Apply3D(listener, emitter);
            //And return
            return instance;
        }
    }
}
