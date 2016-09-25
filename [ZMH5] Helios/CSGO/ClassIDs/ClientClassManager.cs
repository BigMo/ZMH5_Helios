using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.ClassIDs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ClientClassManager
    {
        #region FIELDS
        [FieldOffset(0x00)]
        public int m_pCIDArray;
        [FieldOffset(0x04)]
        public int m_iCount;
        #endregion
    }
}
