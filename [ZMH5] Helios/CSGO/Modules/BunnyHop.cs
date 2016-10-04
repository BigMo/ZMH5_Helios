using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using _ZMH5__Helios.CSGO.Enums;
using ZatsHackBase;
using _ZMH5__Helios.CSGO.Entities;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class BunnyHop : HackModule
    {
        public BunnyHop() : base(Program.Hack, ModulePriority.Normal) { }

        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
                return;
            
            if (WinAPI.GetAsyncKeyState(System.Windows.Forms.Keys.Space) != 0)
            {
                var address = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.ForceJump;
                var key = Program.Hack.Memory.Read<int>(address);

                if (((Flags)lp.m_fFlags).HasFlag(Flags.FL_ONGROUND))
                {
                    if (key == 4)
                        Program.Hack.Memory.Write<int>(address, 5);
                }
                else
                {
                    if (key == 5)
                        Program.Hack.Memory.Write<int>(address, 4);
                }
            }
        }
    }
}
