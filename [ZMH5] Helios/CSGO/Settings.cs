using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _ZMH5__Helios.CSGO
{
    public class Settings
    {
        #region ENUMS
        public enum eMode { Hold, Toggle }
        #endregion

        #region PROPERTIES
        #region INFO
        public string _InfoModes = "Modes are: \"Hold\" and \"Toggle\"";
        public string _InfoKeys = "For keys, use one of the key-names listed on this site: https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx";
        #endregion

        #region AIM
        public bool AimEnabled { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys AimKey { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public eMode AimMode { get; set; }
        public float AimFov { get; set; }
        public bool AimLock { get; set; }
        public int AimBone { get; set; }
        public float AimSmooth { get; set; }
        #endregion

        #region TRIGGER
        public bool TriggerEnabled { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys TriggerKey { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public eMode TriggerMode { get; set; }
        public int TriggerDelay { get; set; }
        public bool TriggerBurst { get; set; }
        public int TriggerBurstCount { get; set; }
        public int TriggerBurstDelay { get; set; }
        #endregion

        #region GLOW
        public bool GlowEnabled { get; set; }
        public bool GlowShowAllies { get; set; }
        public bool GlowShowEnemies { get; set; }
        public bool GlowShowWeapons { get; set; }
        public bool GlowShowC4 { get; set; }
        public bool GlowShowGrenades { get; set; }
        public Color GlowAlliesColor { get; set; }
        public Color GlowEnemiesColor { get; set; }
        public Color GlowWeaponColor { get; set; }
        public Color GlowC4Color { get; set; }
        public Color GlowGrenadeColor { get; set; }
        public bool GlowFade { get; set; }
        public float GlowFadeDistance { get; set; }
        #endregion
        #endregion

        #region CONSTRUCTORS
        public Settings()
        {
            AimEnabled = true;
            AimKey = Keys.MButton;
            AimMode = eMode.Toggle;
            AimLock = true;
            AimFov = 1f;
            AimBone = 6;

            TriggerEnabled = true;
            TriggerKey = Keys.XButton2;
            TriggerMode = eMode.Toggle;
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
            GlowFade = false;
            GlowFadeDistance = 500f;
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
