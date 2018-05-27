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
    public class ViewModule : HackModule
    {
        #region VARIABLES
        private Vector3 delta;
        #endregion

        public ViewModule() : base(Program.Hack, ModulePriority.Lowest)
        { }

        #region METHODS
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            if (delta != Vector3.Zero)
            {
                var final = ClampAngle(Program.Hack.StateMod.ClientState.Value.ViewAngles + delta);
                Program.Hack.StateMod.WriteViewAngles(final);
                delta = Vector3.Zero;
            }
        }

        public void ApplyChange(Vector3 change)
        {
            delta += change;
        }

        public static Vector3 ClampAngle(Vector3 qaAng)
        {
            if (qaAng[0] > 89.0f)
                qaAng[0] = 89.0f;

            if (qaAng[0] < -89.0f)
                qaAng[0] = -89.0f;

            while (qaAng[1] > 180)
                qaAng[1] -= 360;

            while (qaAng[1] < -180)
                qaAng[1] += 360;

            qaAng.Z = 0;

            return qaAng;
        }
        #endregion
    }
}
