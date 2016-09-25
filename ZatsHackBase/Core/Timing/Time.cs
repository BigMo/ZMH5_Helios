using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core.Timing
{
    public struct Time
    {
        public TimeSpan TotalTime { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public Time(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            TotalTime = totalTime;
            ElapsedTime = elapsedTime;
        }
    }
}
