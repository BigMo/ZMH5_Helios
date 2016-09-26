using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;

namespace ZatsHackBase.GUI.Controls
{
    public class Frame : ContainerControl
    {

        public Frame(ContainerControl parent = null) : base(parent)
        {
            
        }

        public override void Render(RenderEventArgs e)
        {

            //clear bg with bgcol
            //set clip rect our client area

            foreach (var el in Controls)
                el.Render(e);

            base.Render(e);
        }
    }
}
