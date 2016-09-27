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

        public NoRecoilModule() : base(ModulePriority.Normal)
        {
        }

        #region METHODS
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            return;
            //TODO: Fix...
            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
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
