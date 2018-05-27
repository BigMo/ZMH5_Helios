using _ZMH5__Helios.CSGO;
using _ZMH5__Helios.UI.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _ZMH5__Helios.UI
{
    public partial class ConfigForm : MetroForm
    {
        private Settings settings;
        public Settings Settings
        {
            get { return settings; }
            set
            {
                if (settings != value)
                {
                    settings = value;
                    cfgRemove.Enabled = cfgSave.Enabled = settings != null;

                    if (settings != null)
                    {
                        Program.CurrentSettings = settings;
                        espChickens.Entry = settings.ESP.Chickens;
                        espAllies.Entry = settings.ESP.Allies;
                        espEnemies.Entry = settings.ESP.Enemies;
                        espGrenades.Entry = settings.ESP.Grenades;
                        espC4.Entry = settings.ESP.C4;
                        espWeapons.Entry = settings.ESP.Weapons;
                        glowChickens.Entry = settings.Glow.Chickens;
                        glowAllies.Entry = settings.Glow.Allies;
                        glowEnemies.Entry = settings.Glow.Enemies;
                        glowGrenades.Entry = settings.Glow.Grenades;
                        glowC4.Entry = settings.Glow.C4;
                        glowWeapons.Entry = settings.Glow.Weapons;
                        aimGeneralSettings1.Settings = settings.Aim;
                        aimSmoothingSettings1.Settings = settings.Aim.Smoothing;
                        triggerSettingsUI1.Settings = settings.Trigger;
                        rcsSettings1.Settings = settings.NoRecoil;
                    }
                }
            }
        }


        public ConfigForm()
        {
            this.InitializeComponent();

            this.cfgAll.SelectedIndexChanged += (s, e) =>
            {
                if (Settings != null)
                {
                    Settings.IsActive = false;
                    Settings.Save();
                }
                Settings = (Settings)cfgAll.SelectedItem;
                if (Settings != null)
                {
                    Settings.IsActive = true;
                    Settings.Save();
                }
            };
            this.cfgNewName.TextChanged += (s, e) =>
            {
                var name = cfgNewName.Text;
                foreach (var c in Path.GetInvalidFileNameChars())
                    name = name.Replace(c.ToString(), "");
                cfgNewName.Text = name;

                cfgNewCreate.Enabled = name.Length > 0;
            };
            this.cfgNewCreate.Click += (s, e) =>
            {
                var settings = new Settings(cfgNewName.Text);
                settings.Save();
                Program.LoadSettings();
                cfgAll.DataSource = Program.AllSettings;
                cfgAll.SelectedItem = settings;
                cfgNewName.Text = "";
            };
            cfgSave.Click += (s, e) => Settings.Save();

            cfgAll.DataSource = Program.AllSettings;
            cfgAll.SelectedItem= Program.CurrentSettings;
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = e.CloseReason != CloseReason.ApplicationExitCall;
        }
    }
}
