using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Drawing;

namespace _ZMH5__Helios.CSGO.Misc
{
    public class GlowSettings
    {
        public bool Enabled;
        public GlowEntry Allies,
            Enemies,
            Weapons,
            C4,
            Grenades,
            Chickens;
    }

    public class GlowEntry
    {
        public bool Enabled;
        public Color Color;
    }
}
