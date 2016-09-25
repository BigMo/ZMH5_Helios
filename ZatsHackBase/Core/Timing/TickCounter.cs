using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core.Timing
{
    public class TickCounter
    {
        #region VARIABLES
        private long lastTick;
        private int[] list = new int[1000];
        private int idx = 0;
        #endregion

        #region PROPERTIES
        public double TickSum
        {
            get { return ((double)list.Sum(x => x)) / 1000.0; }
        }
        public double TPS
        {
            get { return list.Length / Math.Max(1.0, TickSum); }
        }
        public ulong TotalTicks { get; private set; }
        public ulong TotalMS { get; private set; }
        #endregion

        public TickCounter()
        {
        }

        #region METHODS
        /// <summary>
        /// Has to be called once per frame.
        /// Keeps track of the amount of milliseconds since the last tick.
        /// </summary>
        public void Tick()
        {
            if (lastTick == 0)
                lastTick = Environment.TickCount;

            int ms = (int)(Environment.TickCount - lastTick);

            TotalTicks++;
            TotalMS += (ulong)ms;

            //Round-robin: Fill array with ms between ticks, 
            //Continue at the beginning in case we reached the end of the array
            list[idx] = ms;
            idx++;
            idx %= list.Length;

            //Remember our last tick
            lastTick = Environment.TickCount;
        }
        #endregion
    }
}
