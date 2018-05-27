using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Core;

namespace _ZMH5__Helios.CSGO.Misc
{
    public class TriggerSettings
    {
        public bool Enabled;
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys Key;
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyMode Mode;
        public int Delay;
        public BurstSettings Burst;
    }

    public class BurstSettings
    {
        public bool Enabled;
        public int Count;
        public int Delay;
    }
}
