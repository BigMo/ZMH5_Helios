using _ZMH5__Helios.CSGO.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities.EntityHelpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Skeleton
    {
        private const int MAX_BONES = 32;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_BONES)]
        public Bone[] m_Bones;
    }
}
