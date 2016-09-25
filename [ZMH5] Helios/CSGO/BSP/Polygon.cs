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
    public struct Polygon
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSPFlags.MAX_SURFINFO_VERTS)]
        public Vector3[] m_Verts;

        public int m_nVerts;
        public VPlane m_Plane;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSPFlags.MAX_SURFINFO_VERTS)]
        public VPlane[] m_EdgePlanes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BSPFlags.MAX_SURFINFO_VERTS)]
        public Vector3[] m_Vec2D;

        public int m_Skip;
    }
}
