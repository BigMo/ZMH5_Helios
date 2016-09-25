using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.ClassIDs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ClientClass
    {
        #region FIELDS
        [FieldOffset(0x08)]
        public int m_pNetworkName;
        [FieldOffset(0x0C)]
        public int m_pRecvTable;
        [FieldOffset(0x10)]
        public int m_pNext;
        [FieldOffset(0x14)]
        public int m_ClassID;
        #endregion
    }
}
