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
    public struct RecvProp_t
    {
        #region ENUMS
        public enum ePropType : int
        {
            Int = 0,
            Float,
            Vector,
            VectorXY,
            String,
            Array,
            DataTable,
            NUMSendPropTypes
        };
        #endregion

        #region FIELDS
        [FieldOffset(0x0)]
        public int m_pVarName;
        [FieldOffset(0x4)]
        public int m_RecvType;
        [FieldOffset(0x28)]
        public int m_pDataTable;
        [FieldOffset(0x2C)]
        public int m_Offset;
        [FieldOffset(0x34)]
        public ePropType m_nElements;
        #endregion
    }
}
