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
            Int64,
            NUMSendPropTypes
        };
        #endregion

        public static int GetPropTypeSize(ePropType type)
        {
            switch (type)
            {
                case ePropType.Int:
                case ePropType.Float:
                    return 4;
                case ePropType.VectorXY:
                case ePropType.Int64:
                    return 8;
                case ePropType.Vector:
                    return 12;
            }
            return -1;
        }

        #region FIELDS
        [FieldOffset(0x0)]
        public int m_pVarName;
        [FieldOffset(0x4)]
        public ePropType m_RecvType;
        [FieldOffset(0x8)]
        public int m_Flags;
        [FieldOffset(0xC)]
        public int m_StringBufferSize;
        [FieldOffset(0x10)]
        public bool m_bInsideArray;
        [FieldOffset(0x14)]
        public int m_pExtraData;
        [FieldOffset(0x18)]
        public int m_pArrayProp;
        [FieldOffset(0x1C)]
        public int m_ArrayLengthProxy;
        [FieldOffset(0x20)]
        public int m_ProxyFn;
        [FieldOffset(0x24)]
        public int m_DataTableProxyFn;
        [FieldOffset(0x28)]
        public int m_pDataTable;
        [FieldOffset(0x2C)]
        public int m_Offset;
        [FieldOffset(0x30)]
        public int m_ElementStride;
        [FieldOffset(0x34)]
        public int m_nElements;
        [FieldOffset(0x38)]
        public int m_pParentArrayPropName;
        #endregion
    }
}
