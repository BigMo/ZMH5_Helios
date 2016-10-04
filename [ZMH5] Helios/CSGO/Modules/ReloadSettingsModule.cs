using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class ReloadSettingsModule : HotkeyModule
    {
        public ReloadSettingsModule() : base(Program.Hack, ModulePriority.Low)
        {
        }

        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            Hotkey = System.Windows.Forms.Keys.Insert;
            Mode = KeyMode.Hold;
        }

        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            if (ActiveByHotkey)
            {
                Reset();

                Program.LoadSettings();
            }
        }
    }
}
