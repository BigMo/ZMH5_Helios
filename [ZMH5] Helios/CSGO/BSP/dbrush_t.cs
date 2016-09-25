using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    public struct dbrush_t
    {
        public int m_Firstside;
        public int m_Numsides;
        public int m_Contents;
    }
}
