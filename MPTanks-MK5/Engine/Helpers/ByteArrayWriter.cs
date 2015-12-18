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
    public class ByteArrayWriter
    {
        public int Offset { get; set; }
        public int Size { get; set; }
        public byte[] Data
        {
            get
            {
                var newArr = new byte[Size];
                Array.Copy(_data, 0, newArr, 0, Size);
                return newArr;
            }
        }
        private byte[] _data = new byte[4];
        /// <summary>
        /// Gets the backing array, which will almost always be larger than the actual data.
        /// <see cref="Data"/> will always return a perfectly sized array for the contents.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBackingArray() => _data;
        private ByteArrayWriter()
        {
        }

        private void EnsureSize(int amount)
        {
            Size = Math.Max(Size, Offset + amount);
            if (Offset + amount >= _data.Length)
            {
                int amountToResize = _data.Length * 2;
                while (Offset + amount >= amountToResize)
                    amountToResize *= 2;
                Array.Resize(ref _data, amountToResize);
            }
        }

        public void Write(bool value)
        {
            EnsureSize(1);
            _data[Offset++] = value ? (byte)1 : (byte)0;
        }
        public void Write(byte value)
        {
            EnsureSize(1);
            _data[Offset++] = value;
        }
        public void Write(sbyte value)
        {
            EnsureSize(1);
            _data[Offset++] = unchecked((byte)value);
        }
        public void Write(short value)
        {
            EnsureSize(2);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 2);
            Offset += 2;
        }
        public void Write(ushort value)
        {
            EnsureSize(2);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 2);
            Offset += 2;
        }
        public void Write(int value)
        {
            EnsureSize(4);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 4);
            Offset += 4;
        }
        public void Write(uint value)
        {
            EnsureSize(4);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 4);
            Offset += 4;
        }
        public void Write(long value)
        {
            EnsureSize(8);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 8);
            Offset += 8;
        }
        public void Write(ulong value)
        {
            EnsureSize(8);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 8);
            Offset += 8;
        }
        public void Write(float value)
        {
            EnsureSize(4);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 4);
            Offset += 4;
        }
        public void Write(double value)
        {
            EnsureSize(8);
            Array.Copy(BitConverter.GetBytes(value), 0, _data, Offset, 8);
            Offset += 8;
        }
        public void Write(Vector2 value)
        {
            Write(value.X);
            Write(value.Y);
        }
        public void Write(HalfVector2 value)
        {
            Write(value.PackedValue);
        }
        public void Write(Color value)
        {
            Write(value.PackedValue);
        }
        public void Write(string value)
        {
            Write(Encoding.UTF8.GetBytes(value));
        }
        public void Write(TimeSpan value)
        {
            Write(value.Ticks);
        }
        public void Write<T>(T value)
        {
            Write(SerializationHelpers.JSONSerializationBytes);
            Write(JsonConvert.SerializeObject(value));
        }
        public void WriteStruct<T>(T value) where T : struct
        {
            int size = Marshal.SizeOf(value);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            Write(arr);
        }
        public void Write(byte[] value)
        {
            Write((ushort)value.Length);
            EnsureSize(value.Length);
            Array.Copy(value, 0, _data, Offset, value.Length);
            Offset += value.Length;
        }
        public void Write(Half value)
        {
            Write(value.InternalValue);
        }

        private static Stack<ByteArrayWriter> _cache = new Stack<ByteArrayWriter>();

        public static ByteArrayWriter Get()
        {
            ByteArrayWriter r;
            if (_cache.Count > 0)
                r = _cache.Pop();
            else r = new ByteArrayWriter();
            r.Size = 0;
            r.Offset = 0;
            return r;
        }

        public static void Release(ByteArrayWriter writer)
        {
            if (!_cache.Contains(writer) && _cache.Count < 64 && writer._data.Length < 1024 * 16) //max 16kb stored
                _cache.Push(writer);
        }
        public void Release() =>
            Release(this);

        public byte[] ReleaseAndReturnData()
        {
            var data = Data;
            Release();
            return data;
        }
    }
}
