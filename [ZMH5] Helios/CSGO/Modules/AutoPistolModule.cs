﻿using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase;
using System.Threading;
using _ZMH5__Helios.CSGO.Entities;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class AutoPistolModule : HackModule
    {
        public AutoPistolModule() : base(Program.Hack, ModulePriority.Normal)
        { }

        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            if (!Program.CurrentSettings.MiscAutoPistol)
                return;

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
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
