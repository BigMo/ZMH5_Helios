using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using _ZMH5__Helios.CSGO.Entities;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class NoRecoilModule : HackModule
    {
        #region VARIABLES
        private int lastClip = 0;
        private Vector3 lastPunch = Vector3.Zero;
        #endregion

        public NoRecoilModule() : base(Program.Hack, ModulePriority.Normal)
        {
        }

        #region METHODS
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);
            if (!Program.CurrentSettings.NoRecoil.Enabled)
                return;

            //TODO: Fix...
            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
                return;

            var wep = lp.m_ActiveWeapon.Value;
            if (wep == null || !wep.IsValid)
                return;

            if (Program.Hack.AimBot.CurrentTarget == 0)
            {
                var vecPunch = lp.m_aimPunchAngle.Value;
                var delta = (vecPunch - lastPunch) * -2f * Program.CurrentSettings.NoRecoil.Force;
                if (wep.m_iClip1 != lastClip || lp.m_iShotsFired > 0)
                {
                    lastClip = wep.m_iClip1;
                    Program.Hack.View.ApplyChange(delta);
                    Program.Logger.Log("PunchY: {0}, punchY: {1}, lastPunchY: {2}",
                        System.Math.Round(vecPunch.Y, 2),
                        System.Math.Round(delta.Y, 2),
                        System.Math.Round(lastPunch.Y, 2));
                }
                lastPunch = vecPunch;
            }
            else
            {
                Program.Hack.View.ApplyChange(lp.m_aimPunchAngle.Value * -2f * Program.CurrentSettings.NoRecoil.Force);
            }
        }
        #endregion
    }
}
