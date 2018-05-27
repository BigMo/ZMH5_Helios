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
    public partial class ESPSettings : UserControl
    {
        public string Label
        {
            get { return lblName.Text; }
            set
            {
                lblName.Text = value;
            }
        }
        private ESPEntry entry;
        public ESPEntry Entry
        {
            get { return entry; }
            set
            {
                if(entry != value)
                {
                    entry = value;
                    if (entry != null)
                    {
                        espAlliesEnabled.Checked = entry.Enabled;
                        espAlliesBox.Checked = entry.ShowBox;
                        espAlliesName.Checked = entry.ShowName;
                        espAlliesDistance.Checked = entry.ShowDist;
                        espAlliesHealth.Checked = entry.ShowHealth;
                        espAlliesWeapon.Checked = entry.ShowWeapon;
                        espAlliesColor.SetColor(entry.Color.ToColor());
                    }
                }
            }
        }

        public ESPSettings()
        {
            InitializeComponent();

            this.espAlliesBox.CheckedChanged += (s, e) => { if (entry != null) entry.ShowBox = espAlliesBox.Checked; };
            this.espAlliesName.CheckedChanged += (s, e) => { if (entry != null) entry.ShowName = espAlliesName.Checked; };
            this.espAlliesDistance.CheckedChanged += (s, e) => { if (entry != null) entry.ShowDist = espAlliesDistance.Checked; };
            this.espAlliesEnabled.CheckedChanged += (s, e) => { if (entry != null) entry.Enabled = espAlliesEnabled.Checked; };
            this.espAlliesHealth.CheckedChanged += (s, e) => { if (entry != null) entry.ShowHealth = espAlliesHealth.Checked; };
            this.espAlliesWeapon.CheckedChanged += (s, e) => { if (entry != null) entry.ShowWeapon = espAlliesWeapon.Checked; };
            this.espAlliesColor.ColorChanged += (s, e) => { if (entry != null) entry.Color = new ZatsHackBase.UI.Drawing.Color(espAlliesColor.ColorSystem); };
        }
    }
}
