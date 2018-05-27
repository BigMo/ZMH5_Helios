using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override void Draw(Renderer renderer)
        {
            if (DrawBackground)
                renderer.FillRectangle(BackColor, AbsolutePosition, Size);
            if (DrawBorder)
                renderer.DrawRectangle(BorderColor, AbsolutePosition, Size);

            base.Draw(renderer);
        }
    }
}
