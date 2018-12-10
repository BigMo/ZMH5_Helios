using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using ZatsHackBase;
using ZatsHackBase.Drawing;
using _ZMH5__Helios.CSGO.Entities;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class AimModule : HotkeyModule
    {
        #region PROPERTIES
        public int CurrentTarget { get; private set; }
        #endregion

        public AimModule() : base(Program.Hack, ModulePriority.Normal)
        { }

        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            ActiveChanged += (o, e) => Program.Logger.Log("[Aim] State changed: {0}", ActiveByHotkey ? "active" : "inactive");
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            if (!Program.CurrentSettings.Aim.Enabled)
                return;

            Hotkey = Program.CurrentSettings.Aim.Key;
            Mode = Program.CurrentSettings.Aim.Mode;

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
                return;

            if (!ActiveByHotkey)
            {
                CurrentTarget = 0;
                return;
            }
            var src = lp.m_vecOrigin + lp.m_vecViewOffset;// + lp.m_vecVelocity * (float)args.Time.ElapsedTime.TotalSeconds;
            if (Program.CurrentSettings.Aim.Predict)
            {
                var oldLp = Program.Hack.StateMod.PlayersOld[lp.m_iID];
                src += lp.m_vecOrigin - oldLp.m_vecOrigin;
            }

            if (Program.CurrentSettings.Aim.Lock && CurrentTarget != 0)
            {
                var enemy = Program.Hack.StateMod.Players[CurrentTarget];
                if (enemy == null || !enemy.IsValid || enemy.m_lifeState != Enums.LifeState.Alive || enemy.m_bDormant == 1)
                {
                    CurrentTarget = 0;
                    Reset();
                }
            }
            else
            {
                CurrentTarget = GetTarget(src);
            }
            if (CurrentTarget != 0)
            {
                var enemy = Program.Hack.StateMod.Players[CurrentTarget];
                if(enemy == null || !enemy.IsValid)
                {
                    CurrentTarget = 0;
                    return;
                }
                float a = 0.75f + (float)(0.25 * System.Math.Sin(ZatsHackBase.Maths.Math.DegreesToRadians(DateTime.Now.TimeOfDay.TotalSeconds * 1500)));
                
                //Program.Hack.Glow.EncolorObject(Color.FromKnownColor(Color.Orange,
                //    a)
                //    , enemy.m_iGlowIndex);

                var dest = enemy.m_Skeleton.m_Bones[Program.CurrentSettings.Aim.Bone].ToVector();
                if (Program.CurrentSettings.Aim.Predict)
                {
                    var oldEnemy = Program.Hack.StateMod.PlayersOld[CurrentTarget];
                    if (oldEnemy != null && oldEnemy.IsValid)
                    {
                        var oldDest = oldEnemy.m_Skeleton.m_Bones[Program.CurrentSettings.Aim.Bone].ToVector();
                        dest = dest + (dest - oldDest);
                    }
                }

                dest += Vector3.UnitZ * ESPModule.MetersToUnits(Program.CurrentSettings.Aim.HeightOffset / 100f);

                var angle = CalcAngle(src, dest) - Program.Hack.StateMod.ClientState.Value.ViewAngles;
                angle = ViewModule.ClampAngle(angle);
                if (Program.CurrentSettings.Aim.Smoothing.Enabled)
                {
                    switch (Program.CurrentSettings.Aim.Smoothing.Mode)
                    {
                        case Settings.SmoothMode.Scalar:
                            angle *= Program.CurrentSettings.Aim.Smoothing.Scalar;
                            break;
                        case Settings.SmoothMode.MaxDist:
                            if (angle.Length > Program.CurrentSettings.Aim.Smoothing.PerAxis.Length)
                            {
                                angle.Normalize();
                                angle = angle * Program.CurrentSettings.Aim.Smoothing.PerAxis.Length;
                            }
                            break;
                        case Settings.SmoothMode.MaxDistPerAxis:
                            if (angle.X < 0f)
                                angle.X = System.Math.Max(angle.X, -Program.CurrentSettings.Aim.Smoothing.PerAxis.X);
                            else if (angle.X > 0f)
                                angle.X = System.Math.Min(angle.X, Program.CurrentSettings.Aim.Smoothing.PerAxis.X);

                            if (angle.Y < 0f)
                                angle.Y = System.Math.Max(angle.Y, -Program.CurrentSettings.Aim.Smoothing.PerAxis.Y);
                            else if (angle.Y > 0f)
                                angle.Y = System.Math.Min(angle.Y, Program.CurrentSettings.Aim.Smoothing.PerAxis.Y);
                            break;
                        case Settings.SmoothMode.ScalarPerAxis:
                            angle = new Vector3(angle.X * Program.CurrentSettings.Aim.Smoothing.PerAxis.X, angle.Y * Program.CurrentSettings.Aim.Smoothing.PerAxis.Y, 0f);
                            break;
                    }
                }

                Program.Hack.View.ApplyChange(angle);
            }
            if(CurrentTarget != lastId)
            {
                var enemy = Program.Hack.StateMod.Players[CurrentTarget];
                //Program.Logger.Log("[Aim] Aiming at {0}: {1}", enemy != null ? enemy.m_iID : -1, enemy != null ? enemy.m_ClientClass.NetworkName : "null");
                lastId = CurrentTarget;
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
            {
                if (Program.CurrentSettings.Aim.BrokenAim)
                    ret.Y -= 180.0f;
                else
                    ret.Y += 180.0f;
            }
            return ret;
        }
        private int GetTarget(Vector3 src)
        {
            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            var enemies = Program.Hack.StateMod.GetPlayersSet(true, true, true, lp.m_iTeamNum).// lp.m_iTeamNum == Enums.Team.CounterTerrorists ? Enums.Team.Terrorists : Enums.Team.CounterTerrorists).
                OrderBy(x => (x.m_vecOrigin - lp.m_vecOrigin).Length).ToArray();

            var newEnemyId = 0;

            if (Program.CurrentSettings.Aim.Sticky)
            {
                var inc = lp.m_iCrosshairIdx;
                if (inc == 0)
                    return 0;
                var enemy = Program.Hack.StateMod.Players[inc];
                if (enemy == null || !enemy.IsValid || inc == lp.m_iID)// || enemy.m_iTeamNum == lp.m_iTeamNum)
                    return 0;
                newEnemyId = enemy.m_iID;
            }
            else
            {
                Vector3 closest = Vector3.Zero;
                float closestFov = float.MaxValue;
                foreach (var enemy in enemies.OrderBy(x => (lp.m_vecOrigin - x.m_vecOrigin).LengthSqrt))
                {
                    var dest = enemy.m_Skeleton.m_Bones[Program.CurrentSettings.Aim.Bone].ToVector() + Vector3.UnitZ * ESPModule.MetersToUnits(Program.CurrentSettings.Aim.HeightOffset / 100f);
                    if (Program.CurrentSettings.Aim.VisibleOnly)
                    {
                        var map = Program.Hack.StateMod.Map;
                        if (map == null)
                        {
                            if (!enemy.SeenBy(lp))
                                continue;
                        }
                        else if (!map.IsVisible(src, dest))
                            continue;
                    }
                    var newAngles = CalcAngle(src, dest) - Program.Hack.StateMod.ClientState.Value.ViewAngles;
                    newAngles = ViewModule.ClampAngle(newAngles);
                    float fov = newAngles.Length;
                    if (fov < closestFov && fov < Program.CurrentSettings.Aim.FOV)
                    {
                        closestFov = fov;
                        closest = newAngles;
                        newEnemyId = enemy.m_iID;
                    }
                }
            }
            var v = Program.Hack.StateMod.PlayerResources;
            //if (newEnemyId != 0)
            //    Program.Logger.Log("[Aim] Aiming at {0} [{1}]", Program.Hack.StateMod.PlayerResources.Value.m_sNames[newEnemyId], newEnemyId);

            return newEnemyId;
        }
    }
}
