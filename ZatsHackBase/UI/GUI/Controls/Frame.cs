using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.GUI.Controls
{
    public class Frame : ContainerControl
    {

        public Frame(ContainerControl parent = null) : base(parent)
        {
            
        }

        public override void Render(RenderEventArgs e)
        {
            if (BackColor != Color.Transparent) 
                e.Renderer.FillRectangle(BackColor, AbsoluteBounds.Location, AbsoluteBounds.Size);

            //clear bg with bgcol
            //set clip rect our client area

            foreach (var el in Controls)
                el.Render(e);

            base.Render(e);
        }
    }
}
