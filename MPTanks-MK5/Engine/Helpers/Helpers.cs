using MPTanks.Engine.Assets;
using MPTanks.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Helpers
{
    public static partial class BasicHelpers
    {
        private static Random _rand = new Random();

        public static T ChooseRandom<T>(this T[] options)
        {
            var index = _rand.Next(0, options.Length);
            return options[index];
        }

    }
}
