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
        private Font font = Font.CreateDummy("Segoe UI", 14f);

        public Test() : base("notepad", 60, true)
        {
            this.Run();
            Overlay.BackColor = Color.Red;
        }

        protected override void BeforePluginsTick(TickEventArgs args)
        {
            base.BeforePluginsTick(args);

            Overlay.Renderer.DrawString(Color.Red, font, new Vector2(300f, 10f), DateTime.Now.ToLongTimeString() + @"\n" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ\n" +
                "abcdefghijklmnopqrstuvwxyz\n" +
                "0123456789 eee");

            Overlay.Renderer.Debug(font);
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
