using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using _ZMH5__Helios.CSGO.Entities;
using ZatsHackBase.Drawing;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class NoRecoilModule : HackModule
    {
        #region VARIABLES
        private int lastClip = 0;
        private Vector3 lastPunch = Vector3.Zero;
        private Vector2 crSize = new Vector2(10, 1);
        private Vector2 crSize2 = new Vector2(1, 10);
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
            if (Program.CurrentSettings.NoRecoil.NoRecoilSA)
                if (wep == null || !wep.IsValid)
                    return;
                else
                if (wep == null || !wep.IsValid || wep.IsPistol || wep.IsPumpgun || wep.IsSniper)
                {
                    DrawCrosshair();
                    return;
                }


            if (Program.CurrentSettings.NoRecoil.SCrosshair)
                DrawCrosshair();

            if (Program.Hack.AimBot.CurrentTarget == 0)
            {
                var vecPunch = lp.m_aimPunchAngle;
                var delta = (vecPunch - lastPunch) * -2f * Program.CurrentSettings.NoRecoil.Force;
                if (wep.m_iClip1 != lastClip || lp.m_iShotsFired > 0)
                {
                    lastClip = wep.m_iClip1;
                    Program.Hack.View.ApplyChange(delta);
                    //Program.Logger.Log("PunchY: {0}, punchY: {1}, lastPunchY: {2}",
                    //    System.Math.Round(vecPunch.Y, 2),
                    //    System.Math.Round(delta.Y, 2),
                    //    System.Math.Round(lastPunch.Y, 2));
                }
                lastPunch = vecPunch;
            }
            else
            {
                Program.Hack.View.ApplyChange(lp.m_aimPunchAngle * -2f * Program.CurrentSettings.NoRecoil.Force);
            }
        }

        private void DrawCrosshair()
        {
            var lp = Program.Hack.StateMod.LocalPlayer.Value;

            float Height = Program.Hack.Overlay.Form.Size.Height, Width = Program.Hack.Overlay.Form.Size.Width;

            float x = Width / 2f;
            float y = Height / 2f;
            float dy = Height / 90f;
            float dx = Width / 90f;

            x -= (dx * (lp.m_aimPunchAngle.Y));
            y += (dy * (lp.m_aimPunchAngle.X));

            Vector2 crLoc = new Vector2(x - 5, y);
            Vector2 crLoc2 = new Vector2(x, y - 5);

            Program.Hack.Overlay.Visuals.FillRectangle(Color.Green, crLoc, crSize);
            Program.Hack.Overlay.Visuals.FillRectangle(Color.Green, crLoc2, crSize2);
        }

        #endregion
    }
}
