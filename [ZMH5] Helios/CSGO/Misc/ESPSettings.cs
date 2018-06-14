using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Drawing;

namespace _ZMH5__Helios.CSGO.Misc
{
    public class ESPSettings
    {
        public bool Enabled;
        public ESPEntry Allies,
            Enemies,
            Weapons,
            C4,
            Grenades,
            Chickens,
            World;
    }
    public class ESPEntry
    {
        public bool Enabled;
        public bool ShowName;
        public bool ShowBox;
        public bool ShowHealth;
        public bool ShowDist;
        public bool ShowWeapon;
        public Color Color;
        public Color ColorOccluded;
    }
}
