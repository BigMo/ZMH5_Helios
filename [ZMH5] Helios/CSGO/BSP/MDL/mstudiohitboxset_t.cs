using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;

namespace _ZMH5__Helios.CSGO.BSP.MDL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct mstudiohitboxset_t
    {
        public int sznameindex;
        public int GetNameOffset() { return sznameindex; }
        //public char* const pszName( void ) const { return ((char*)this) + sznameindex; }
        public int numhitboxes;
        public int hitboxindex;
        public int GetHitboxOffset(int i) { return hitboxindex + SizeCache<mstudiohitboxset_t>.Size * i; }
        //inline mstudiobbox_t *pHitbox(int i) const { return (mstudiobbox_t*) (((byte*)this) + hitboxindex) + i; };
    }
}
