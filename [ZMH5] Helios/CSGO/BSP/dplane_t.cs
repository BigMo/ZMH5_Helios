using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.BSP
{
    [StructLayout(LayoutKind.Sequential)]
    struct dplane_t
    {
        public Vector3 m_Normal;
        public float m_Distance;
        public byte m_Type;
    }
}
