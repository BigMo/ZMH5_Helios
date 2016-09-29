using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;

namespace _ZMH5__Helios.CSGO.Misc
{
    public class RPMLazyArray<T> : LazyArray<T> where T : struct
    {
        #region VARIABLES
        public static int Size = Marshal.SizeOf(typeof(T));
        private byte[] data;
        private MemoryStream stream;
        #endregion

        public RPMLazyArray(int address, int size) : base(size)
        {
            data = new byte[Size * size];
            Program.Hack.Memory.Position = address;
            Program.Hack.Memory.Read(data, 0, data.Length);
            stream = new MemoryStream(data);

            this.loadFunc = (i) => ReadObject(i); 
        }

        private T ReadObject(int index)
        {
            return stream.Read<T>(Size * index);
        }
    }
}
