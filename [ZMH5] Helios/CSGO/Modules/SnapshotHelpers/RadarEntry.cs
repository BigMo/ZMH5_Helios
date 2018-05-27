using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.Modules.SnapshotHelpers
{
    [StructLayout(LayoutKind.Explicit, CharSet=CharSet.Unicode)]//, Pack=4)]
    public struct RadarEntry
    {
        private const int ENTRY_SIZE = 0x1EC; //was 0x1e0
        
        [FieldOffset(0x84)]
        public Vector3 Origin;
        [FieldOffset(0x90)]
        public Vector3 ViewAngles;

        [FieldOffset(0xC0)]
        public float flSimTime;

        [FieldOffset(0xCC)]
        public int Id;
        [FieldOffset(0xD0)]
        public int Health;

        [FieldOffset(0xD4)]
        public string Name;

        [FieldOffset(0xF4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ENTRY_SIZE - 0xF4)]
        public byte[] padding;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Radar
    {
        private const int NUM_ENTRIES = 64; //meh
        private const int DIST = 0x154; //was 0x140

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DIST)]
        private byte[] junk1;


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_ENTRIES)]
        public RadarEntry[] Entries;

    }
}
