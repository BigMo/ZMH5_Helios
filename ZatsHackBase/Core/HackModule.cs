using ZatsHackBase.Core.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    public abstract class HackModule
    {
        #region VARIABLES
        private bool firstRun;
        private DateTime lastTimer;
        #endregion

        #region PROPERTIES
        public ModulePriority Priority { get; private set; }
        protected bool TimerEnabled { get; set; }
        protected int TimerInterval { get; set; }
        protected Hack Hack { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public HackModule(Hack hack, ModulePriority prio)
        {
            Hack = hack;
            this.Priority = prio;
            TimerEnabled = false;
            TimerInterval = (int)(1000f / 60f);
        }
        #endregion

        #region METHODS
        public void Update(TickEventArgs args)
        {
            if (!firstRun)
            {
                firstRun = true;
                OnFirstRun(args);
            }
            OnUpdate(args);
            if (TimerEnabled)
            {
                var elapsed = DateTime.Now - lastTimer;
                if (elapsed.TotalMilliseconds >= TimerInterval)
                    OnTimer(args, new Time(args.Time.TotalTime, elapsed));
            }
        }
        protected virtual void OnFirstRun(TickEventArgs args)
        {
            lastTimer = DateTime.Now;
        }
        protected virtual void OnUpdate(TickEventArgs args) { }
        protected virtual void OnTimer(TickEventArgs args, Time sinceLastTimer)
        {
            lastTimer = DateTime.Now;
        }
        #endregion
    }
}
