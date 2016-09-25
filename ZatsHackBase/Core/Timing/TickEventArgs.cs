using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core.Timing
{
    public class TickEventArgs : EventArgs
    {
        #region PROPERTIES
        public bool Stop { get; set; }
        public Time Time { get; private set; }
        public TickCounter TicksPerSecond { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public TickEventArgs(Time time, TickCounter tps)
        {
            Time = time;
            TicksPerSecond = tps;
            Stop = false;
        }
        #endregion
    }
}
