using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Core;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;

namespace _ZMH5__Helios.CSGO
{
    public class Settings
    {
        #region ENUMS
        public enum SmoothMode { Scalar, ScalarPerAxis, MaxDist, MaxDistPerAxis };
        #endregion

        #region PROPERTIES
        #region INFO
        public string _InfoKeyModes = "Modes are: " + string.Join(", ", Enum.GetNames(typeof(KeyMode)).Select(x => "" + x + ""));
        public string _InfoKeys = "For keys, use one of the key-names listed on this site: https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx";
        public string _InfoModes = "SmoothModes are: " + string.Join(", ", Enum.GetNames(typeof(SmoothMode)).Select(x => "" + x + ""));
        #endregion

        #region AIM
        public bool AimEnabled;
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys AimKey;
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyMode AimMode;
        public float AimFov;
        public bool AimLock;
        public int AimBone;
        [JsonConverter(typeof(StringEnumConverter))]
        public SmoothMode AimSmoothMode;
        public bool AimSmoothEnabled;
        public float AimSmoothScalar;
        public Vector2 AimSmoothPerAxis;
        public bool AimPredict;
        #endregion

        #region TRIGGER
        public bool TriggerEnabled;
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys TriggerKey;
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyMode TriggerMode;
        public int TriggerDelay;
        public bool TriggerBurst;
        public int TriggerBurstCount;
        public int TriggerBurstDelay;
        #endregion

        #region GLOW
        public bool GlowEnabled;
        public bool GlowShowAllies;
        public bool GlowShowEnemies;
        public bool GlowShowWeapons;
        public bool GlowShowC4;
        public bool GlowShowGrenades;
        public Color GlowAlliesColor;
        public Color GlowEnemiesColor;
        public Color GlowWeaponColor;
        public Color GlowC4Color;
        public Color GlowGrenadeColor;
        #endregion

        #region ESP
        public bool EspEnabled;
        public bool EspShowAllies;
        public bool EspShowEnemies;
        public bool EspShowWeapons;
        public bool EspShowC4;
        public bool EspShowGrenades;
        public bool EspPlayerShowName;
        public bool EspPlayerShowWeapon;
        public bool EspPlayerShowLifeArmor;
        public bool EspPlayerShowBox;
        public Color EspAlliesColor;
        public Color EspEnemiesColor;
        public Color EspWeaponColor;
        public Color EspC4Color;
        public Color EspGrenadeColor;
        #endregion
        #endregion

        #region CONSTRUCTORS
        public Settings()
        {
            AimEnabled = true;
            AimKey = Keys.MButton;
            AimMode = KeyMode.Toggle;
            AimLock = true;
            AimFov = 1f;
            AimBone = 6;
            AimPredict = true;
            AimSmoothEnabled = false;
            AimSmoothMode = SmoothMode.MaxDist;
            AimSmoothScalar = 0.2f;
            AimSmoothPerAxis = new Vector2(0.01f, 0.01f);

            TriggerEnabled = true;
            TriggerKey = Keys.XButton2;
            TriggerMode = KeyMode.Toggle;
            TriggerDelay = 20;
            TriggerBurst = false;
            TriggerBurstCount = 5;
            TriggerBurstDelay = 200;

            GlowEnabled = true;
            GlowShowAllies = true;
            GlowShowEnemies = true;
            GlowShowWeapons = true;
            GlowShowC4 = true;
            GlowShowGrenades = true;
            GlowAlliesColor = new Color() { A = 1f, R = 0f, G = 0f, B = 0.7f };
            GlowEnemiesColor = new Color() { A = 1f, R = 0.9f, G = 0.1f, B = 0f };
            GlowC4Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            GlowGrenadeColor = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            GlowWeaponColor = new Color() { A = 0.5f, R = 0f, G = 7f, B = 0f };

            EspEnabled = true;
            EspShowAllies = true;
            EspShowEnemies = true;
            EspShowWeapons = true;
            EspShowC4 = true;
            EspShowGrenades = true;
            EspAlliesColor = new Color() { A = 1f, R = 0f, G = 0f, B = 0.7f };
            EspEnemiesColor = new Color() { A = 1f, R = 0.9f, G = 0.1f, B = 0f };
            EspC4Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            EspGrenadeColor = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            EspWeaponColor = new Color() { A = 0.5f, R = 0f, G = 7f, B = 0f };
        }
        public static Settings FromFile(string file)
        {
            JsonSerializer s = new JsonSerializer();
            using (StreamReader str = new StreamReader(file))
                return (Settings)s.Deserialize(str, typeof(Settings));
        }
        #endregion

        #region METHODS
        public void Save(string file)
        {
            JsonSerializer s = new JsonSerializer();
            s.Formatting = Formatting.Indented;
            using (StreamWriter writer = new StreamWriter(file, false))
                s.Serialize(writer, this);
        }
        #endregion
    }
}
