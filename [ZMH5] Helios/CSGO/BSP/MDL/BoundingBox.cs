using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.BSP.MDL
{
    public class BoundingBox
    {
        public int Bone { get; private set; }
        public int Group { get; private set; }
        public Vector3 BBMin { get; private set; }
        public Vector3 BBMax { get; private set; }
        public string HitboxName { get; private set; }


        public BoundingBox(Stream str, int address, mstudiobbox_t box)
        {
            Bone = box.bone;
            Group = box.group;
            BBMax = box.bbmax;
            BBMin = box.bbmin;
            if (box.szhitboxnameindex == 0)
                HitboxName = "";
            else
                HitboxName = str.ReadString(address + box.szhitboxnameindex, 64, Encoding.ASCII);
        }
    }
}
