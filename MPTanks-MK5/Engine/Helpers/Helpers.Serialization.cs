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
        
        public static readonly byte TrueByte = 1;
        public static readonly byte FalseByte = 0;

        #region Byte array serialization
        public static byte[] ToByteArray(Vector2 obj)
        {
            var arr = new byte[8];
            Array.Copy(BitConverter.GetBytes(obj.X), arr, 4);
            Array.Copy(BitConverter.GetBytes(obj.Y), 0, arr, 4, 4);
            return arr;
        }
        public static byte[] ToByteArray(Color obj) => BitConverter.GetBytes(obj.PackedValue);

        public static Vector2 Vector2FromByteArray(byte[] arr) =>
            new Vector2() { X = BitConverter.ToSingle(arr, 0), Y = BitConverter.ToSingle(arr, 4) };

        public static Color ColorFromByteArray(byte[] arr) =>
            new Color() { PackedValue = BitConverter.ToUInt32(arr, 0) };
        #endregion

        public static void SetContents(this byte[] arr1, byte[] source, int offset) =>
            Array.Copy(source, 0, arr1, offset, source.Length);

        public static void SetByteArray(this byte[] arr1, byte[] source, int offset)
        {
            arr1.SetContents((ushort)source.Length, offset);
            arr1.SetContents(source, offset + 2);
        }

        public static void SetContents(this byte[] arr1, int obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, uint obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, ushort obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, short obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, ulong obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, long obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, float obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, double obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, Color obj, int offset)
        {
            var source = ToByteArray(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, Vector2 obj, int offset)
        {
            var source = ToByteArray(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] array, string value, int offset, bool header = true)
        {
            var source = new byte[2 + Encoding.UTF8.GetByteCount(value)];
            if (header)
                array.SetContents((ushort)Encoding.UTF8.GetByteCount(value), offset);

            array.SetContents(Encoding.UTF8.GetBytes(value), header ? offset + 2 : offset);
        }
        public static void SetContents(this byte[] array, bool value, int offset)
        {
            array[offset] = value ? (byte)1 : (byte)0;
        }
        public static void SetContents(this byte[] array, Half value, int offset)
        {
            array.SetContents(value.InternalValue, offset);
        }

        public static void SetContents(this byte[] array, HalfVector2 value, int offset)
        {
            array.SetContents(value.PackedValue, offset);
        }


        public static unsafe T GetValue<T>(this byte[] src, int offset) where T : struct
        {
            if (typeof(T) == typeof(int))
                return (T)(object)GetInt(src, offset);
            if (typeof(T) == typeof(uint))
                return (T)(object)BitConverter.ToUInt32(src, offset);

            if (typeof(T) == typeof(Vector2))
                return (T)(object)GetVector(src, offset);
            if (typeof(T) == typeof(HalfVector2))
                return (T)(object)GetHalfVector(src, offset);
            if (typeof(T) == typeof(Color))
                return (T)(object)GetInt(src, offset);
            if (typeof(T) == typeof(float))
                return (T)(object)GetFloat(src, offset);
            if (typeof(T) == typeof(double))
                return (T)(object)BitConverter.ToDouble(src, offset);

            if (typeof(T) == typeof(long))
                return (T)(object)BitConverter.ToInt64(src, offset);
            if (typeof(T) == typeof(ulong))
                return (T)(object)BitConverter.ToUInt64(src, offset);

            if (typeof(T) == typeof(short))
                return (T)(object)BitConverter.ToInt16(src, offset);
            if (typeof(T) == typeof(ushort))
                return (T)(object)BitConverter.ToUInt16(src, offset);

            if (typeof(T) == typeof(byte))
                return (T)(object)src[offset];
            if (typeof(T) == typeof(sbyte))
                return (T)(object)(sbyte)src[offset];

            if (typeof(T) == typeof(string))
                return (T)(object)src.GetString(offset);

            if (typeof(T) == typeof(bool))
                return (T)(object)(src[offset] == 1);

            if (typeof(T) == typeof(Half))
                return (T)(object)(new Half() { InternalValue = BitConverter.ToUInt16(src, offset) });

            if (typeof(T) == typeof(Guid))
                return (T)(object)src.GetGuid(offset);

            return UnsafeRead<T>(src.Slice(offset, Marshal.SizeOf(typeof(T))));
        }

        private static T UnsafeRead<T>(byte[] bytes)
        {

            // Read in a byte array
            // Pin the managed memory while, copy it out the data, then unpin it
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        public static byte[] ToByteArray(this object value)
        {
            if (!value.GetType().IsValueType)
                throw new Exception("Must be a value type (struct)");
            var size = Marshal.SizeOf(value);
            var arr = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);

            return arr;
        }


        public static Vector2 GetVector(this byte[] src, int offset)
        {
            return new Vector2(src.GetFloat(offset), src.GetFloat(offset + 4));
        }
        public static HalfVector2 GetHalfVector(this byte[] src, int offset)
        {
            return new HalfVector2() { PackedValue = src.GetUInt(offset) };
        }

        public static Color GetColor(this byte[] src, int offset)
        {
            var bytes = GetBytes(src, offset, 4);
            return ColorFromByteArray(bytes);
        }

        public static float GetFloat(this byte[] src, int offset) => BitConverter.ToSingle(src, offset);
        public static double GetDouble(this byte[] src, int offset) => BitConverter.ToDouble(src, offset);
        public static Half GetHalf(this byte[] src, int offset) => new Half() { InternalValue = BitConverter.ToUInt16(src, offset) };
        public static ushort GetUShort(this byte[] src, int offset) => BitConverter.ToUInt16(src, offset);
        public static short GetShort(this byte[] src, int offset) => BitConverter.ToInt16(src, offset);

        public static int GetInt(this byte[] src, int offset) => BitConverter.ToInt32(src, offset);
        public static uint GetUInt(this byte[] src, int offset) => BitConverter.ToUInt32(src, offset);
        public static long GetLong(this byte[] src, int offset) => BitConverter.ToInt64(src, offset);
        public static ulong GetULong(this byte[] src, int offset) => BitConverter.ToUInt64(src, offset);
        public static bool GetBool(this byte[] src, int offset) => src[offset] == 1;
        public static Guid GetGuid(this byte[] src, int offset) => new Guid(src.Slice(offset, 16));

        public static byte[] GetByteArray(this byte[] src, int offset, int? count = null)
        {
            if (count == null)
            {
                count = src.GetUShort(offset);
                offset += 2;
            }
            return src.Slice(offset, count.Value);
        }

        public static bool SequenceBegins(this byte[] arr, byte[] other, int offset = 0)
        {
            if (other.Length > arr.Length - offset)
                return false;
            for (var i = 0; i < other.Length; i++)
                if (arr[offset + i] != other[i]) return false;

            return true;
        }

        public static string GetString(this byte[] src, int offset, int? length = null)
        {
            if (length == null)
                length = src.GetValue<ushort>(offset);
            offset += 2;
            return Encoding.UTF8.GetString(src, offset, length.Value);
        }

        public static byte[] GetBytes(byte[] src, int offset, int? count = null)
        {
            if (count == null)
            {
                count = src.GetUShort(offset);
                offset += 2;
            }
            var bytes = new byte[count.Value];
            Array.Copy(src, offset, bytes, 0, count.Value);
            return bytes;
        }

        public static byte[] AllocateArray(bool shouldCopyContentsIntoArray, params object[] contents)
        {
            int size = 0;
            foreach (var obj in contents)
            {
                if (obj == null) continue; //it's null :(

                if (obj.GetType() == typeof(ulong))
                    size += 8;

                else if (obj.GetType() == typeof(long))
                    size += 8;

                else if (obj.GetType() == typeof(uint))
                    size += 4;
                else if (obj.GetType() == typeof(int))
                    size += 4;

                else if (obj.GetType() == typeof(ushort))
                    size += 2;
                else if (obj.GetType() == typeof(short))
                    size += 2;

                else if (obj.GetType() == typeof(byte))
                    size += 1;
                else if (obj.GetType() == typeof(sbyte))
                    size += 1;

                else if (obj.GetType() == typeof(bool))
                    size += 1;

                else if (obj.GetType() == typeof(float))
                    size += 4;
                else if (obj.GetType() == typeof(double))
                    size += 8;
                else if (obj.GetType() == typeof(Half))
                    size += 2;

                else if (obj.GetType() == typeof(Vector2))
                    size += 8;
                else if (obj.GetType() == typeof(HalfVector2))
                    size += 4;
                else if (obj.GetType() == typeof(Color))
                    size += 4;

                else if (obj.GetType() == typeof(Guid))
                    size += 16;

                else if (obj.GetType() == typeof(string))
                {
                    size += 2; //header
                    size += Encoding.UTF8.GetByteCount((string)obj);
                }
                else if (obj.GetType() == typeof(byte[]))
                    size += ((byte[])obj).Length + 2;
                else if (obj.GetType().IsValueType)
                    size += ToByteArray(obj).Length + 2;
                else
                {
                    var ser = JsonConvert.SerializeObject(obj);
                    size += 2;
                    size += Encoding.UTF8.GetByteCount(ser);
                }
            }

            var arr = new byte[size];

            if (shouldCopyContentsIntoArray)
            {
                int offset = 0;
                foreach (var obj in contents)
                {
                    if (obj == null) continue; //it's null :(

                    if (obj.GetType() == typeof(ulong))
                    {
                        arr.SetContents((ulong)obj, offset);
                        offset += 8;
                    }
                    else if (obj.GetType() == typeof(long))
                    {
                        arr.SetContents((long)obj, offset);
                        offset += 8;
                    }

                    else if (obj.GetType() == typeof(uint))
                    {
                        arr.SetContents((uint)obj, offset);
                        offset += 4;
                    }
                    else if (obj.GetType() == typeof(int))
                    {
                        arr.SetContents((int)obj, offset);
                        offset += 4;
                    }

                    else if (obj.GetType() == typeof(ushort))
                    {
                        arr.SetContents((ushort)obj, offset);
                        offset += 2;
                    }
                    else if (obj.GetType() == typeof(short))
                    {
                        arr.SetContents((short)obj, offset);
                        offset += 2;
                    }

                    else if (obj.GetType() == typeof(byte))
                    {
                        arr[offset] = (byte)obj;
                        offset += 1;
                    }

                    else if (obj.GetType() == typeof(sbyte))
                    {
                        arr[offset] = unchecked((byte)(sbyte)obj);
                        offset += 1;
                    }

                    else if (obj.GetType() == typeof(bool))
                    {
                        arr[offset] = (bool)obj ? (byte)1 : (byte)0;
                        offset += 1;
                    }

                    else if (obj.GetType() == typeof(float))
                    {
                        arr.SetContents((float)obj, offset);
                        offset += 4;
                    }
                    else if (obj.GetType() == typeof(double))
                    {
                        arr.SetContents((double)obj, offset);
                        offset += 8;
                    }
                    else if (obj.GetType() == typeof(Half))
                    {
                        arr.SetContents((Half)obj, offset);
                        offset += 2;
                    }

                    else if (obj.GetType() == typeof(Vector2))
                    {
                        arr.SetContents((Vector2)obj, offset);
                        offset += 8;
                    }
                    else if (obj.GetType() == typeof(HalfVector2))
                    {
                        arr.SetContents((HalfVector2)obj, offset);
                        offset += 4;
                    }
                    else if (obj.GetType() == typeof(Color))
                    {
                        arr.SetContents((Color)obj, offset);
                        offset += 4;
                    }

                    else if (obj.GetType() == typeof(string))
                    {
                        arr.SetContents((string)obj, offset);
                        offset += 2; //header
                        offset += Encoding.UTF8.GetByteCount((string)obj);
                    }
                    else if (obj.GetType() == typeof(byte[]))
                    {
                        arr.SetByteArray((byte[])obj, offset);
                        offset += ((byte[])obj).Length + 2;
                    }
                    else if (obj.GetType().IsValueType)
                    {
                        var data = obj.ToByteArray();
                        arr.SetContents(data, offset);
                        offset += data.Length;
                    }
                    else
                    {
                        arr.SetContents(JSONSerializationBytes, offset);
                        offset += JSONSerializationBytes.Length;
                        var ser = JsonConvert.SerializeObject(obj);
                        arr.SetContents(ser, offset);
                        offset += 2; //header
                        offset += Encoding.UTF8.GetByteCount(ser);
                    }
                }
            }
            return arr;
        }

        public static byte[] Serialize(byte[] obj) => obj;

        public static byte[] Serialize(string obj)
        {
            return AllocateArray(true,
                StringSerializationBytes,
                obj
                );
        }

        private static JsonSerializerSettings _serializerSettingsForStateChange = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameHandling = TypeNameHandling.All
        };
        public static byte[] Serialize(object obj)
        {
            if (obj.GetType() == typeof(byte[]))
                return (byte[])obj;
            if (obj.GetType() == typeof(string))
                return Serialize((string)obj);

            return AllocateArray(
                true,
                JSONSerializationBytes,
                JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettingsForStateChange));
        }


        public static void ResolveDeserialize(byte[] arr, Action<byte[]> arrAction, Action<string> strAction, Action<dynamic> dynamicAction)
        {
            if (arr.SequenceBegins(JSONSerializationBytes))
            {
                //Try to deserialize
                var obj = JsonConvert.DeserializeObject(arr.GetString(JSONSerializationBytes.Length));
                dynamicAction(obj);
            }
            else if (arr.SequenceBegins(StringSerializationBytes))
            {
                //Try to deserialize
                var obj = arr.GetString(StringSerializationBytes.Length);
                strAction(obj);
            }
            else
            {
                arrAction(arr);
            }
        }

        public static byte[] MergeArrays(byte[] arr1, byte[] arr2, bool addLengthHeader = false)
        {
            var arr3 = new byte[arr1.Length + arr2.Length + (addLengthHeader ? 2 : 0)];
            Array.Copy(arr1, arr3, arr1.Length);
            if (addLengthHeader)
            {
                arr3.SetContents((ushort)arr2.Length, arr1.Length);
                Array.Copy(arr2, 0, arr3, arr1.Length + 2, arr2.Length);
            }
            else
                Array.Copy(arr2, 0, arr3, arr1.Length, arr2.Length);
            return arr3;
        }

        public static byte[] Slice(this byte[] src, int offset, int count) => GetBytes(src, offset, count);
        
    }
}
