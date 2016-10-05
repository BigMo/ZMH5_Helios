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
        private Font font = Font.CreateDummy("Verdana", 12f);

        public Test() : base("notepad", 60, true)
        {
            this.Run();
            Overlay.BackColor = Color.Red;
            int a = 0;
        }

        protected override void BeforePluginsTick(TickEventArgs args)
        {
            base.BeforePluginsTick(args);
            
            Overlay.Renderer.DrawString(Color.Red, font, new Vector2(10f, 10f), DateTime.Now.ToLongTimeString());
        }
        static bool once = false;
        protected override void AfterPluginsTick(TickEventArgs args)
        {
            base.AfterPluginsTick(args);

            if (once)
                return;

            once = true;
        }

        protected override void SetupModules()
        {

        }
    }
}
