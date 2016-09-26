using ZatsHackBase.Core.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.UI;
using System.Threading;

namespace ZatsHackBase.Core
{
    public abstract class Hack
    {
        #region VARIABLES
        private LoopTicker ticker;
        private List<HackModule> modules;
        #endregion

        #region PROPERTIES
        public EUCProcess Process { get; private set; }
        public Memory Memory { get { return Process.Memory; } }
        public HackOverlay Overlay { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public Hack (string processName, int tickRate = 60, bool createOverlay = true, int timeOut = -1)
        {
            Process = EUCProcess.WaitForProcess(processName, timeOut);
            ticker = new LoopTicker();
            ticker.TickRate = tickRate;
            modules = new List<HackModule>();
            if (createOverlay)
            {
                Overlay = new HackOverlay(Process);
                Overlay.Start();
            }
            
            ticker.Tick += (o, e) =>
            {
                if (!Process.IsRunning)
                {
                    e.Stop = true;
                    return;
                }

                OnTick(e);
            };
            ticker.AfterRun += (o, e) => AfterRun();
            ticker.BeforeRun += (o, e) => BeforeRun();
        }
        #endregion

        #region METHODS
        protected void AddModule(HackModule mod)
        {
            modules.Add(mod);
        }
        protected void RemoveModule(HackModule mod)
        {
            modules.Remove(mod);
        }
        public void Run()
        {
            ticker.Run();
        }

        public virtual void AfterRun() { }
        public virtual void BeforeRun() { }
        public virtual void OnTick(TickEventArgs args)
        {
            Process.Process.Refresh();
            if(!Process.IsRunning)
            {
                args.Stop = true;
                return;
            }
            BeforePluginsTick(args);

            foreach (var mod in modules.OrderByDescending(x => x.Priority))
                mod.Update(args);

            AfterPluginsTick(args);
        }
        public virtual void BeforePluginsTick(TickEventArgs args)
        {
            if (Overlay != null)
            {
                Overlay.AdjustForm();
                Overlay.Renderer.Clear(Overlay.BackColor);
                //Overlay.Renderer.GeometryBuffer.ClipRegion = new Maths.Rectangle();
            }
        }
        public virtual void AfterPluginsTick(TickEventArgs args)
        {
            if (Overlay != null)
                Overlay.Renderer.Present();
        }
        #endregion
    }
}
