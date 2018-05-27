using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    public struct dgamelump_t
    {
        public int m_Id;
        public ushort m_Flags;
        public ushort m_Version;
        public int m_FileOfs;
        public int m_FileLen;
    }
}
