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
    public partial class GlowSettingsUI : UserControl
    {
        public string Label
        {
            get { return lblName.Text; }
            set
            {
                lblName.Text = value;
            }
        }
        private GlowEntry entry;
        public GlowEntry Entry
        {
            get { return entry; }
            set
            {
                if (entry != value)
                {
                    entry = value;
                    if (entry != null)
                    {
                        espAlliesEnabled.Checked = entry.Enabled;
                        espAlliesColor.SetColor(entry.Color.ToColor());
                    }
                }
            }
        }
        public GlowSettingsUI()
        {
            InitializeComponent();

            this.espAlliesEnabled.CheckedChanged += (s, e) => { if (entry != null) entry.Enabled = espAlliesEnabled.Checked; };
            this.espAlliesColor.ColorChanged += (s, e) => { if (entry != null) entry.Color = new ZatsHackBase.UI.Drawing.Color(espAlliesColor.ColorSystem); };
        }
    }
}
