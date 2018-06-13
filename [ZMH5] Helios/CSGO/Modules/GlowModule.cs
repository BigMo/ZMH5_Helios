using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using _ZMH5__Helios.CSGO.Modules.GlowHelpers;
using _ZMH5__Helios.CSGO.Entities;
using ZatsHackBase;
using _ZMH5__Helios.CSGO.Misc;
using ZatsHackBase.Drawing;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class GlowModule : HackModule
    {
        #region VARIABLES
        private GlowManager glowManager;
        #endregion

        #region PROPERTIES
        //public GlowArray Array { get; private set; }
        public RPMLazyArray<GlowObjectDefinition> Array { get; private set; }
        #endregion

        public GlowModule() : base(Program.Hack, ModulePriority.High)
        {
        }
        
        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            Program.Logger.Log("sizeof(GlowObjectDefinition): {0}", RPMLazyArray<GlowObjectDefinition>.Size);
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            glowManager = Program.Hack.Memory.Read<GlowManager>(Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.GlowManager);

            if (glowManager.m_pGlowArray == 0 || glowManager.m_iNumObjects == 0)
                return;

            var lp = Program.Hack.StateMod.LocalPlayer.Value;

            Array = new RPMLazyArray<GlowObjectDefinition>(glowManager.m_pGlowArray, glowManager.m_iNumObjects);

            for (int i = 0; i < glowManager.m_iNumObjects; i++)
            {
                var obj = Array[i];
                var proto = Program.Hack.GetEntityByAddress<EntityPrototype>(obj.pEntity);

                if (proto == null || !proto.IsValid)
                    continue;
                
                BaseCombatWeapon wep = null;
                CSPlayer player = null;

                if (BaseCombatWeapon.IsWeapon(proto))
                    wep = Program.Hack.GetEntityByAddress<BaseCombatWeapon>(obj.pEntity);
                if (CSPlayer.IsPlayer(proto))
                    player = Program.Hack.GetEntityByAddress<CSPlayer>(obj.pEntity);

                if (wep != null && wep.IsValid)
                {
                    if (Program.CurrentSettings.Glow.C4.Enabled && wep.IsC4)
                        EncolorObject(obj, Program.CurrentSettings.Glow.C4.Color, i);
                    else if (Program.CurrentSettings.Glow.Grenades.Enabled && wep.IsGrenadeProjectile)
                        EncolorObject(obj, Program.CurrentSettings.Glow.Grenades.Color, i);
                    else if (Program.CurrentSettings.Glow.Weapons.Enabled)
                        EncolorObject(obj, Program.CurrentSettings.Glow.Weapons.Color, i);

                    Program.Hack.StateMod.Weapons[wep.m_iID] = wep;
                }
                else if (player != null && player.IsValid)
                {
                    if (lp == null || !lp.IsValid || ((Heke)Hack).AimBot.CurrentTarget == player.m_iID.Value)
                        continue;

                    bool friend = lp.m_iTeamNum.Value == player.m_iTeamNum.Value;
                    if (Program.CurrentSettings.Glow.Allies.Enabled && friend)
                        EncolorObject(obj, Program.CurrentSettings.Glow.Allies.Color, i);
                    if (Program.CurrentSettings.Glow.Enemies.Enabled && !friend)
                        EncolorObject(obj, Program.CurrentSettings.Glow.Enemies.Color, i);
                }
                else
                {
                    if (proto.m_ClientClass.Value.NetworkName.Value == "CChicken")
                        EncolorObject(obj, Color.Orange, i);

                    var baseEnt = Program.Hack.GetEntityByAddress<BaseEntity>(proto.Address);

                }
            }
        }

        public void EncolorObject(Color color, int index)
        {
            EncolorObject(Array[index], color, index);
        }
        public void EncolorObject(GlowObjectDefinition obj, Color color, int index)
        {
            obj.m_bRenderWhenOccluded = 1;
            obj.m_bRenderWhenUnoccluded = 0;
            obj.m_bFullBloom = 0;
            obj.a = color.A;
            obj.r = color.R;
            obj.g = color.G;
            obj.b = color.B;
            WriteElement(obj, index);
        }

        public void WriteElement(GlowObjectDefinition obj, int index)
        {
            if (Array.Length <= index)
                return;
            try
            {
                int address = glowManager.m_pGlowArray + RPMLazyArray<GlowObjectDefinition>.Size * index;
                Program.Hack.Memory.Write<GlowObjectDefinition>(address, obj, 0x04, 0x14); //colors
                Program.Hack.Memory.Write<GlowObjectDefinition>(address, obj, 0x24, 0x03); //flags
            }catch { }
        }
    }
}
