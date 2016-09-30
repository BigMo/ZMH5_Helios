using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Core;
using ZatsHackBase;
using System.Threading;
using _ZMH5__Helios.CSGO.Entities;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class TriggerModule : HotkeyModule
    {
        #region VARIABLES
        private int shotsLeft = 0;
        private int lastShots = 0;
        private int lastEnemyId = 0;
        private bool lostOnce;
        private DateTime lastEnemyDetection;
        #endregion

        #region CONSTRUCTORS
        public TriggerModule() : base(ModulePriority.Normal)
        {
            lastEnemyDetection = DateTime.Now;
        }
        #endregion

        #region METHODS
        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            ActiveChanged += (o, e) => Program.Logger.Log("[Trigger] State changed: {0}", ActiveByHotkey ? "active" : "inactive");
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            Hotkey = Program.Settings.TriggerKey;
            Mode = Program.Settings.TriggerMode;

            //Aquire localplayer
            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
            {
                //Program.Logger.Log("TRG: No LocalPlayer");
                return;
            }
            //Aquire active weapon
            var wep = lp.m_ActiveWeapon;
            if (wep.Value == null || !wep.Value.IsValid)
            {
                //Program.Logger.Log("TRG: No ActiveWeapon");
                return;
            }

            //Take care of any burst-shots before doing anything else
            if (shotsLeft > 0)
            {
                if (wep.Value.m_iClip1.Value > 0)
                {
                    if ((wep.Value.m_flNextPrimaryAttack - lp.m_flSimulationTime) <= 0f)
                    {
                        //Program.Logger.Log("TRG: Shooting burst");
                        Shoot();
                        shotsLeft--;
                    }
                } else
                {
                    shotsLeft = 0;
                }
                lastShots = lp.m_iShotsFired;
                lastEnemyId = 0;
                return;
            }

            if (!ActiveByHotkey)
                return;

            //Empty crosshair? -> Reset id
            if (lp.m_iCrosshairIdx == 0)
            {
                if (lostOnce)
                {
                    //if (lastEnemyId != localPlayer.m_iCrosshairIdx)
                    //    Program.Logger.Log("TRG: No CrosshairIdx");
                    lastEnemyId = 0;
                }
                lostOnce = true;
            }
            //New enemy? Check entity and set variables
            else if (lp.m_iCrosshairIdx != lastEnemyId)
            {
                var enemy = Program.Hack.StateMod.Players[lp.m_iCrosshairIdx.Value];
                if (enemy == null || !enemy.IsValid || enemy.m_iTeamNum.Value == lp.m_iTeamNum.Value)
                {
                    //Program.Logger.Log("TRG: No (valid) enemy");
                    lastEnemyId = 0;
                    lostOnce = true;
                }
                else
                {
                    //Program.Logger.Log("TRG: Enemy set to {0}", localPlayer.m_iCrosshairIdx.Value);
                    lastEnemyDetection = DateTime.Now;
                    lastEnemyId = lp.m_iCrosshairIdx;
                    lostOnce = false;
                }
            }
            else //Still the same enemy -> Check delay and fire
            {
                lostOnce = false;
                if ((DateTime.Now - lastEnemyDetection).TotalMilliseconds < Program.Settings.TriggerDelay)
                    return;

                var enemy = Program.Hack.StateMod.Players[lp.m_iCrosshairIdx.Value];
                if (enemy == null || !enemy.IsValid || enemy.m_iTeamNum.Value == lp.m_iTeamNum.Value)
                    return;

                //Program.Logger.Log("TRG: ALRIGHT");
                if (wep.Value.m_iClip1.Value > 0)// && wep.Value.m_fAccuracyPenalty < 0.01f)
                {
                    if (Program.Settings.TriggerBurst)
                    {
                        shotsLeft += Program.Settings.TriggerBurstCount;
                    }
                    else
                    {
                        Shoot();
                    }
                    lastEnemyId = 0;
                }
            }
        }

        private void Shoot()
        {
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(1);
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF.LEFTUP, 0, 0, 0, 0);
        }
        #endregion
    }
}
