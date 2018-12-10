using _ZMH5__Helios.CSGO.BSP;
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
using ZatsHackBase.Drawing;
using ZatsHackBase;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class ESPModule : HackModule
    {
        #region CONSTANTS
        private static Vector3 MARGINS_Z = new Vector3(0, 0, 10);
        private Font espFont = Font.CreateDummy("Segoe UI", 14, true);
        private Font weaponFont = Font.CreateDummy("csgo_icons", 40, true, false, false, (char)0xE001, (char)0xE203);
        private Font[] weaponFonts = new Font[4];
        #endregion

        #region CONSTRUCTORS
        public ESPModule() : base(Program.Hack, ModulePriority.Normal)
        {
            var min = 1f;
            var max = 40f;
            var stepSize = (max - min) / weaponFonts.Length;
            for (int i = 0; i < weaponFonts.Length; i++)
            {
                weaponFonts[i] = Font.CreateDummy("csgo_icons", max - stepSize * i, true, false, false, (char)0xE001, (char)0xE203);
            }
        }
        #endregion

        #region METHODS
        private bool w2s(ref Vector2 screenPos, Vector3 worldPos)
        {
            screenPos = ZatsHackBase.Maths.Math.WorldToScreen(Program.Hack.StateMod.ViewMatrix, Program.Hack.Overlay.Size, worldPos);
            return screenPos != Vector2.Zero && 
                (screenPos.X < Program.Hack.Overlay.Size.X && screenPos.Y < Program.Hack.Overlay.Size.Y) &&
                (screenPos.X >= 0 && screenPos.Y >= 0)
                ;
        }

        private bool IsGrenade(BaseEntity x)
        {
            return x.m_ClientClass.NetworkName.Value.Contains("Projectile") || x.m_ClientClass.NetworkName.Value.Contains("Grenade");
        }

        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            if(Program.CurrentSettings.ESP.World.Enabled)
                DrawWorld();

            //DrawModels();

            espFont = Program.Hack.Overlay.Renderer.Fonts[espFont];
            weaponFont = Program.Hack.Overlay.Renderer.Fonts[weaponFont];
            for (int i = 0; i < weaponFonts.Length; i++)
                weaponFonts[i] = Program.Hack.Overlay.Renderer.Fonts[weaponFonts[i]];

            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (lp == null || !lp.IsValid)
                return;

            var alivePlayers = Program.Hack.StateMod.GetPlayersSet(true, true, true).Where(x => x.Address != lp.Address);

            var enemies = alivePlayers.Where(x => x.m_iTeamNum != lp.m_iTeamNum).
                OrderBy(x => x.DistanceTo(lp));
            var allies = alivePlayers.Where(x => x.m_iTeamNum == lp.m_iTeamNum).
                OrderBy(x => x.DistanceTo(lp));

            var grenades = Program.Hack.StateMod.BaseEntitites.Entites.Where(
                x => IsGrenade(x) && Program.Hack.StateMod.Weapons[x.m_iID] != null && Program.Hack.StateMod.Weapons[x.m_iID].m_hOwner <= 0
            );

            var weapons = Program.Hack.StateMod.Weapons.Entites.Where(x => x.m_hOwner <= 0).OrderByDescending(x => x.DistanceTo(lp));
            DrawBaseEntitySet(lp, grenades, new ESPEntry() { Enabled = true, ShowBox = true, ShowName = true, Color = new Color(1, 0, 0) });
            //TODO: Granaten und C4 (inkl. CPlantedC4) gesondert rendern
            if (Program.CurrentSettings.ESP.Weapons.Enabled)// && WinAPI.GetAsyncKeyState(System.Windows.Forms.Keys.LShiftKey) != 0)// && Program.Hack.Input.KeysDown.Contains(System.Windows.Forms.Keys.LControlKey))
                DrawWeaponSet(lp, weapons, Program.CurrentSettings.ESP.Weapons);

            var ents = Program.Hack.StateMod.BaseEntitites.Entites.Where(x => x != null && x.IsValid);
            if (Program.CurrentSettings.ESP.Chickens.Enabled)
                DrawBaseEntitySet(lp, ents.Where(x => x.m_ClientClass.NetworkName == "CChicken"), Program.CurrentSettings.ESP.Chickens);

            if (Program.CurrentSettings.ESP.Allies.Enabled)
                DrawPlayers(allies, Program.CurrentSettings.ESP.Allies);
            if (Program.CurrentSettings.ESP.Enemies.Enabled)
                DrawPlayers(enemies, Program.CurrentSettings.ESP.Enemies);

            if (Program.CurrentSettings.DebugShowBones)
                DrawBones();
        }

        private void DrawWorld()
        {
            if (Program.Hack.StateMod.Map != null)
            {
                var map = Program.Hack.StateMod.Map;
                var lp = Program.Hack.StateMod.LocalPlayer.Value;
                if (lp == null || !lp.IsValid)
                    return;

                var alivePlayers = Program.Hack.StateMod.GetPlayersSet(true, true, true).Where(x => x.Address != lp.Address);

                var allPositions = alivePlayers.Select(x => x.m_vecOrigin).ToArray(); //.Concat(new Vector3[] { lp.m_vecOrigin }).ToArray();

                mvertex_t line3d1, line3d2;
                foreach (var face in map.m_Surfaces)
                {
                    if (map.m_Texinfos.Length > face.m_Texinfo)
                    {
                        if (((map.m_Texinfos[face.m_Texinfo].m_Flags & BSPFlags.SURF_NODRAW) != 0))
                            continue;
                    }

                    var plane = map.m_Planes[face.m_Planenum];
                    if (!(plane.m_Normal.Z > plane.m_Normal.X && plane.m_Normal.Z > plane.m_Normal.Y))
                        continue;

                    for (int e = 0; e < face.m_Numedges; e++)
                    {
                        float dist = 1f;
                        int surfedge = map.m_Surfedges[face.m_Firstedge + e];
                        dedge_t edge = map.m_Edges[System.Math.Abs(surfedge)];
                        if(surfedge > 0)
                        {
                            line3d1 = map.m_Vertexes[edge.m_V[0]];
                            line3d2 = map.m_Vertexes[edge.m_V[1]];
                        }
                        else
                        {
                            line3d1 = map.m_Vertexes[edge.m_V[1]];
                            line3d2 = map.m_Vertexes[edge.m_V[0]];
                        }
                        var p3d1 = new Vector3(line3d1.m_Position.X, line3d1.m_Position.Y, line3d1.m_Position.Z);
                        var p3d2 = new Vector3(line3d2.m_Position.X, line3d2.m_Position.Y, line3d2.m_Position.Z);
                        if (!allPositions.Any(x => 
                        ((p3d1 - x).Length < 200f && (p3d2 - x).Length < 200f) && 
                        (System.Math.Abs(p3d1.Z - x.Z) >= 0 && System.Math.Abs(p3d1.Z - x.Z)<=72)
                        ))
                            continue;

                        Vector2 p1 = Vector2.Zero, p2 = Vector2.Zero;
                        if (!w2s(ref p1, p3d1) ||
                            !w2s(ref p2, p3d2))
                            continue;

                        Program.Hack.Overlay.Visuals.DrawLine(Program.CurrentSettings.ESP.World.Color, p1, p2);
                    }
                }
            }
        }

        private void DrawModels()
        {
            if (Program.Hack.StateMod.Map != null)
            {
                var map = Program.Hack.StateMod.Map;
                Vector2 pos = Vector2.Zero, dir = Vector2.Zero;
                foreach(var prop in map.m_StaticProps)
                {
                    if (prop.PropType >= map.m_StaticPropsModelNames.Length)
                        continue;

                    var angles = new Vector3(prop.Angles);
                    angles.Normalize();

                    if ((prop.Origin - Program.Hack.StateMod.LocalPlayer.Value.m_vecOrigin).Length > 500f || 
                        !w2s(ref pos, prop.Origin) ||
                        !w2s(ref dir, prop.Origin + angles))
                        continue;

                    Program.Hack.Overlay.Visuals.DrawLine(Color.White, pos, dir);
                    var name = map.m_StaticPropsModelNames[prop.PropType];
                    if (name.Length > 32)
                        name = name.Split("/".ToCharArray()).Last();
                    Program.Hack.Overlay.Visuals.DrawString(Color.White, espFont, pos, name);                    
                }
            }
        }

        private void DrawBones()
        {
            var lp = Program.Hack.StateMod.LocalPlayer.Value;
            if (lp == null ||lp.m_iCrosshairIdx <= 0 || lp.m_iCrosshairIdx >= 64)
                return;

            var enemy = Program.Hack.StateMod.Players[lp.m_iCrosshairIdx];
            if (enemy == null || enemy.m_iID == lp.m_iID)
                return;

            var boneVecs = enemy.m_Skeleton.m_Bones.Select(x => x.ToVector()).Where(x=>(enemy.m_vecOrigin-x).Length <= 72).ToArray();

            var i = 0;
            foreach(var bone in boneVecs)
            {
                var pos = new Vector2();
                if (w2s(ref pos, bone))
                {
                    var meters = DistToMeters((lp.m_vecOrigin - bone).Length);
                    var size = new Vector2(4);
                    Program.Hack.Overlay.Visuals.DrawRectangle(Color.Green, pos - size*0.5f, size);
                    Program.Hack.Overlay.Visuals.DrawString(Color.White, espFont, new Vector2(pos.X - espFont.Height / 2f, pos.Y - espFont.Height / 2f), i.ToString());
                }
                i++;
            }
        }

        public static float MetersToUnits(float dist)
        {
            return dist / 0.01905f;
        }

        public static float DistToMeters(float dist)
        {
            return dist * 0.01905f;
            //return dist / 12f * 0.3048f / 1000;
        }
        private void DrawEntityEsp(Vector2 upperLeft, Vector2 size, ESPEntry settings, string text, float armor = 0f, float life = 0f)
        {
            if (settings.ShowBox)
                DrawOutlinedRect(upperLeft, size, settings.Color, Color.Black);

            if (settings.ShowHealth)
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
                Program.Hack.Overlay.Visuals.DrawString(settings.Color, espFont, textPos, text);
            }
        }

        private void DrawPlayers(IEnumerable<CSPlayer> players, ESPEntry settings)
        {
            List<Vector2> points = new List<Vector2>();
            foreach (var player in players)
            {
                if (!player.IsValid || player.IsDormant || player.m_lifeState != Enums.LifeState.Alive)
                    continue;

                var _head = player.m_Skeleton.m_Bones[8].ToVector() + Vector3.UnitZ * 15;
                var _feet = player.m_Skeleton.m_Bones[1].ToVector() - Vector3.UnitZ * 10;
                Vector2 head = Vector2.Zero, feet = Vector2.Zero;
                if (!w2s(ref head, _head) || !w2s(ref feet, _feet))
                    continue;

                var width = (feet - head).Y * 0.5f;
                var x = (head.X + feet.X) / 2f;

                var position = new Vector2(x - width / 2, head.Y);
                var size = new Vector2(width, feet.Y - head.Y);
                DrawPlayer(player, settings, position, size);

                points.Clear();
            }
        }

        private void DrawPlayer(CSPlayer player, ESPEntry settings, Vector2 position, Vector2 size)
        {
            var drawColor = settings.Color;
            var map = Program.Hack.StateMod.Map;
            if (map != null)
            {
                var from = Program.Hack.StateMod.LocalPlayer.Value.m_vecOrigin + Program.Hack.StateMod.LocalPlayer.Value.m_vecViewOffset;
                var to = player.m_Skeleton.m_Bones[8].ToVector();
                if (!map.IsVisible(from, to))
                    drawColor = settings.ColorOccluded;
            }
            if (settings.ShowBox)
            {
                Program.Hack.Overlay.Visuals.DrawRectangle(drawColor, position, size);
                //DrawOutlinedRect(position, size, drawColor, Color.Black);
            }
            if (settings.ShowHealth)
            {
                var lifePerc = System.Math.Max(System.Math.Min(player.m_iHealth, 100), 0) / 100f;
                var lifeFrom = position - Vector2.UnitX * 2f;
                var lifeTo = lifeFrom + Vector2.UnitY * size.Y * lifePerc;
                Program.Hack.Overlay.Visuals.DrawLine(Color.Green, lifeFrom, lifeTo);

                //var lifeSize = espFont.MeasureString(player.m_iHealth.ToString());
                //var lifeText = lifeFrom - Vector2.UnitY * lifeSize.Y - Vector2.UnitX * lifeSize.X * 0.5f;
                //Program.Hack.Overlay.Visuals.DrawString(Color.White, espFont, lifeText, player.m_iHealth.ToString());

                var armorPerc = System.Math.Max(System.Math.Min(player.m_ArmorValue, 100), 0) / 100f;
                var armorFrom = position  + Vector2.UnitX * size.X + Vector2.UnitX * 2f;
                var armorTo = armorFrom + Vector2.UnitY * size.Y * armorPerc;
                Program.Hack.Overlay.Visuals.DrawLine(Color.Blue, armorFrom, armorTo);

                //var armorSize = espFont.MeasureString(player.m_ArmorValue.ToString());
                //var armorText = armorFrom - Vector2.UnitY * armorSize.Y - Vector2.UnitX * armorSize.X * 0.5f;
                //Program.Hack.Overlay.Visuals.DrawString(Color.White, espFont, armorText, player.m_ArmorValue.ToString());
            }
            var infoMiddle = position + Vector2.UnitY * size.Y + Vector2.UnitX * size.X * 0.5f + Vector2.UnitY * 2f;
            if (settings.ShowName && Program.Hack.StateMod.PlayerResources.Value != null)
            {
                var name = Program.Hack.StateMod.PlayerResources.Value.m_sNames[player.m_iID];
                if (name != null)
                {
                    var nameSize = espFont.MeasureString(name);
                    Program.Hack.Overlay.Visuals.DrawString(drawColor, espFont, infoMiddle + Vector2.UnitX * (nameSize.X * -0.5f), name);
                    infoMiddle += Vector2.UnitY * nameSize.Y;
                }
            }
            if (settings.ShowDist)
            {
                var dist = System.Math.Ceiling(DistToMeters((player.m_vecOrigin - Program.Hack.StateMod.LocalPlayer.Value.m_vecOrigin).Length)).ToString() + "m";
                var distSize = espFont.MeasureString(dist);
                Program.Hack.Overlay.Visuals.DrawString(drawColor, espFont, infoMiddle + Vector2.UnitX * (distSize.X * -0.5f), dist);
            }
            if (settings.ShowWeapon)
            {
                var wep = player.m_ActiveWeapon.Value;
                if (wep != null)
                    DrawWeaponIcon(
                        wep.WeaponId,
                        position + Vector2.UnitX * size.X * 0.5f - Vector2.UnitY * 10f,
                        DistToMeters((player.m_vecOrigin - Program.Hack.StateMod.LocalPlayer.Value.m_vecOrigin).Length) * 0.5f,
                        settings.Color);
            }

            //Loot stuff
            
            //var lootstuff = Enumerable.Range(0, 1024).Select(x => Program.Hack.StateMod.BaseEntitites[x]).Where(x => 
            // x!=null && x.m_ClientClass!=null &&(
            //    x.m_ClientClass.ClassID == 47 ||
            //    x.m_ClientClass.ClassID == 48 ||
            //    x.m_ClientClass.ClassID == 123 ||
            //    x.m_ClientClass.ClassID == 124 ||
            //    x.m_ClientClass.ClassID == 125
            //    )).ToArray();
            //DrawEspSet(lootstuff, new ESPEntry() { Enabled = true, Color = Color.Orange, ColorOccluded = Color.Orange, ShowName = true, ShowBox=true },
            //    x => x.m_vecOrigin, x => x.m_vecOrigin, (d, u) => Vector2.Unit*32f, x => 100f, x => 100f, (x, e) => x.m_ClientClass.NetworkName.Value);
        }

        private void DrawEsp<T>(
            T ent,
            ESPEntry settings,
            Func<T, Vector3> fnGetBottom,
            Func<T, Vector3> fnGetTop,
            Func<Vector2, Vector2, Vector2> fnGetSize,
            Func<T, float> fnGetLife,
            Func<T, float> fnGetArmor,
            Func<T, ESPEntry, string> fnGetText) where T : BaseEntity
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

            if (settings.ShowHealth && fnGetLife != null && fnGetArmor != null)
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
                Program.Hack.Overlay.Visuals.DrawString(settings.Color, espFont, textPos, text);
            }
        }

        private void DrawEspSet<T>(
            IEnumerable<T> ents,
            ESPEntry settings,
            Func<T, Vector3> fnGetBottom,
            Func<T, Vector3> fnGetTop,
            Func<Vector2, Vector2, Vector2> fnGetSize,
            Func<T, float> fnGetLife,
            Func<T, float> fnGetArmor,
            Func<T, ESPEntry, string> fnGetText) where T : BaseEntity
        {
            foreach (var e in ents)
                DrawEsp<T>(e, settings, fnGetBottom, fnGetTop, fnGetSize, fnGetLife, fnGetArmor, fnGetText);
        }

        
        private void DrawWeaponSet(CSLocalPlayer lp, IEnumerable<BaseCombatWeapon> weapons, ESPEntry settings)
        {
            //DrawEspSet<BaseCombatWeapon>(
            //    weapons,
            //    settings,
            //    (w) => w.m_vecOrigin,
            //    (w) => w.m_vecOrigin,
            //    (d, u) =>
            //    {
            //        var f = d - u;
            //        return new Vector2(System.Math.Max(f.X, f.Y));
            //    },
            //    null,
            //    null,
            //    (w, s) =>
            //    {
            //        string text = "";
            //        if (settings.ShowName)
            //            text += string.Format("{0} [{1}]", 
            //                w.m_ClientClass.NetworkName.Replace("CWeapon", ""),
            //                w.m_iClip1 >= 0 ? w.m_iClip1.ToString() : "/") + "\n";

            //        if (settings.ShowDist)
            //            text += string.Format("[{0}m]", DistToMeters(lp.DistanceTo(w)).ToString("0.00")) + "\n";

            //        return text;
            //    }
            //);
            foreach (var wep in weapons)
                //if((wep.m_vecOrigin - Program.Hack.StateMod.LocalPlayer.Value.m_vecOrigin).Length < 50)
                DrawWeapon(lp, wep, settings);
        }

        private void DrawWeapon(CSLocalPlayer lp, BaseCombatWeapon weapon, ESPEntry settings)
        {
            var up = Vector2.Zero;
            var down = Vector2.Zero;
            if (!w2s(ref up, weapon.m_vecOrigin + Vector3.UnitZ * 5f) || !w2s(ref down, weapon.m_vecOrigin - Vector3.UnitZ * 5f))
                return;
            var mid = (down + up) * 0.5f;
            var delta = weapon.m_vecOrigin - lp.m_vecOrigin;
            var size = new Vector2((down - up).Y);
            
            if (settings.ShowDist || settings.ShowName)
            {
                var text = "";
                if (settings.ShowName)
                {
                    text += string.Format("{0} [{1}/{2}]",
                        weapon.WeaponIDName.ToString().Substring(6),
                        //weapon.m_ClientClass.NetworkName.Value.Replace("CWeapon", ""),
                            weapon.m_iClip1 >= 0 ? weapon.m_iClip1.ToString() : "-",
                            weapon.m_iClip2 >= 0 ? weapon.m_iClip2.ToString() : "-")+"\n";
                }
                if(settings.ShowDist)
                {
                    text+= DistToMeters(lp.DistanceTo(weapon)).ToString("0.00") +"m";
                }
                var nameSize = espFont.MeasureString(text);
                Program.Hack.Overlay.Visuals.DrawString(settings.Color, espFont, mid + Vector2.UnitY * size.Y - nameSize * 0.5f, text);
            }
            DrawWeaponIcon(weapon.WeaponId, mid, lp.DistanceTo(weapon), settings.Color);
        }

        private void DrawWeaponIcon(int weaponIndex, Vector2 position, float distance, Color color)
        {
            var meters = distance;
            meters = System.Math.Min(System.Math.Max(1, meters), 20);
            var index = (int)System.Math.Ceiling(weaponFonts.Length * meters / 20) - 1;
            var fnt = weaponFonts[index];
            var textSize = fnt.MeasureString(((char)(0xE000 + weaponIndex)).ToString());
            Program.Hack.Overlay.Visuals.DrawString(color, fnt, position - textSize * 0.5f, ((char)(0xE000 + weaponIndex)).ToString());
        }

        private void DrawBaseEntitySet(CSLocalPlayer lp, IEnumerable<BaseEntity> ents, ESPEntry settings)
        {
            DrawEspSet<BaseEntity>(
                ents,
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
                        text += w.m_ClientClass.NetworkName + "\n";

                    if (settings.ShowDist)
                        text += string.Format("[{0}m]", DistToMeters(lp.DistanceTo(w)).ToString("0.00")) + "\n";

                    return text;
                }
            );
        }


        private void DrawOutlinedRect(Vector2 upperLeft, Vector2 size, Color border, Color outline)
        {
            Program.Hack.Overlay.Visuals.DrawRectangle(
                    outline,
                    upperLeft - Vector2.Unit,
                    size + Vector2.Unit * 2f);
            Program.Hack.Overlay.Visuals.DrawRectangle(
                outline,
                upperLeft + Vector2.Unit,
                size - Vector2.Unit * 2f);
            Program.Hack.Overlay.Visuals.DrawRectangle(
                border,
                upperLeft,
                size);
        }
        private void DrawVBar(Vector2 upperLeft, Vector2 size, float fillPerc, Color border, Color bg, Color fill, Color outline)
        {
            Program.Hack.Overlay.Visuals.FillRectangle(bg, upperLeft, size);

            float percHeight = System.Math.Min(1f, System.Math.Max(0f, fillPerc));
            DrawOutlinedRect(
                new Vector2(upperLeft.X, upperLeft.Y + size.Y - size.Y * percHeight),
                new Vector2(size.X, size.Y * percHeight),
                fill, outline);
            Program.Hack.Overlay.Visuals.FillRectangle(fill,
                new Vector2(upperLeft.X, upperLeft.Y + size.Y - size.Y * percHeight),
                new Vector2(size.X, size.Y * percHeight));

            Program.Hack.Overlay.Visuals.DrawRectangle(border, upperLeft, size);
            //DrawOutlinedRect(upperLeft, size, border, outline);
        }
        private void DrawHBar(Vector2 upperLeft, Vector2 size, float fillPerc, Color border, Color bg, Color fill, Color outline)
        {
            Program.Hack.Overlay.Visuals.FillRectangle(bg, upperLeft, size);

            float percWidth = System.Math.Min(1f, System.Math.Max(0f, fillPerc));
            DrawOutlinedRect(
                new Vector2(upperLeft.X, upperLeft.Y),
                new Vector2(size.X * percWidth, size.Y),
                fill, outline);
            Program.Hack.Overlay.Visuals.FillRectangle(fill,
                new Vector2(upperLeft.X, upperLeft.Y),
                new Vector2(size.X * percWidth, size.Y));

            Program.Hack.Overlay.Visuals.DrawRectangle(border, upperLeft, size);
            //DrawOutlinedRect(upperLeft, size, border, outline);
        }
        #endregion
    }
}
