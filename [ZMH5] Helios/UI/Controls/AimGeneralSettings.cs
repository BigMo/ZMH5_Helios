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
using ZatsHackBase.Core;

namespace _ZMH5__Helios.UI.Controls
{
    public partial class AimGeneralSettings : MetroUserControl
    {
        private AimSettings settings;

        private enum Bones
        {
            Head = 8,
            Neck = 6,
            Pelvis = 0,
            Stomach = 5
        }

        public AimSettings Settings
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
                        cfgPredict.Checked = settings.Predict;
                        cfgSticky.Checked = settings.Sticky;
                        cfgLock.Checked = settings.Lock;
                        cfgVisible.Checked = settings.VisibleOnly;
                        cfgFov.Value = (int)(settings.FOV * 100f);
                        cfgMode.SelectedItem = settings.Mode;
                        cfgOffset.Value = (int)(settings.HeightOffset * 100f);
                        cfgBone.SelectedItem = (Bones)settings.Bone;
                        cfgKey.Text = settings.Key.ToString();
                        cfgKeySelection.SelectedItem = settings.Key;
                    }
                }
            }
        }

        public AimGeneralSettings()
        {
            InitializeComponent();

            cfgMode.DataSource = (KeyMode[])Enum.GetValues(typeof(KeyMode));
            cfgBone.DataSource = (Bones[])Enum.GetValues(typeof(Bones));
            cfgKeySelection.DataSource = ((Keys[])Enum.GetValues(typeof(Keys))).OrderBy(x => x.ToString()).ToArray();

            cfgKeySelection.SelectedValueChanged += (s, e) =>
            {
                if (settings != null)
                    settings.Key = (Keys)cfgKeySelection.SelectedValue;
                cfgKey.Text = cfgKeySelection.SelectedValue.ToString();
            };


            cfgMode.SelectedValueChanged += (s, e) => { if (settings != null) settings.Mode = (KeyMode)cfgMode.SelectedValue; };
            cfgBone.SelectedValueChanged += (s, e) => { if (settings != null) settings.Bone = (int)(Bones)cfgBone.SelectedValue; };
            cfgEnabled.CheckedChanged += (s, e) => { if (settings != null) settings.Enabled = cfgEnabled.Checked; };
            cfgPredict.CheckedChanged += (s, e) => { if (settings != null) settings.Predict = cfgPredict.Checked; };
            cfgSticky.CheckedChanged += (s, e) => { if (settings != null) settings.Sticky= cfgSticky.Checked; };
            cfgLock.CheckedChanged += (s, e) => { if (settings != null) settings.Lock = cfgLock.Checked; };
            cfgVisible.CheckedChanged += (s, e) => { if (settings != null) settings.VisibleOnly = cfgVisible.Checked; };

            cfgFov.ValueChanged += (s, e) => {
                if (settings != null) settings.FOV = cfgFov.Value / 100f;
                lblFov.Text = (cfgFov.Value / 100f).ToString("0.00");
            };

            cfgOffset.ValueChanged += (s, e) => {
                if (settings != null) settings.HeightOffset = cfgOffset.Value / 100f;
                lblOffset.Text = (cfgOffset.Value / 100f).ToString("0.00") + "cm";
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
