using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.Modules.SnapshotHelpers
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    public struct RadarEntry
    {
        private const int ENTRY_SIZE = 0x1E0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xA0)]
        private byte[] junk1;

        public Vector3 Origin;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        private byte[] junk2;

        public int Id;
        public int Health;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ENTRY_SIZE - 0xA0 - 12 - 16 - 4 - 4 - 32 * 2)]
        private byte[] junk3;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Radar
    {
        private const int NUM_ENTRIES = 64;
        private const int DIST = 0x140;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DIST)]
        private byte[] junk1;


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_ENTRIES)]
        public RadarEntry[] Entries;

    }
}
