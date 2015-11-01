using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Helpers
{
    public class ByteArrayReader
    {
        public int Offset { get; set; }
        public byte[] Data { get; private set; }
        private ByteArrayReader()
        {
        }

        public byte ReadByte()
        {
            return Data[Offset++];
        }
        public byte[] ReadBytes()
        {
            return Data.GetByteArray(Offset);
        }
        public byte[] ReadBytes(int count)
        {
            return Data.GetByteArray(Offset, count);
        }
        public ushort ReadUShort()
        {
            var o = BitConverter.ToUInt16(Data, Offset);
            Offset += 2;
            return o;
        }
        public short ReadShort()
        {
            var o = BitConverter.ToInt16(Data, Offset);
            Offset += 2;
            return o;
        }
        public uint ReadUInt()
        {
            var o = BitConverter.ToUInt32(Data, Offset);
            Offset += 4;
            return o;
        }
        public int ReadInt()
        {
            var o = BitConverter.ToInt32(Data, Offset);
            Offset += 4;
            return o;
        }
        public ulong ReadULong()
        {
            var o = BitConverter.ToUInt64(Data, Offset);
            Offset += 8;
            return o;
        }
        public long ReadLong()
        {
            var o = BitConverter.ToInt64(Data, Offset);
            Offset += 8;
            return o;
        }
        public bool ReadBool()
        {
            return Data[Offset++] != 0;
        }
        public string ReadString()
        {
            var len = ReadUShort();
            var str = Encoding.UTF8.GetString(Data, Offset, len);
            Offset += len;
            return str;
        }

        public Vector2 ReadVector()
        {
            return new Vector2(ReadFloat(), ReadFloat());
        }
        public Vector2 ReadHalfVector()
        {
            return new HalfVector2 { PackedValue = ReadUInt() }.ToVector2();
        }

        public Color ReadColor()
        {
            return new Color { PackedValue = ReadUInt() };
        }

        public float ReadFloat()
        {
            var o = BitConverter.ToSingle(Data, Offset);
            Offset += 4;
            return o;
        }
        public double ReadDouble()
        {
            var o = BitConverter.ToDouble(Data, Offset);
            Offset += 8;
            return o;
        }

        public T ReadGeneric<T>()
        {
            //Validate header
            if (ReadBytes(SerializationHelpers.JSONSerializationBytes.Length) != 
                SerializationHelpers.JSONSerializationBytes)
                throw new Exception("Unexpected token! JSON serialization preamble not found");
            var str = ReadString();
            return JsonConvert.DeserializeObject<T>(str);
        }

        private static Stack<ByteArrayReader> _cache = new Stack<ByteArrayReader>();

        public static ByteArrayReader Get(byte[] bytes)
        {
            ByteArrayReader r;
            if (_cache.Count > 0)
                r = _cache.Pop();
            else r = new ByteArrayReader();
            r.Data = bytes;
            return r;
        }

        public static void Release(ByteArrayReader reader)
        {
            if (!_cache.Contains(reader))
                _cache.Push(reader);
        }
        public void Release() =>
            Release(this);
    }
}
