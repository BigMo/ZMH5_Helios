using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Core;
using ZatsHackBase.Maths;
using static _ZMH5__Helios.CSGO.Settings;

namespace _ZMH5__Helios.CSGO.Misc
{
    public class AimSettings
    {
        public bool Enabled;
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys Key;
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyMode Mode;
        public float FOV;
        public bool Lock;
        public int Bone;
        public AimSmooth Smoothing;
        public bool Predict;
        public bool VisibleOnly;
        public bool Sticky;
        public float HeightOffset;
    }

    public class AimSmooth
    {
        public bool Enabled;
        [JsonConverter(typeof(StringEnumConverter))]
        public SmoothMode Mode;
        public float Scalar;
        public Vector2 PerAxis;
    }
}
