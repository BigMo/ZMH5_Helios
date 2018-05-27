using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.UI.Controls
{
    public class ControlConverter
    {
        public static Window Convert(Form form)
        {
            var window = new Window();
            GrabBasics(form, window);
            Crawl(window, form);
            return window;
        }

        private static void Crawl(Control to, System.Windows.Forms.Control from)
        {
            foreach(System.Windows.Forms.Control c in from.Controls)
            {
                Control t = null;
                var type = c.GetType();
                if (type == typeof(System.Windows.Forms.Panel))
                    t = new Panel();
                else if (type == typeof(System.Windows.Forms.Label))
                    t = new Label();
                else
                {
                    Debug.WriteLine("Unknown control type \"{0}\"; skipping", type.Name);
                    continue;
                }
                GrabBasics(c, t);
                to.AddChild(t);
                Crawl(t, c);
            }
        }

        private static void GrabBasics(System.Windows.Forms.Control control, Control dest)
        {
            var props = control.GetType().GetProperties();
            var borderStyle = props.FirstOrDefault(x => x.Name == "BorderStyle");

            dest.ForeColor = new Color(control.ForeColor);
            dest.BackColor = new Color(control.BackColor);
            dest.BorderColor = dest.ForeColor;
            if (borderStyle != null)
                dest.DrawBorder = ((BorderStyle)borderStyle.GetValue(control)) != BorderStyle.None;
            dest.Font = Font.CreateDummy(control.Font.FontFamily.Name, control.Font.Size, control.Font.Bold, control.Font.Italic);
            dest.Text = control.Text;
            dest.Position = new Maths.Vector2(control.Location.X, control.Location.Y);
            dest.Size = new Maths.Vector2(control.Size.Width, control.Size.Height);
            dest.Margin = new Distance(control.Margin.Top, control.Margin.Left, control.Margin.Right, control.Margin.Bottom);
            dest.Padding = new Distance(control.Padding.Top, control.Padding.Left, control.Padding.Right, control.Padding.Bottom);
            dest.Name = control.Name;
            //dest.Visible = control.Visible;
            dest.Enabled = control.Enabled;
        }
    }
}
