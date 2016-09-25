using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    public struct snode_t
    {
        public int m_Planenum;
        public long m_pPlane;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] m_Children;

        public long m_LeafChildren;
        public long m_NodeChildren;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] m_Mins;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] m_Maxs;

        public ushort m_Firstface;
        public ushort m_Numfaces;
        public short m_Area;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] m_Pad;
    }
}
