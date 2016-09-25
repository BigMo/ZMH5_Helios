using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase;
using System.Threading;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class AutoPistol : HackModule
    {
        public AutoPistol() : base(ModulePriority.Normal)
        { }

        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (lp == null || !lp.IsValid || lp.m_lifeState.Value != Enums.LifeState.Alive)
                return;

            var wep = lp.m_ActiveWeapon.Value;
            if (wep == null || !wep.IsValid || !wep.IsPistol)
                return;

            var address = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.ForceAttack;
            var key = Program.Hack.Memory.Read<int>(address);
            if (WinAPI.GetAsyncKeyState(System.Windows.Forms.Keys.LButton) != 0)
            {
                if (wep.m_iClip1 > 0)
                {

                    if (key == 4)
                        Program.Hack.Memory.Write<int>(address, 5);
                    else if (key == 5)
                        Program.Hack.Memory.Write<int>(address, 4);
                }
            } else
            {
                if (key == 5)
                    Program.Hack.Memory.Write<int>(address, 4);

            }

        }
        private void Shoot()
        {
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(1);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
    }
}
