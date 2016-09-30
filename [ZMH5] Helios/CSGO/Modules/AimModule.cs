using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using ZatsHackBase;
using ZatsHackBase.UI.Drawing;
using _ZMH5__Helios.CSGO.Entities;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class AimModule : HotkeyModule
    {
        #region VARIABLES
        private int currentId;
        #endregion

        public AimModule() : base(ModulePriority.Normal)
        { }

        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            Hotkey = Program.Settings.AimKey;
            Mode = Program.Settings.AimMode;
            ActiveChanged += (o, e) => Program.Logger.Log("[Aim] State changed: {0}", ActiveByHotkey ? "active" : "inactive");
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
                return;

            if (!ActiveByHotkey)
            {
                currentId = 0;
                return;
            }

            var src = lp.m_vecOrigin.Value + lp.m_vecViewOffset.Value + lp.m_vecVelocity.Value * (float)args.Time.ElapsedTime.TotalSeconds;

            if (Program.Settings.AimLock && currentId != 0)
            {
                var enemy = Program.Hack.StateMod.Players[currentId];
                if (enemy == null || !enemy.IsValid || enemy.m_lifeState.Value != Enums.LifeState.Alive || enemy.m_bDormant == 1)
                {
                    currentId = 0;
                    Reset();
                }
            }
            else
            {
                currentId = GetTarget(src);
            }
            if (currentId != 0)
            {
                var enemy = Program.Hack.StateMod.Players[currentId];
                if(enemy == null || !enemy.IsValid)
                {
                    currentId = 0;
                    return;
                }
                Program.Hack.Glow.EncolorObject(Color.Blue, enemy.m_iGlowIndex);

                var dest = enemy.m_Skeleton.Value.m_Bones[Program.Settings.AimBone].ToVector();
                //if Aimbot.Predict
                var oldEnemy = Program.Hack.StateMod.PlayersOld[currentId];
                if (oldEnemy != null && oldEnemy.IsValid)
                {
                    var oldDest = oldEnemy.m_Skeleton.Value.m_Bones[Program.Settings.AimBone].ToVector();
                    dest = dest + (dest - oldDest);
                }

                var angle = CalcAngle(src, dest) - Program.Hack.StateMod.ViewAngles.Value;
                if (Program.Settings.AimSmoothEnabled)
                {
                    switch (Program.Settings.AimSmoothMode)
                    {
                        case Settings.SmoothMode.Scalar:
                            Console.WriteLine("{0} * {1} = {2}", angle, Program.Settings.AimSmoothScalar, angle * Program.Settings.AimSmoothScalar);
                            angle *= Program.Settings.AimSmoothScalar;
                            break;
                        case Settings.SmoothMode.MaxDist:
                            if (angle.Length > Program.Settings.AimSmoothMaxDist.Length)
                            {
                                angle.Normalize();
                                angle *= Program.Settings.AimSmoothMaxDist.Length;
                            }
                            break;
                        case Settings.SmoothMode.MaxDistPerAxis:
                            var vx = new Vector2(Program.Settings.AimSmoothMaxDist.X, 0f);
                            var vy = new Vector2(0f, Program.Settings.AimSmoothMaxDist.X);
                            if (vx.Length > Program.Settings.AimSmoothMaxDist.Length)
                            {
                                vx.Normalize();
                                vx *= Program.Settings.AimSmoothMaxDist.Length;
                            }
                            if (vy.Length > Program.Settings.AimSmoothMaxDist.Length)
                            {
                                vy.Normalize();
                                vy *= Program.Settings.AimSmoothMaxDist.Length;
                            }
                            angle = new Vector3(vx.X, vy.Y, 0f);
                            break;
                    }
                }

                Program.Hack.View.ApplyChange(angle);
            }
            if(currentId != lastId)
            {
                var enemy = Program.Hack.StateMod.Players[currentId];
                Program.Logger.Log("[Aim] Aiming at {0}: {1}", enemy != null ? enemy.m_iID.Value : -1, enemy != null ? enemy.m_ClientClass.Value.NetworkName : "null");
                lastId = currentId;
            }
        }

        private int lastId = 0;

        private static Vector3 CalcAngle(Vector3 src, Vector3 dst)
        {
            Vector3 ret = new Vector3();
            Vector3 vDelta = src - dst;
            float fHyp = (float)System.Math.Sqrt((vDelta.X * vDelta.X) + (vDelta.Y * vDelta.Y));

            ret.X = ZatsHackBase.Maths.Math.RadiansToDegrees((float)System.Math.Atan(vDelta.Z / fHyp));
            ret.Y = ZatsHackBase.Maths.Math.RadiansToDegrees((float)System.Math.Atan(vDelta.Y / vDelta.X));

            if (vDelta.X >= 0.0f)
                ret.Y += 180.0f;
            return ret;
        }
        private int GetTarget(Vector3 src)
        {
            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            var enemies = Program.Hack.StateMod.GetAllPlayers().
                Where(x => x != null && x.IsValid).
                Where(x => x.m_iTeamNum.Value != lp.m_iTeamNum.Value).
                Where(x => x.m_lifeState.Value == Enums.LifeState.Alive).
                OrderBy(x => (x.m_vecOrigin.Value - lp.m_vecOrigin.Value).Length).ToArray();           

            Vector3 closest = Vector3.Zero;
            float closestFov = float.MaxValue;
            foreach (var enemy in enemies)
            {
                var newAngles = CalcAngle(src, enemy.m_Skeleton.Value.m_Bones[Program.Settings.AimBone].ToVector()) - Program.Hack.StateMod.ViewAngles.Value;
                newAngles = ViewModule.ClampAngle(newAngles);
                float fov = newAngles.Length;
                if (fov < closestFov && fov < Program.Settings.AimFov)
                {
                    Program.Logger.Log("[Aim] Aiming at {0} at {1}° delta", enemy.m_iID.Value, System.Math.Round(fov, 2));
                    closestFov = fov;
                    closest = newAngles;
                    return enemy.m_iID.Value;
                }
            }

            return 0;
        }
    }
}
