using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.ClassIDs
{

    [StructLayout(LayoutKind.Explicit)]
    public struct RecvTable_t
    {
        #region FIELDS
        [FieldOffset(0x0)]
        public int m_pProps;
        [FieldOffset(0x4)]
        public int m_nProps;
        //[FieldOffset(0x8)]
        //EMTPY!
        [FieldOffset(0xC)]
        public int m_pNetTableName;
        #endregion
    }
}
