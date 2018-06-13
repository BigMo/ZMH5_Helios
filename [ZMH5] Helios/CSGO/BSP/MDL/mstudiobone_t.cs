using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.BSP.MDL
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        public float x, y, z, w;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct matrix3x4_t
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3 * 4)]
        public float[] m_flMatVal;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct mstudiobone_t
    {
        public int sznameindex;
        //inline char* const pszName( void ) const { return ((char*)this) + sznameindex; }
        public int parent;     // parent bone
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public int[] bonecontroller;  // bone controller index, -1 == none

        // default values
        public Vector3 pos;
        public Quaternion quat;
        public Vector3 rot;
        // compression scale
        public Vector3 posscale;
        public Vector3 rotscale;

        public matrix3x4_t poseToBone;
        public Quaternion qAlignment;
        public int flags;
        public int proctype;
        public int procindex;      // procedural rule
        public int physicsbone;    // index into physically simulated bone
        //inline void* pProcedure() const { if (procindex == 0) return NULL; else return  (void*) (((byte*)this) + procindex); };
        public int surfacepropidx; // index into string tablefor property name
        //inline char* const pszSurfaceProp( void ) const { return ((char*)this) + surfacepropidx; }
        public int contents;       // See BSPFlags.h for the contents flags

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        int[] unused;		// remove as appropriate

    }
}
