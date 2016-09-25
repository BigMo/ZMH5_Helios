using ZatsHackBase.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZatsHackBase.Core.Timing
{
    class LoopTicker
    {
        #region VARIABLES
        private int tickRate, timePerTick;
        private TickCounter counter;
        #endregion

        #region PROPERTIES
        public int TickRate
        {
            get { return tickRate; }
            set
            {
                if (tickRate != value)
                {
                    tickRate = Math.Max(1, value);
                    timePerTick = (int)System.Math.Floor(1000.0 / tickRate);
                }
            }
        }
        public int TimePerTick
        {
            get { return timePerTick; }
            set
            {
                if (timePerTick != value)
                {
                    timePerTick = Math.Max(1, value);
                    tickRate = (int)System.Math.Floor(1000.0 / timePerTick);
                }
            }
        }
        public bool LimitFrames { get; set; }
        public double TicksPerSeconds { get { return counter.TPS; } }
        #endregion

        #region EVENTS
        public EventHandler<TickEventArgs> Tick { get; set; }
        public EventHandler<EventArgs> BeforeRun { get; set; }
        public EventHandler<EventArgs> AfterRun { get; set; }
        #endregion

        #region CONSTRUCTORS
        public LoopTicker()
        {
            counter = new TickCounter();
            LimitFrames = true;
            TickRate = 60;
        }
        #endregion

        #region METHODS
        public void Run()
        {
            BeforeRun?.Invoke(this, new EventArgs());
            int lastTick = Environment.TickCount;
            int firstTick = lastTick;
            int currentTick = 0;
            while (true)
            {
                currentTick = Environment.TickCount;

                counter.Tick();
                TickEventArgs args = new TickEventArgs(new Time(TimeSpan.FromMilliseconds(Environment.TickCount - firstTick), TimeSpan.FromMilliseconds(currentTick - lastTick)), counter);
                Tick?.Invoke(this, args);
                if (args.Stop)
                    break;
                lastTick = Environment.TickCount;

                if (this.LimitFrames)
                {
                    long passed = lastTick - currentTick + 1;
                    if (passed <= timePerTick)
                        Thread.Sleep((int)(timePerTick - passed));
                }
            }
            AfterRun?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
