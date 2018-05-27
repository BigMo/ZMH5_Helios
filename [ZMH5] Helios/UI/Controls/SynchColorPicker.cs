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

namespace _ZMH5__Helios.UI.Controls
{
    public partial class SynchColorPicker : MetroUserControl, IValueObserver
    {
        public Color ColorSystem
        {
            get { return clr.BackColor; }
            set
            {
                if (value != clr.BackColor)
                {
                    clr.BackColor = value;
                    Observable?.SetValue(PropertyName, new ZatsHackBase.UI.Drawing.Color(value));
                }
            }
        }

        public void SetColor(Color value)
        {
            trckbarA.Value = (int)(value.A / 255f * 100f);
            trckbarR.Value = (int)(value.R / 255f * 100f);
            trckbarG.Value = (int)(value.G / 255f * 100f);
            trckbarB.Value = (int)(value.B / 255f * 100f);
        }

        public event EventHandler ColorChanged;

        public SynchColorPicker()
        {

            InitializeComponent();
            trckbarA.ValueChanged += UpdateColor;
            trckbarR.ValueChanged += UpdateColor;
            trckbarG.ValueChanged += UpdateColor;
            trckbarB.ValueChanged += UpdateColor;
        }

        private void UpdateColor(object sender, EventArgs e)
        {
            lblA.Text = ((float)trckbarA.Value / 100f).ToString("0.00");
            lblR.Text = ((float)trckbarR.Value / 100f).ToString("0.00");
            lblG.Text = ((float)trckbarG.Value / 100f).ToString("0.00");
            lblB.Text = ((float)trckbarB.Value / 100f).ToString("0.00");

            var color = Color.FromArgb(
                (byte)((float)trckbarA.Value / 100f * 255f),
                (byte)((float)trckbarR.Value / 100f * 255f),
                (byte)((float)trckbarG.Value / 100f * 255f),
                (byte)((float)trckbarB.Value / 100f * 255f));

            ColorSystem = color;
            ColorChanged?.Invoke(this, new EventArgs());
        }

        public void UpdateValue()
        {
            var color = ZatsHackBase.UI.Drawing.Color.White;
            if (!Observable.GetValue(PropertyName, ref color))
                return;
            trckbarA.Value = (int)(color.A * 100);
            trckbarR.Value = (int)(color.R * 100);
            trckbarG.Value = (int)(color.G * 100);
            trckbarB.Value = (int)(color.B * 100);
        }

        public string PropertyName { get; set; }

        private ValueObservable observable;
        public ValueObservable Observable
        {
            get { return observable; }
            set
            {
                if(observable != value)
                {
                    if (observable != null)
                        observable.Unsubscribe(this);

                    observable = value;
                    if(observable != null)
                    {
                        observable.Subscribe(this);
                        var color = ZatsHackBase.UI.Drawing.Color.White;
                        if (observable.GetValue(PropertyName, ref color))
                            ColorSystem = Color.FromArgb(
                                (byte)(color.A * 255f),
                                (byte)(color.R * 255f),
                                (byte)(color.G * 255f),
                                (byte)(color.B * 255f));
                    }
                }
            }
        }
    }
}
