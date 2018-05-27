using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
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

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }

        public override void Update(Time time, HackInput input, Vector2 cursorPos)
        {
            base.Update(time, input, cursorPos);
        }
    }
}
