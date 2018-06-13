using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Modules.GlowHelpers
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GlowObjectDefinition
    {
        [FieldOffset(0x00)]
        public IntPtr pEntity;
        [FieldOffset(0x04)]
        public float r;
        [FieldOffset(0x08)]
        public float g;
        [FieldOffset(0x0C)]
        public float b;
        [FieldOffset(0x10)]
        public float a;

        //16 bytes junk

        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] junk0;

        [FieldOffset(0x24)]
        public byte m_bRenderWhenOccluded;
        [FieldOffset(0x25)]
        public byte m_bRenderWhenUnoccluded;
        [FieldOffset(0x26)]
        public byte m_bFullBloom;

        //12 bytes junk
        [FieldOffset(0x27)]
        public byte junk2;

        [FieldOffset(0x28)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] junk1;
    }
}
