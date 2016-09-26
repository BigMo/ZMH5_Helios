using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var margins = Destination.Margins;

            bounds.Left += margins.Left;
            bounds.Top += margins.Top;
            bounds.Width -= margins.Left + margins.Right;
            bounds.Height -= margins.Top + margins.Bottom;
        }

        public void SizeChanged(float wide_scroll, float high_scroll, Size old_size)
        {
            
        }
    }
}
