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
    public partial class SynchToggle : MetroUserControl, IValueObserver
    {
        public string PropertyName { get; set; }
        public bool Checked
        {
            get { return metroToggle1.Checked; }
            set
            {
                if (value != metroToggle1.Checked)
                {
                    metroToggle1.Checked = value;
                }
            }
        }

        public event EventHandler CheckedChanged;
        private ValueObservable observable;

        public SynchToggle()
        {
            InitializeComponent();
            this.metroToggle1.CheckedChanged += (s, e) =>
            {
                observable?.SetValue(PropertyName, metroToggle1.Checked);
                CheckedChanged?.Invoke(this, new EventArgs());
            };
        }

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
                    if (observable != null)
                        observable.Subscribe(this);
                }
            }
        }
        
        public void UpdateValue()
        {
            bool val = false;
            if (!Observable.GetValue(PropertyName, ref val))
                return;

            this.metroToggle1.Checked = val;
        }
    }
}
