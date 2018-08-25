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

namespace _ZMH5__Helios.UI.Controls
{
    public partial class RCSSettings : UserControl
    {
        private NoRecoilSettings settings;
        public NoRecoilSettings Settings
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
                        cfgForce.Value = (int)(settings.Force * 100f);
                        cfgCrosshairEnable.Checked = settings.ShowCrosshair;
                        cfgNoRecoilSemiAuto.Checked = settings.NoRecoilSemiAuto;
                    }
                }
            }
        }

        public RCSSettings()
        {
            InitializeComponent();

            cfgEnabled.CheckedChanged += (s, e) => { if (settings != null) settings.Enabled = cfgEnabled.Checked; };

            cfgForce.ValueChanged += (s, e) => {
                if (settings != null) settings.Force = cfgForce.Value / 100f;
                lblForce.Text = (cfgForce.Value / 100f).ToString("0.00");
            };

            cfgCrosshairEnable.CheckedChanged += (s, e) => { if (settings != null) settings.ShowCrosshair = cfgCrosshairEnable.Checked; };

            cfgNoRecoilSemiAuto.CheckedChanged += (s, e) => { if (settings != null) settings.NoRecoilSemiAuto = cfgNoRecoilSemiAuto.Checked; };
        }
    }
}
