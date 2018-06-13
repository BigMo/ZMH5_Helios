using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Drawing;
using ZatsHackBase.UI.Controls.Layouts;

namespace ZatsHackBase.UI.Controls
{
    public class Panel : Control
    {
        public Panel()
        {
            DrawText = false;
            DrawBackground = DrawBorder = true;
            //Layout = LinearLayout.Instance;
        }

        public override void Draw(Graphics g)
        {
            if (DrawBackground)
                g.FillRectangle(BackColor, AbsolutePosition, Size);
            if (DrawBorder)
                g.DrawRectangle(BorderColor, AbsolutePosition, Size);

            base.Draw(g);
        }
    }
}
