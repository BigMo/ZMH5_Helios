using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Drawing;
using ZatsHackBase.Input;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI.Controls
{
    public class Window : Panel
    {
        public Window()
        {
            DrawBorder = true;
            DrawBackground = true;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        public override void Update(Time time, HackInput input, Vector2 cursorPos)
        {
            base.Update(time, input, cursorPos);
        }
    }
}
