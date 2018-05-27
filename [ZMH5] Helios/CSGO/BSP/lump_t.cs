using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    public struct lump_t
    {
        public int m_Fileofs;
        public int m_Filelen;
        public int m_Version;
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        //public string m_FourCC;
        public int m_FourCC;
    }
}
