using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    struct dedge_t
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] m_V;
    }
}
