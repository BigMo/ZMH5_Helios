using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.Entities.EntityHelpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Bone
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x0C)]
        public byte[] junk0;
        public float X;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x0C)]
        public byte[] junk1;
        public float Y;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x0C)]
        public byte[] junk2;
        public float Z;

        public Vector3 ToVector() { return new Vector3(X, Y, Z); }
    }
}
