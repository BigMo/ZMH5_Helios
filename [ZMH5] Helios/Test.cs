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
        #region VARIABLES
        private Font dbg = Font.CreateDummy("Segoe UI", 14);
        #endregion
        
        #region CONSTRUCTORS
        public Test() : base("notepad", 999, true, false)
        {
            
        }
        #endregion

        #region METHODS
        protected override void OnFirstTick(TickEventArgs args)
        {
            base.OnFirstTick(args);
            //Overlay.Renderer.Init(Overlay.Form);
        }
        
        protected override bool ProcessInput()
        {
            return Process.IsInForeground;
        }

        protected override void AfterRun()
        {
            base.AfterRun();

            Program.Logger.Info("Terminating.");
        }
        
        protected override void AfterPluginsTick(TickEventArgs args)
        {
            if (!Overlay.Renderer.Initialized)
                return;
            dbg = Overlay.Renderer.Fonts[dbg];

            //if (Process.IsInForeground)
            {
                Overlay.Renderer.DrawRectangle(Color.Green, new Vector2(100f, 10f), new Vector2(100f, 100f));
                Overlay.Renderer.DrawString(Color.Red, dbg, new Vector2(95f, 10f), DateTime.Now.ToLongTimeString());

            }
            base.AfterPluginsTick(args);
        }

        protected override void SetupModules()
        {
        }
        #endregion
    }
}
