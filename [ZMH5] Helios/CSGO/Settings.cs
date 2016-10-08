using _ZMH5__Helios.CSGO.Misc;
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
        public ESPSettings EspAllies;
        public ESPSettings EspEnemies;
        public ESPSettings EspChickens;
        public ESPSettings EspWeapons;
        public ESPSettings EspGrenades;
        public ESPSettings EspC4;
        public bool EspC4ShowTime;
        public bool EspPlayersShowWeapon;
        #endregion

        #region MISC
        public bool MiscAutoPistol;
        public bool MiscBunnyHop;
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

            EspAllies = new ESPSettings();
            EspC4 = new ESPSettings();
            EspChickens = new ESPSettings();
            EspEnemies = new ESPSettings();
            EspGrenades = new ESPSettings();
            EspWeapons = new ESPSettings();

            EspAllies.Enabled = true;
            EspC4.Enabled = true;
            EspChickens.Enabled = true;
            EspEnemies.Enabled = true;
            EspGrenades.Enabled = true;
            EspWeapons.Enabled = true;

            EspAllies.ShowBox = true;
            EspC4.ShowBox = true;
            EspChickens.ShowBox = true;
            EspEnemies.ShowBox = true;
            EspGrenades.ShowBox = true;
            EspWeapons.ShowBox = true;

            EspAllies.ShowName = true;
            EspC4.ShowName = true;
            EspChickens.ShowName = true;
            EspEnemies.ShowName = true;
            EspGrenades.ShowName = true;
            EspWeapons.ShowName = true;

            EspAllies.ShowDist = true;
            EspC4.ShowDist = true;
            EspChickens.ShowDist = true;
            EspEnemies.ShowDist = true;
            EspGrenades.ShowDist = true;
            EspWeapons.ShowDist = true;

            EspAllies.ShowLifeArmor = true;
            EspC4.ShowLifeArmor = false;
            EspChickens.ShowLifeArmor = true;
            EspEnemies.ShowLifeArmor = true;
            EspGrenades.ShowLifeArmor = false;
            EspWeapons.ShowLifeArmor = false;

            EspAllies.Color = new Color() { A = 1f, R = 0f, G = 0f, B = 0.7f };
            EspC4.Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            EspChickens.Color = Color.FromKnownColor(Color.Orange, 0.9f);
            EspEnemies.Color = new Color() { A = 1f, R = 0.9f, G = 0.1f, B = 0f };
            EspGrenades.Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            EspWeapons.Color = new Color() { A = 0.5f, R = 0f, G = 7f, B = 0f };

            MiscAutoPistol = true;
            MiscBunnyHop = true;
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
