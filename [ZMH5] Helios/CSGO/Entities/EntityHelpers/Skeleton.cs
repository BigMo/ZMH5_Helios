using _ZMH5__Helios.CSGO.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities.EntityHelpers
{
    public class Skeleton : RPMLazyArray<Bone>
    {
        #region CONSTANTS
        public const int MAX_BONES = 80;
        #endregion

        public Skeleton(int address) : base(address, MAX_BONES)
        { }
    }
}
