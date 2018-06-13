using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Misc;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class EchoModule : HackModule
    {
        #region VARIABLES
        private long lastIn, lastOut;
        #endregion

        #region CONSTRUCTORS
        public EchoModule() : base(Program.Hack, ZatsHackBase.Core.ModulePriority.Low)
        {
            TimerEnabled = true;
            TimerInterval = 1000;
        }
        #endregion

        #region METHODS
        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            lastIn = Program.Hack.Memory.BytesIn;
            lastOut = Program.Hack.Memory.BytesOut;
        }
        protected override void OnTimer(TickEventArgs args, Time sinceLastTimer)
        {
            base.OnTimer(args, sinceLastTimer);

            Program.Logger.PrintLine("ECHO", "{5}T\t | ▼{1} ({4})\t | ▲{0} ({3})\t | {2}t/s", new object[]
            {
                    SizeFormatter.GetUnitFromSize(Program.Hack.Memory.BytesOut, true),
                    SizeFormatter.GetUnitFromSize(Program.Hack.Memory.BytesIn, true),
                    ((int)Program.Hack.TicksPerSecond).ToString(),
                    //(int)args.TicksPerSecond.TPS,
                    SizeFormatter.GetUnitFromSize(Program.Hack.Memory.BytesOut - lastOut, true),
                    SizeFormatter.GetUnitFromSize(Program.Hack.Memory.BytesIn - lastIn, true),
                    args.TicksPerSecond.TotalTicks.ToString().PadLeft(6,' ')
             });

            lastIn = Program.Hack.Memory.BytesIn;
            lastOut = Program.Hack.Memory.BytesOut;
        }
        #endregion
    }
}
