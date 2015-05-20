using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering.Lighting
{
    public class LightEngine
    {
        private HashSet<Light> _lights = new HashSet<Light>();
        public IEnumerable<Light> Lights { get { return _lights; } }

        public void AddLight(Light light)
        {
            _lights.Add(light);
        }

        public void RemoveLight(Light light, bool throwOnNotFound = false)
        {
            if (_lights.Contains(light))
                _lights.Remove(light);
            else if (throwOnNotFound) throw new KeyNotFoundException("Light not found");
        }
    }
}
