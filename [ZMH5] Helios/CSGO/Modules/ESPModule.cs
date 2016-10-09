using _ZMH5__Helios.CSGO.Entities;
using _ZMH5__Helios.CSGO.Misc;
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
        private Font espFont = Font.CreateDummy("Segoe UI", 14);
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

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (!CSLocalPlayer.IsProcessable(lp))
                return;

            var alivePlayers = Program.Hack.StateMod.GetPlayersSet().Where(x => x.Address != lp.Address);

            var enemies = alivePlayers.Where(x => x.m_iTeamNum.Value != lp.m_iTeamNum.Value).
                OrderBy(x => x.DistanceTo(lp));
            var allies = alivePlayers.Where(x => x.m_iTeamNum.Value == lp.m_iTeamNum.Value).
                OrderBy(x => x.DistanceTo(lp));

            var vEnts = Program.Hack.StateMod.RadarEntries.Value == null ?
                new SnapshotHelpers.RadarEntry[0] :
                Program.Hack.StateMod.RadarEntries.Value.Where(x => !string.IsNullOrEmpty(x.Name));

            var weapons = Program.Hack.StateMod.Weapons.Entites.Where(x => x.m_hOwner.Value <= 0).OrderByDescending(x=>x.DistanceTo(lp));
            //TODO: Granaten und C4 (inkl. CPlantedC4) gesondert rendern
            if (Program.Settings.EspWeapons.Enabled)
                DrawWeaponSet(lp, weapons, Program.Settings.EspWeapons);

            var ents = Program.Hack.StateMod.BaseEntitites.Entites.Where(x => x != null && x.IsValid);
            if (Program.Settings.EspChickens.Enabled)
                DrawBaseEntitySet(lp, ents.Where(x => x.m_ClientClass.Value.NetworkName == "CChicken"), Program.Settings.EspChickens);

            if (Program.Settings.EspAllies.Enabled)
                DrawPlayerSet(lp, allies, Program.Settings.EspAllies, vEnts);
            if (Program.Settings.EspEnemies.Enabled)
                DrawPlayerSet(lp, enemies, Program.Settings.EspEnemies, vEnts);
        }

        private static float DistToMeters(float dist)
        {
            return dist / 12f * 0.3048f / 1000;
        }
        private void DrawEntityEsp(Vector2 upperLeft, Vector2 size, ESPSettings settings, string text, float armor = 0f, float life = 0f)
        {
            if (settings.ShowBox)
                DrawOutlinedRect(upperLeft, size, settings.Color, Color.Black);

            if (settings.ShowLifeArmor)
            {
                Vector2 barSize = new Vector2(size.X, size.Y / 20f);
                Vector2 barHP = new Vector2(upperLeft.X, upperLeft.Y + size.Y + barSize.Y * 0.5f);
                Vector2 barArmor = new Vector2(upperLeft.X, upperLeft.Y + size.Y + barSize.Y + barSize.Y);
                DrawHBar(barHP, barSize, life / 100f, settings.Color, Color.Transparent, Color.Green, Color.Black);
                DrawHBar(barArmor, barSize, armor / 100f, settings.Color, Color.Transparent, Color.White, Color.Black);
            }

            if (!string.IsNullOrEmpty(text))
            {
                espFont = Program.Hack.Overlay.Renderer.Fonts[espFont];
                var textPos = upperLeft + Vector2.UnitX * size.X;
                Program.Hack.Overlay.Renderer.DrawString(settings.Color, espFont, textPos, text);
            }
        }

        private void DrawEsp<T>(
            T ent,
            ESPSettings settings,
            Func<T, Vector3> fnGetBottom,
            Func<T, Vector3> fnGetTop,
            Func<Vector2, Vector2, Vector2> fnGetSize,
            Func<T, float> fnGetLife,
            Func<T, float> fnGetArmor,
            Func<T, ESPSettings, string> fnGetText) where T : BaseEntity
        {
            var ptDown3d = fnGetBottom(ent) - MARGINS_Z;
            var ptUp3d = fnGetTop(ent) + MARGINS_Z;
            Vector2 ptDown = Vector2.Zero, ptUp = Vector2.Zero;

            if (!w2s(ref ptDown, ptDown3d) || !w2s(ref ptUp, ptUp3d))
                return;

            var size = fnGetSize(ptDown, ptUp);
            var upperLeft = new Vector2((ptUp.X + ptDown.X) / 2f - size.X * 0.5f, ptUp.Y);
            var text = fnGetText(ent, settings);

            if (settings.ShowBox)
                DrawOutlinedRect(upperLeft, size, settings.Color, Color.Black);

            if (settings.ShowLifeArmor && fnGetLife != null && fnGetArmor != null)
            {
                Vector2 barSize = new Vector2(size.X, size.Y / 20f);
                Vector2 barPos = new Vector2(upperLeft.X, upperLeft.Y + size.Y + barSize.Y * 0.5f);
                if (fnGetLife != null)
                {
                    DrawHBar(barPos, barSize, fnGetLife(ent) / 100f, settings.Color, Color.Transparent, Color.Green, Color.Black);
                    barPos.Y += barSize.Y * 1.5f;
                }
                if (fnGetArmor != null && fnGetArmor(ent) > 0f)
                    DrawHBar(barPos, barSize, fnGetArmor(ent) / 100f, settings.Color, Color.Transparent, Color.White, Color.Black);
            }

            if (!string.IsNullOrEmpty(text))
            {
                espFont = Program.Hack.Overlay.Renderer.Fonts[espFont];
                var textPos = upperLeft + Vector2.UnitX * size.X;
                Program.Hack.Overlay.Renderer.DrawString(settings.Color, espFont, textPos, text);
            }
        }

        private void DrawEspSet<T>(
            IEnumerable<T> ents,
            ESPSettings settings,
            Func<T, Vector3> fnGetBottom,
            Func<T, Vector3> fnGetTop,
            Func<Vector2, Vector2, Vector2> fnGetSize,
            Func<T, float> fnGetLife,
            Func<T, float> fnGetArmor,
            Func<T, ESPSettings, string> fnGetText) where T : BaseEntity
        {
            foreach (var e in ents)
                DrawEsp<T>(e, settings, fnGetBottom, fnGetTop, fnGetSize, fnGetLife, fnGetArmor, fnGetText);
        }

        private void DrawPlayerSet(CSLocalPlayer lp, IEnumerable<CSPlayer> players, ESPSettings settings, IEnumerable<RadarEntry> vEnts)
        {
            DrawEspSet<CSPlayer>(
                players,
                settings,
                (p) => p.m_vecOrigin,
                (p) => p.m_Skeleton.Value.m_Bones[6].ToVector(),
                (d, u) => new Vector2((d.Y - u.Y) * 0.5f, d.Y - u.Y),
                (p) => p.m_iHealth,
                (p) => p.m_ArmorValue,
                (p, s) =>
                {
                    string text = "";
                    if (settings.ShowName)
                        if (vEnts.Any(x => x.Id == p.m_iID.Value))
                            text += vEnts.First(x => x.Id == p.m_iID.Value).Name + "\n";

                    if (
                        (lp.m_iTeamNum.Value == p.m_iTeamNum.Value && Program.Settings.EspAlliesShowWeapon)
                        || (lp.m_iTeamNum.Value != p.m_iTeamNum.Value && Program.Settings.EspEnemiesShowWeapon)
                        )
                        if (p.m_ActiveWeapon.Value != null && p.m_ActiveWeapon.Value.IsValid)
                        {
                            var wep = p.m_ActiveWeapon.Value;
                            text += string.Format("{0} [{1}]",
                                wep.m_ClientClass.Value.NetworkName.Value.Replace("CWeapon", ""),
                                wep.m_iClip1.Value >= 0 ? wep.m_iClip1.ToString() : "/")
                                + "\n";
                        }
                        else
                            text += "[unarmed]\n";

                    if (settings.ShowDist)
                        text += string.Format("[{0}m]", System.Math.Ceiling(DistToMeters(lp.DistanceTo(p)))) + "\n";

                    return text;
                }
            );
        }
        private void DrawWeaponSet(CSLocalPlayer lp, IEnumerable<BaseCombatWeapon> weapons, ESPSettings settings)
        {
            DrawEspSet<BaseCombatWeapon>(
                weapons,
                settings,
                (w) => w.m_vecOrigin,
                (w) => w.m_vecOrigin,
                (d, u) =>
                {
                    var f = d - u;
                    return new Vector2(System.Math.Max(f.X, f.Y));
                },
                null,
                null,
                (w, s) =>
                {
                    string text = "";
                    if (settings.ShowName)
                        text += string.Format("{0} [{1}]", 
                            w.m_ClientClass.Value.NetworkName.Value.Replace("CWeapon", ""),
                            w.m_iClip1.Value >= 0 ? w.m_iClip1.Value.ToString() : "/") + "\n";

                    if (settings.ShowDist)
                        text += string.Format("[{0}m]", System.Math.Ceiling(DistToMeters(lp.DistanceTo(w)))) + "\n";

                    return text;
                }
            );
        }
        private void DrawBaseEntitySet(CSLocalPlayer lp, IEnumerable<BaseEntity> ents, ESPSettings settings)
        {
            DrawEspSet<BaseEntity>(
                ents,
                settings,
                (w) => w.m_vecOrigin,
                (w) => w.m_vecOrigin,
                (d, u) => u - d,
                null,
                null,
                (w, s) =>
                {
                    string text = "";
                    if (settings.ShowName)
                        text += w.m_ClientClass.Value.NetworkName.Value + "\n";

                    if (settings.ShowDist)
                        text += string.Format("[{0}m]", System.Math.Ceiling(DistToMeters(lp.DistanceTo(w)))) + "\n";

                    return text;
                }
            );
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
