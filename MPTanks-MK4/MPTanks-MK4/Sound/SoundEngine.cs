using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Sound
{
    class SoundEngine
    {
        private FMOD.System _soundSys;
        public void Initialize()
        {
            _(FMOD.Factory.System_Create(out _soundSys));
            _(_soundSys.init(5, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));
        }

        /// <summary>
        /// An internal FMOD assert function. It turns FMOD result errors into actual CLR errors.
        /// </summary>
        /// <param name="result"></param>
        private void _(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
                throw new Exception("FMOD Error: " + FMOD.Error.String(result));
        }

        public void Tick()
        {
            _(_soundSys.update());
        }

        public void PlaySound(Resources.Resource.Sound sound, Vector2 location)
        {

        }

        public void Close()
        {

        }
    }
}
