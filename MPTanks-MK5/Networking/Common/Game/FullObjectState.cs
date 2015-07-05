using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Helpers;

namespace MPTanks.Networking.Common.Game
{
    public struct FullObjectState
    {
        public byte[] Data;
        public ushort ObjectId;

        public string ReflectionName;

        public FullObjectState(byte[] data)
        {
            Data = data;
            ObjectId = data.GetValue<ushort>(0);
            ReflectionName = data.GetString(2);
        }
    }
}
