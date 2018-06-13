using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    /// <summary>
    /// Some extensions to the abstract stream-class that allows for reading/writing simple data-types
    /// </summary>
    public static class StreamExtension
    {
        #region METHODS
        public static void Write<T>(this Stream str, long position, T value) where T : struct
        {
            str.Position = position;
            str.Write(value);
        }
        public static void Write<T>(this Stream str, long position, T value, int offset, int length) where T : struct
        {
            str.Position = position;
            str.Write(value, offset, length);
        }
        public static void Write<T>(this Stream str, T value) where T : struct
        {
            var data = TToBytes(value);
            str.Write(data, 0, data.Length);
        }
        public static void Write<T>(this Stream str, T value, int offset, int length) where T : struct
        {
            str.Position += offset;
            var data = TToBytes(value);
            var wrData = new byte[length];
            Array.Copy(data, offset, wrData, 0, length);
            str.Write(wrData, 0, length);
        }

        public static T Read<T>(this Stream str, IntPtr position) where T : struct
        {
            str.Position = (long)position;
            return str.Read<T>();
        }
        public static T Read<T>(this Stream str, long position) where T : struct
        {
            str.Position = position;
            return str.Read<T>();
        }
        public static T Read<T>(this Stream str) where T : struct
        {
            byte[] data = new byte[SizeCache<T>.Size];
            int cnt = str.Read(data, 0, data.Length);
            if (cnt != data.Length)
                throw new Exception(string.Format("Failed to read a \"{0}\" from stream: Read {1} of {2} bytes.",
                    typeof(T).Name, cnt, data.Length));

            return BytesToT<T>(data);
        }

        public static T[] ReadArray<T>(this Stream str, long position, int count) where T : struct
        {
            str.Position = position;
            return str.ReadArray<T>(count);
        }
        public static T[] ReadArray<T>(this Stream str, int count) where T : struct
        {
            int sz = SizeCache<T>.Size;
            byte[] data = new byte[sz * count];
            int cnt = str.Read(data, 0, data.Length);
            if (cnt != data.Length)
                throw new Exception(string.Format("Failed to read a \"{0}[{1}]\" from stream: Read {2} of {3} bytes.",
                    typeof(T).Name, count, cnt, data.Length));

            T[] res = new T[count];
            byte[] dt = new byte[sz];

            for (int i = 0; i < count; i++)
            {
                Array.Copy(data, sz * i, dt, 0, sz);
                res[i] = BytesToT<T>(dt);
            }

            return res;
        }

        public static string ReadString(this Stream str, IntPtr position, int length, Encoding encoding)
        {
            str.Position = (long)position;
            return ReadString(str, length, encoding);
        }
        public static string ReadString(this Stream str, long position, int length, Encoding encoding)
        {
            str.Position = position;
            return ReadString(str, length, encoding);
        }
        public static string ReadString(this Stream str, int length, Encoding encoding)
        {
            byte[] data = new byte[length];
            str.Read(data, 0, length);
            return encoding.GetString(data.TakeWhile(x => x != 0).ToArray());
        }
        #endregion

        #region MARSHALLING
        private unsafe static byte[] TToBytes<T>(T value) where T : struct
        {
            byte[] data = new byte[SizeCache<T>.Size];

            fixed (byte* b = data)
            Marshal.StructureToPtr(value, (IntPtr)b, true);

            return data;
        }
        private static unsafe T BytesToT<T>(byte[] data, T defVal = default(T)) where T : struct
        {
            T structure = defVal;

            fixed (byte* b = data)
            structure = (T)Marshal.PtrToStructure((IntPtr)b, typeof(T));

            return structure;
        }
        #endregion
    }
}
