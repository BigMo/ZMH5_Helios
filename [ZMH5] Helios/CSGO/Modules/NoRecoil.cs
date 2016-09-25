using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class NoRecoil : HackModule
    {
        #region VARIABLES
        private int lastClip = 0;
        private Vector3 lastPunch = Vector3.Zero;
        #endregion

        public NoRecoil() : base(ModulePriority.Normal)
        {
        }

        #region METHODS
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            return;

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (lp == null || !lp.IsValid)
                return;

            var wep = lp.m_ActiveWeapon.Value;
            if (wep == null || !wep.IsValid)
                return;

            var vecPunch = lp.m_viewPunchAngle.Value;
            var punch = new Vector3(vecPunch.X - lastPunch.X, vecPunch.Y - lastPunch.Y, 0);
            if (wep.m_iClip1 != lastClip && lp.m_iShotsFired > 0)
            {
                lastClip = wep.m_iClip1;
                Program.Hack.View.ApplyChange(vecPunch * -0.1f);
                Program.Logger.Log("PunchY: {0}, punchY: {1}, lastPunchY: {2}", 
                    System.Math.Round(vecPunch.Y, 2),
                    System.Math.Round(punch.Y, 2),
                    System.Math.Round(lastPunch.Y, 2));
            }
            lastPunch = vecPunch;
        }
        #endregion
    }
}
