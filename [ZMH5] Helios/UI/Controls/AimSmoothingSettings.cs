using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using _ZMH5__Helios.CSGO.Misc;
using static _ZMH5__Helios.CSGO.Settings;

namespace _ZMH5__Helios.UI.Controls
{
    public partial class AimSmoothingSettings : MetroUserControl
    {
        private AimSmooth settings;

        public AimSmooth Settings
        {
            get { return settings; }
            set
            {
                if(value != settings)
                {
                    settings = value;
                    //Update!
                    if(settings != null)
                    {
                        cfgEnable.Checked = settings.Enabled;
                        cfgScalar.Value = (int)(settings.Scalar * 100f);
                        cfgAxisX.Value = (int)(settings.PerAxis.X*100f);
                        cfgAxisY.Value = (int)(settings.PerAxis.Y * 100f);
                        cfgMode.SelectedItem = settings.Mode;
                    }
                }
            }
        }

        public AimSmoothingSettings()
        {
            InitializeComponent();

            cfgMode.DataSource = (SmoothMode[])Enum.GetValues(typeof(SmoothMode));
            cfgEnable.CheckedChanged += (s, e) => { if (settings != null) settings.Enabled = cfgEnable.Checked; };
            cfgMode.SelectedValueChanged += (s, e) => { if (settings != null) settings.Mode = (SmoothMode)cfgMode.SelectedItem; };
            cfgScalar.ValueChanged += (s, e) => {
                if (settings != null) settings.Scalar = cfgScalar.Value / 100f;
                lblScalar.Text = (cfgScalar.Value / 100f).ToString("0.00");
            };
            cfgAxisX.ValueChanged += (s, e) => {
                if (settings != null) settings.PerAxis.X = cfgAxisX.Value / 100f;
                lblAxisX.Text = (cfgAxisX.Value / 100f).ToString("0.00");
            };
            cfgAxisY.ValueChanged += (s, e) => {
                if (settings != null) settings.PerAxis.Y = cfgAxisY.Value / 100f;
                lblAxisY.Text = (cfgAxisY.Value / 100f).ToString("0.00");
            };
        }
    }
}
