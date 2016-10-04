using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.GUI.Controls;
using ZatsHackBase.Maths;
using ZatsHackBase.UI;
using ZatsHackBase.UI.Drawing;

namespace _ZMH5__Helios
{
    public class Test : Hack
    {
        #region CONSTRUCTORS 

        public Test() : base("notepad", 60, true)
        {
            this.Run();
        }

        #endregion 

        #region VARIABLES 

        private Font font;
        static bool once = false;

        private Frame mainFrame;
        private Frame titleFrame;
        private Frame clientFrame;

        #endregion

        #region METHODS

        private void InitializeMenu()
        {
            
        }

        public override void BeforePluginsTick(TickEventArgs args)
        {
            base.BeforePluginsTick(args);
            if (font == null)
                font = Overlay.Renderer.CreateFont("Verdana", 8);
            Overlay.Renderer.DrawString(new Color(1f, 0f, 0f), font, new Vector2(10f, 10f),DateTime.Now.ToLongTimeString());
        }

        public override void AfterPluginsTick(TickEventArgs args)
        {
            base.AfterPluginsTick(args);

            if (once)
                return;

            once = true;
        }

        #endregion 
    }
}
