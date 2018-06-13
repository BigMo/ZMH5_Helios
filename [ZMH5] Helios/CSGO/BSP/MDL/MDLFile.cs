using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;

namespace _ZMH5__Helios.CSGO.BSP.MDL
{
    public class MDLFile
    {
        private studiohdr_t header;

        public HitboxSet[] HitBoxSets { get; private set; }

        public MDLFile(Stream str)
        {
            header = str.Read<studiohdr_t>();
            var hitboxes = str.ReadArray<mstudiohitboxset_t>(header.hitboxsetindex, header.numhitboxsets);
            HitBoxSets = new HitboxSet[header.numhitboxsets];
            for (int h = 0; h < HitBoxSets.Length; h++)
                HitBoxSets[h] = new HitboxSet(str, header.hitboxsetindex + SizeCache<mstudiohitboxset_t>.Size * h, hitboxes[h]);
        }
    }
}
