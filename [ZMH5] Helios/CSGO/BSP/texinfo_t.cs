using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    struct texinfo_t
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Vector4[] m_TextureVecs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Vector4[] m_LightmapVecs;

        public int m_Flags;
        public int m_Texdata;
    }

    struct Vector4
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] m_Elements;
    }
}
