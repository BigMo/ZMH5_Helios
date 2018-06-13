using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;

namespace _ZMH5__Helios.CSGO.BSP.MDL
{
    public class HitboxSet
    {
        public string Name { get; private set; }
        public BoundingBox[] BoundingBoxes { get; private set; }

        public HitboxSet(Stream str, int address, mstudiohitboxset_t set)
        {
            Name = str.ReadString(address + set.sznameindex, 64, Encoding.ASCII);
            BoundingBoxes = new BoundingBox[set.numhitboxes];
            var boxes = str.ReadArray<mstudiobbox_t>(address + set.hitboxindex, set.numhitboxes);
            for (int i = 0; i < set.numhitboxes; i++)
                BoundingBoxes[i] = new BoundingBox(str, address + set.hitboxindex + SizeCache<mstudiobbox_t>.Size * i, boxes[i]);
        }
    }
}
