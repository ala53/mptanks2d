using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Helpers
{
    public static partial class SerializationHelpers
    {

        const int JSONSerializedMagicNumber = unchecked(0x1337FCED);
        public static readonly byte[] JSONSerializationBytes = BitConverter.GetBytes(JSONSerializedMagicNumber);

        const int stringSerializedMagicNumber = unchecked(0x1338E3EE);
        public static readonly byte[] StringSerializationBytes = BitConverter.GetBytes(stringSerializedMagicNumber);

        public static readonly byte[] EmptyByteArray = new byte[0];
    }
}
