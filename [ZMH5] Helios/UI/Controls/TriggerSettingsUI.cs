using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _ZMH5__Helios.CSGO.Misc;
using ZatsHackBase.Core;

namespace _ZMH5__Helios.UI.Controls
{
    public partial class TriggerSettingsUI : UserControl
    {
        private TriggerSettings settings;
        public TriggerSettings Settings
        {
            get { return settings; }
            set
            {
                if (value != settings)
                {
                    settings = value;
                    //Update!
                    if (settings != null)
                    {
                        cfgEnabled.Checked = settings.Enabled;
                        cfgBurstEnabled.Checked = settings.Burst.Enabled;
                        cfgMode.SelectedItem = settings.Mode;
                        cfgBurstCount.Value = settings.Burst.Count;
                        cfgBurstDelay.Value = settings.Burst.Delay;
                        cfgDelay.Value = settings.Delay;
                        cfgKey.Text = settings.Key.ToString();
                        cfgKeySelection.SelectedItem = settings.Key;
                    }
                }
            }
        }

        public TriggerSettingsUI()
        {
            InitializeComponent();

            cfgMode.DataSource = (KeyMode[])Enum.GetValues(typeof(KeyMode));
            cfgKeySelection.DataSource = ((Keys[])Enum.GetValues(typeof(Keys))).OrderBy(x=>x.ToString()).ToArray();

            cfgMode.SelectedValueChanged += (s, e) => { if (settings != null) settings.Mode = (KeyMode)cfgMode.SelectedValue; };
            cfgEnabled.CheckedChanged += (s, e) => { if (settings != null) settings.Enabled = cfgEnabled.Checked; };
            cfgBurstEnabled.CheckedChanged += (s, e) => { if (settings != null) settings.Burst.Enabled = cfgBurstEnabled.Checked; };

            cfgKeySelection.SelectedValueChanged += (s, e) =>
            {
                if (settings != null)
                    settings.Key = (Keys)cfgKeySelection.SelectedValue;
                cfgKey.Text = cfgKeySelection.SelectedValue.ToString();
            };

            cfgDelay.ValueChanged += (s, e) => {
                if (settings != null) settings.Delay = cfgDelay.Value;
                lblDelay.Text = cfgDelay.Value.ToString();
            };
            cfgBurstDelay.ValueChanged += (s, e) => {
                if (settings != null) settings.Burst.Delay = cfgBurstDelay.Value;
                lblBurstDelay.Text = cfgBurstDelay.Value.ToString();
            };
            cfgBurstCount.ValueChanged += (s, e) => {
                if (settings != null) settings.Burst.Count = cfgBurstCount.Value;
                lblBurstCount.Text = cfgBurstCount.Value.ToString();
            };

            cfgKey.KeyUp += (s, e) =>
            {
                if (settings != null) settings.Key = e.KeyCode;
                cfgKey.Text = e.KeyCode.ToString();
                cfgKeySelection.SelectedItem = e.KeyCode;
            };
        }
    }
}
