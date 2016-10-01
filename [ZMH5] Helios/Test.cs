using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using ZatsHackBase.UI;
using ZatsHackBase.UI.Drawing;

namespace _ZMH5__Helios
{
    public class Test : Hack
    {
        public Test() : base("notepad", 60, true)
        {
            this.Run();
        }

        private Font font;
        public override void BeforePluginsTick(TickEventArgs args)
        {
            base.BeforePluginsTick(args);
            if (font == null)
                font = Overlay.Renderer.CreateFont("Verdana", 12);
            Overlay.Renderer.DrawString(new Color(1f, 0f, 1f), font, new Vector2(10f, 10f),DateTime.Now.ToLongTimeString());
        }
        static bool once = false;
        public override void AfterPluginsTick(TickEventArgs args)
        {
            base.AfterPluginsTick(args);

            if (once)
                return;

            once = true;
        }
    }
}
