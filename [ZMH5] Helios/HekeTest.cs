using _ZMH5__Helios.CSGO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;

namespace _ZMH5__Helios
{
    class HekeTest : Hack
    {
        public HekeTest() : base("notepad", 60, true)
        {
            Overlay.BackColor = new Color(0.25f, 0f, 0f, 0f);
        }

        public override void OnTick(TickEventArgs args)
        {
            base.OnTick(args);
        }

        public override void BeforePluginsTick(TickEventArgs args)
        {
            base.BeforePluginsTick(args);

            //this.Overlay.Renderer.DrawRectangle(Color.White, Vector2.Zero, Vector2.Unit * 10);
            this.Overlay.Renderer.DrawLine(Color.Red, Vector2.Zero, Vector2.Unit * 1000f);
        }
    }
}
