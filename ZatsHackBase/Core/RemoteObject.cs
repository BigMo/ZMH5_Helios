using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    public abstract class RemoteObject
    {
        public IntPtr Address { get; private set; }
        public int Size { get; private set; }

        protected RemoteObject(Memory mem, IntPtr address, int size)
        {
            if (address == IntPtr.Zero)
                throw new Exception("Nullpointer");

            Address = address;
            Size = size;

            Refresh(mem);
        }

        protected RemoteObject(int size)
        {
            Size = size;
        }

        public void Init(Memory mem, IntPtr address)
        {
            Address = address;
            Refresh(mem);
        }

        public unsafe void Refresh(Memory mem)
        {
            var data = new byte[Size];
            mem.Read(Address, data, 0, data.Length);
            fixed (byte* p = data)
            {
                ReadFields(p);
            }
        }

        protected unsafe abstract void ReadFields(byte* data);
    }
}
