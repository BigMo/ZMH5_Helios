using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.Modules.SnapshotHelpers
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]//, Pack=4)]
    public struct RadarEntry
    {
        private const int ENTRY_SIZE = 0x1Ec; //was 0x1e0
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x84)]
        private byte[] pad0;

        public Vector3 Origin;
        public Vector3 ViewAngles;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private byte[] pad1;

        public float flSpawnTime;
        public float flDeathTime;

        //Two floats, the first one always -1000.0, the second one always 1.0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private byte[] pad2;

        public float flSimTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private byte[] pad3;
        
        public int Id;
        public int Health;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] //0xD4
        public string Name;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ENTRY_SIZE - (0xD4+64))]
        private byte[] pad4;
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
