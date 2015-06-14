using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public static partial class Helpers
    {
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

        public static void SetContents(this byte[] arr1, int obj, int offset)
        {
            var source = BitConverter.GetBytes(obj);
            Array.Copy(source, 0, arr1, offset, source.Length);
        }
        public static void SetContents(this byte[] arr1, float obj, int offset)
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

        public static T GetValue<T>(this byte[] src, int offset)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)GetInt(src, offset);
            if (typeof(T) == typeof(uint))
                return (T)(object)BitConverter.ToUInt32(src, offset);

            if (typeof(T) == typeof(Vector2))
                return (T)(object)GetVector(src, offset);
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

            throw new Exception("Not allowed");
        }

        public static Vector2 GetVector(byte[] src, int offset)
        {
            var bytes = GetBytes(src, offset, 8);
            return Vector2FromByteArray(bytes);
        }

        public static Color GetColor(byte[] src, int offset)
        {
            var bytes = GetBytes(src, offset, 4);
            return ColorFromByteArray(bytes);
        }

        public static float GetFloat(byte[] src, int offset) => BitConverter.ToSingle(src, offset);

        public static int GetInt(byte[] src, int offset) => BitConverter.ToInt32(src, offset);

        public static byte[] GetBytes(byte[] src, int offset, int count)
        {
            var bytes = new byte[count];
            Array.Copy(src, offset, bytes, 0, count);
            return bytes;
        }

        public static byte[] Slice(this byte[] src, int offset, int count) => GetBytes(src, offset, count);
    }
}
