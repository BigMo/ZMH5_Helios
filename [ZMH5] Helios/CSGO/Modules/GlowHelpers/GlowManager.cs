using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Modules.GlowHelpers
{
    [StructLayout(LayoutKind.Explicit)]
    struct GlowManager
    {
        #region FIELDS
        [FieldOffset(0x00)]
        public int m_pGlowArray;
        [FieldOffset(0x04)]
        public int m_iCapacity;
        [FieldOffset(0x0C)]
        public int m_iNumObjects;
        #endregion
    }
}
