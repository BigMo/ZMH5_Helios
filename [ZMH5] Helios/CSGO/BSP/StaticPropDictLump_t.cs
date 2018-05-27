using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    public struct StaticPropDictLump_t
    {
        public int m_DictEntries;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct StaticPropDictLumpName
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string m_Name;
    }
}
