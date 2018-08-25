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
using ZatsHackBase.Drawing;

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

        public bool DebugShowBones;

        public AimSettings Aim;

        public TriggerSettings Trigger;
        public GlowSettings Glow;
        public ESPSettings ESP;
        public NoRecoilSettings NoRecoil;
        public bool IsActive;
        public string Name;

        #region MISC
        public bool MiscAutoPistol;
        public bool MiscBunnyHop;
        #endregion
        #endregion

        #region CONSTRUCTORS
        public Settings(string name = "dummySettings")
        {
            Name = name;
            Aim = new AimSettings();
            Aim.Key = Keys.MButton;
            Aim.Mode = KeyMode.Toggle;
            Aim.Lock = true;
            Aim.FOV= 1f;
            Aim.Bone = 6;
            Aim.Predict = true;
            Aim.Sticky = true;
            Aim.VisibleOnly = true;
            Aim.Enabled = true;
            
            Aim.Smoothing = new AimSmooth();
            Aim.Smoothing.Enabled = false;
            Aim.Smoothing.Mode = SmoothMode.MaxDist;
            Aim.Smoothing.Scalar = 0.2f;
            Aim.Smoothing.PerAxis = new Vector2(0.01f, 0.01f);

            Trigger = new TriggerSettings();
            Trigger.Enabled = true;
            Trigger.Key = Keys.XButton2;
            Trigger.Mode = KeyMode.Toggle;
            Trigger.Delay = 20;

            Trigger.Burst = new BurstSettings();
            Trigger.Burst.Enabled = false;
            Trigger.Burst.Count = 5;
            Trigger.Burst.Delay = 200;

            Glow = new GlowSettings();

            Glow.Allies = new GlowEntry();
            Glow.Enemies = new GlowEntry();
            Glow.C4 = new GlowEntry();
            Glow.Grenades = new GlowEntry();
            Glow.Weapons = new GlowEntry();

            Glow.Enabled = true;
            Glow.Allies.Enabled = true;
            Glow.Enemies.Enabled = true;
            Glow.C4.Enabled = true;
            Glow.Grenades.Enabled = true;
            Glow.Weapons.Enabled = true;
            Glow.Allies.Color = new Color() { A = 1f, R = 0f, G = 0f, B = 0.7f };
            Glow.Enemies.Color = new Color() { A = 1f, R = 0.9f, G = 0.1f, B = 0f };
            Glow.C4.Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            Glow.Grenades.Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            Glow.Weapons.Color = new Color() { A = 0.5f, R = 0f, G = 7f, B = 0f };

            ESP = new ESPSettings();

            ESP.Allies = new ESPEntry();
            ESP.C4 = new ESPEntry();
            ESP.Chickens = new ESPEntry();
            ESP.Enemies = new ESPEntry();
            ESP.Grenades = new ESPEntry();
            ESP.Weapons = new ESPEntry();
            ESP.World = new ESPEntry();

            ESP.Allies.Enabled = true;
            ESP.C4.Enabled = true;
            ESP.Chickens.Enabled = true;
            ESP.Enemies.Enabled = true;
            ESP.Grenades.Enabled = true;
            ESP.Weapons.Enabled = true;

            ESP.Allies.ShowBox = true;
            ESP.C4.ShowBox = true;
            ESP.Chickens.ShowBox = true;
            ESP.Enemies.ShowBox = true;
            ESP.Grenades.ShowBox = true;
            ESP.Weapons.ShowBox = true;

            ESP.Allies.ShowName = true;
            ESP.C4.ShowName = true;
            ESP.Chickens.ShowName = true;
            ESP.Enemies.ShowName = true;
            ESP.Grenades.ShowName = true;
            ESP.Weapons.ShowName = true;

            ESP.Allies.ShowDist = true;
            ESP.C4.ShowDist = true;
            ESP.Chickens.ShowDist = true;
            ESP.Enemies.ShowDist = true;
            ESP.Grenades.ShowDist = true;
            ESP.Weapons.ShowDist = true;

            ESP.Allies.ShowHealth = true;
            ESP.C4.ShowHealth = false;
            ESP.Chickens.ShowHealth = true;
            ESP.Enemies.ShowHealth = true;
            ESP.Grenades.ShowHealth = false;
            ESP.Weapons.ShowHealth = false;

            ESP.Allies.Color = new Color() { A = 1f, R = 0f, G = 0f, B = 0.7f };
            ESP.C4.Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            ESP.Chickens.Color = Color.FromKnownColor(Color.Orange, 0.9f);
            ESP.Enemies.Color = new Color() { A = 1f, R = 0.9f, G = 0.1f, B = 0f };
            ESP.Grenades.Color = new Color() { A = 0.9f, R = 1f, G = 0f, B = 0f };
            ESP.Weapons.Color = new Color() { A = 0.5f, R = 0f, G = 7f, B = 0f };

            NoRecoil = new NoRecoilSettings();
            NoRecoil.Enabled = true;
            NoRecoil.Force = 1f;
            NoRecoil.ShowCrosshair = false;
            NoRecoil.NoRecoilSemiAuto = false;

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
        public void Save()
        {
            var file = Path.Combine("configs", Name + ".json");
            JsonSerializer s = new JsonSerializer();
            s.Formatting = Formatting.Indented;
            using (StreamWriter writer = new StreamWriter(file, false))
                s.Serialize(writer, this);
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
