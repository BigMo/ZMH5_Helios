using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using _ZMH5__Helios.CSGO.Enums;
using ZatsHackBase;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class BunnyHop : HackModule
    {
        public BunnyHop() : base(ModulePriority.Normal) { }

        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            var localPlayer = Program.Hack.StateMod.LocalPlayer.Value;
            if (localPlayer == null || !localPlayer.IsValid || localPlayer.m_lifeState != LifeState.Alive)
                return;
            
            if (WinAPI.GetAsyncKeyState(System.Windows.Forms.Keys.Space) != 0)
            {
                var address = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.ForceJump;
                var key = Program.Hack.Memory.Read<int>(address);

                if (((Flags)localPlayer.m_fFlags).HasFlag(Flags.FL_ONGROUND))
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
