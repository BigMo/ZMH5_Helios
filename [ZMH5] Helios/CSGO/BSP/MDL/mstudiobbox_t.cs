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
    public struct  mstudiobbox_t
    {
        public int bone;
        public int group;              // intersection group
        public Vector3 bbmin;               // bounding box
        public Vector3 bbmax;
        public int szhitboxnameindex;  // offset to the name of the hitbox.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public int[] unused;

        public int GetNameOffset()
        {
            if (szhitboxnameindex == 0)
                return 0;
            return szhitboxnameindex;
        }

        //       const char* pszHitboxName()
        //{
        //	if(szhitboxnameindex == 0 )
        //		return "";

        //	return ((const char*)this) + szhitboxnameindex;
        //}
    }
}
