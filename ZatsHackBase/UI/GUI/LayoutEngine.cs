using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Maths;
using Size = System.Drawing.Size;

namespace ZatsHackBase.GUI
{
    public class LayoutEngine
    {
        public ContainerControl Destination;

        public void Run(float wide_scroll, float high_scroll)
        {
            var bounds = Destination.Bounds;

            if (bounds.Width == 0 || bounds.Height == 0)
            {
                bounds.Size = Destination.AbsoluteBounds.Size;
            }

            var dockOffset = Destination.Margins;

            dockOffset.Left += dockOffset.Left;
            dockOffset.Top += dockOffset.Top;

            foreach (var e in Destination.Controls)
            {
                e.OverrideLayout = true;

                var dock = e.Bounds;

                var layout = new Vector2();

                switch (e.Dock)
                {
                    case DockStyle.Left:
                    {
                        dock.Left = dockOffset.Left;
                        dock.Top = dockOffset.Top;
                        dock.Height = bounds.Width - dockOffset.Right;

                        layout = dock.Location;
                        layout -= e.Bounds.Location;

                        e.LayoutChange = layout;
                        e.LayoutSize = dock.Size;

                        dockOffset.Left += dock.Width;
                    }break;
                    case DockStyle.Top:

                        dock.Left = dockOffset.Left;
                        dock.Top = dockOffset.Top;
                        dock.Width = bounds.Width - dockOffset.Right;

                        layout = dock.Location;
                        layout -= e.Bounds.Location;

                        e.LayoutChange = layout;
                        e.LayoutSize = dock.Size;

                        break;
                    case DockStyle.Bottom:
                        break;
                    case DockStyle.Right:
                        break;
                    case DockStyle.Fill:
                        break;
                    case DockStyle.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void SizeChanged(float wide_scroll, float high_scroll, Size old_size)
        {
        }
    }
}
