using _ZMH5__Helios.CSGO.Entities;
using _ZMH5__Helios.CSGO.Modules.SnapshotHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Maths;
using ZatsHackBase.UI;
using ZatsHackBase.UI.Drawing;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class ESPModule : HackModule
    {
        #region CONSTANTS
        private static Vector3 MARGINS_Z = new Vector3(0, 0, 10);
        private Font espFont = Font.CreateDummy("Segoe UI", 12);
        #endregion

        #region CONSTRUCTORS
        public ESPModule() : base(Program.Hack, ModulePriority.Normal)
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

            if (!Program.Settings.EspEnabled)
                return;

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
                return;

            var alivePlayers = Program.Hack.StateMod.GetPlayersSet().Where(x=>x.Address != lp.Address);

            var enemies = alivePlayers.Where(x => x.m_iTeamNum.Value != lp.m_iTeamNum.Value).
                OrderBy(x => x.DistanceTo(lp));
            var allies = alivePlayers.Where(x => x.m_iTeamNum.Value == lp.m_iTeamNum.Value).
                OrderBy(x => x.DistanceTo(lp));

            var vEnts = Program.Hack.StateMod.RadarEntries.Value == null ?
                new SnapshotHelpers.RadarEntry[0] :
                Program.Hack.StateMod.RadarEntries.Value.Where(x => !string.IsNullOrEmpty(x.Name));

            if (Program.Settings.EspShowAllies)
                DrawPlayerSet(lp, allies, Program.Settings.EspAlliesColor, vEnts);
            if (Program.Settings.EspShowAllies)
                DrawPlayerSet(lp, enemies, Program.Settings.EspEnemiesColor, vEnts);
        }

        private void DrawPlayerSet(CSLocalPlayer lp, IEnumerable<CSPlayer> players, Color espColor, IEnumerable<RadarEntry> vEnts)
        {
            foreach (var enemy in players)
            {
                var ptDown3d = enemy.m_vecOrigin.Value - MARGINS_Z;
                var ptUp3d = enemy.m_Skeleton.Value.m_Bones[6].ToVector() + MARGINS_Z;
                Vector2 ptDown = Vector2.Zero, ptUp = Vector2.Zero;

                if (!w2s(ref ptDown, ptDown3d) || !w2s(ref ptUp, ptUp3d))
                    continue;

                float dist = lp.DistanceTo(enemy);
                Vector2 size = new Vector2((ptDown.Y - ptUp.Y) * 0.5f, ptDown.Y - ptUp.Y); //TODO: Präziser berechnen, am besten über Bones
                Vector2 upperLeft = new Vector2((ptUp.X + ptDown.X) / 2f - size.X * 0.5f, ptUp.Y);

                if (Program.Settings.EspPlayerShowBox)
                    DrawOutlinedRect(upperLeft, size, espColor, Color.Black);

                Vector2 barSize = new Vector2(size.X, size.Y / 20f);
                Vector2 barHP = new Vector2(upperLeft.X, upperLeft.Y + size.Y + barSize.Y * 0.5f);
                Vector2 barArmor = new Vector2(upperLeft.X, upperLeft.Y + size.Y + barSize.Y + barSize.Y);

                if (Program.Settings.EspPlayerShowLifeArmor)
                {
                    DrawHBar(barHP, barSize, enemy.m_iHealth / 100f, espColor, Color.Transparent, Color.Green, Color.Black);
                    DrawHBar(barArmor, barSize, enemy.m_ArmorValue / 100f, espColor, Color.Transparent, Color.White, Color.Black);
                }

                if (Program.Settings.EspPlayerShowName || Program.Settings.EspPlayerShowWeapon)
                {
                    if (vEnts.Any(x => x.Id == enemy.m_iID.Value))
                    {
                        var ent = vEnts.First(x => x.Id == enemy.m_iID.Value);

                        espFont = Program.Hack.Overlay.Renderer.Fonts[espFont];
                        var textPos = upperLeft + Vector2.UnitX * size.X;
                        var wep = enemy.m_ActiveWeapon.Value;
                        string text = "";
                        
                        if (Program.Settings.EspPlayerShowName)
                            text += ent.Name;
                        if (Program.Settings.EspPlayerShowWeapon && wep != null && wep.IsValid)
                            text += (text.Length != 0 ? "\n" : "") + wep.m_ClientClass.Value.NetworkName.Value.Replace("CWeapon", "");

                        if (text.Length > 0)
                            Program.Hack.Overlay.Renderer.DrawString(espColor, espFont, textPos, text);
                    }
                }
            }
        }

        private void DrawOutlinedRect(Vector2 upperLeft, Vector2 size, Color border, Color outline)
        {
            Program.Hack.Overlay.Renderer.DrawRectangle(
                    outline,
                    upperLeft - Vector2.Unit,
                    size + Vector2.Unit * 2f);
            Program.Hack.Overlay.Renderer.DrawRectangle(
                outline,
                upperLeft + Vector2.Unit,
                size - Vector2.Unit * 2f);
            Program.Hack.Overlay.Renderer.DrawRectangle(
                border,
                upperLeft,
                size);
        }
        private void DrawVBar(Vector2 upperLeft, Vector2 size, float fillPerc, Color border, Color bg, Color fill, Color outline)
        {
            Program.Hack.Overlay.Renderer.FillRectangle(bg, upperLeft, size);

            float percHeight = System.Math.Min(1f, System.Math.Max(0f, fillPerc));
            DrawOutlinedRect(
                new Vector2(upperLeft.X, upperLeft.Y + size.Y - size.Y * percHeight),
                new Vector2(size.X, size.Y * percHeight),
                fill, outline);
            Program.Hack.Overlay.Renderer.FillRectangle(fill,
                new Vector2(upperLeft.X, upperLeft.Y + size.Y - size.Y * percHeight),
                new Vector2(size.X, size.Y * percHeight));

            Program.Hack.Overlay.Renderer.DrawRectangle(border, upperLeft, size);
            //DrawOutlinedRect(upperLeft, size, border, outline);
        }
        private void DrawHBar(Vector2 upperLeft, Vector2 size, float fillPerc, Color border, Color bg, Color fill, Color outline)
        {
            Program.Hack.Overlay.Renderer.FillRectangle(bg, upperLeft, size);

            float percWidth = System.Math.Min(1f, System.Math.Max(0f, fillPerc));
            DrawOutlinedRect(
                new Vector2(upperLeft.X, upperLeft.Y),
                new Vector2(size.X * percWidth, size.Y),
                fill, outline);
            Program.Hack.Overlay.Renderer.FillRectangle(fill,
                new Vector2(upperLeft.X, upperLeft.Y),
                new Vector2(size.X * percWidth, size.Y));

            Program.Hack.Overlay.Renderer.DrawRectangle(border, upperLeft, size);
            //DrawOutlinedRect(upperLeft, size, border, outline);
        }
        #endregion
    }
}
