using _ZMH5__Helios.CSGO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class ESPModule : HackModule
    {
        #region CONSTANTS
        private static Vector3 MARGINS_Z = new Vector3(0, 0, 10);
        #endregion

        #region CONSTRUCTORS
        public ESPModule() : base(ModulePriority.Normal)
        { }
        #endregion

        #region METHODS
        private bool w2s(ref Vector2 screenPos, Vector3 worldPos)
        {
            screenPos = ZatsHackBase.Maths.Math.WorldToScreen(Program.Hack.StateMod.ViewMatrix, Program.Hack.Overlay.Size, worldPos);
            return screenPos != Vector2.Zero;
        }

        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);


            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
                return;

            var alivePlayers = Program.Hack.StateMod.GetAllPlayers().
                Where(x => x != null && x.IsValid).
                Where(x => x.m_lifeState.Value == Enums.LifeState.Alive).
                Where(x => x.m_Skeleton.Value != null);

            var enemies = alivePlayers.Where(x => x.m_iTeamNum.Value != lp.m_iTeamNum.Value);
            var allies = alivePlayers.Where(x => x.m_iTeamNum.Value == lp.m_iTeamNum.Value);

            foreach (var enemy in enemies)
            {
                var ptDown3d = enemy.m_vecOrigin.Value - MARGINS_Z;
                var ptUp3d = enemy.m_Skeleton.Value[6].ToVector() + MARGINS_Z;
                Vector2 ptDown = Vector2.Zero, ptUp = Vector2.Zero;

                if (!w2s(ref ptDown, ptDown3d) || !w2s(ref ptUp, ptUp3d))
                    continue;

                float dist = lp.DistanceTo(enemy);
                Vector2 size = new Vector2((ptUp.Y - ptDown.Y) * 0.5f, ptDown.Y - ptUp.Y); //TODO: Präziser berechnen, am besten über Bones
                Vector2 upperLeft = new Vector2((ptUp.X + ptDown.X) / 2f - size.X * 0.5f, ptUp.Y);
                Program.Hack.Overlay.Renderer.DrawRectangle(
                    Color.Red, 
                    upperLeft,
                    size);

                //Vector2 hpBarTop = new Vector2(upperLeft.X + size.X + 2f, upperLeft.Y);
                //Vector2 hpBarFillTop = new Vector2(upperLeft.X + size.X + 2f, upperLeft.Y);

            }
        }

        private void DrawBar(Vector2 upperLeft, Vector2 size, float fillPerc, Color border, Color bg, Color fill)
        {
            Program.Hack.Overlay.Renderer.FillRectangle(bg, upperLeft, size);
            Program.Hack.Overlay.Renderer.DrawRectangle(border, upperLeft, size);

            float percHeight = size.Y * System.Math.Min(1f, System.Math.Max(0f, fillPerc));
            //Program.Hack.Overlay.Renderer.FillRectangle(fill, )
        }
        #endregion
    }
}
