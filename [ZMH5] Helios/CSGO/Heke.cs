using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using System.Diagnostics;
using System.Threading;
using _ZMH5__Helios.CSGO.ClassIDs;
using _ZMH5__Helios.CSGO.Entities;
using _ZMH5__Helios.CSGO.Enums;
using ZatsHackBase;
using ZatsHackBase.Misc;
using System.IO;
using ZatsHackBase.Maths;
using ZatsHackBase.UI;
using ZatsHackBase.Drawing;
using _ZMH5__Helios.CSGO.Modules;
using _ZMH5__Helios.CSGO.BSP;
using ZatsHackBase.UI.Controls;

namespace _ZMH5__Helios.CSGO
{
    public class Heke : Hack
    {
        #region VARIABLES
        private Font dbg = Font.CreateDummy("Segoe UI", 14, true);
        #endregion

        #region PROPERTIES
        public ProcessModule ClientDll { get; private set; }
        public ProcessModule EngineDll { get; private set; }
        public EchoModule EchoMod { get; private set; }
        public StateModule StateMod { get; private set; }
        public BunnyHop BunnyHop { get; private set; }
        public TriggerModule TriggerBot { get; private set; }
        public ViewModule View { get; private set; }
        public NoRecoilModule RCS { get; private set; }
        public AimModule AimBot { get; private set; }
        public AutoPistolModule Pistol { get; private set; }
        public GlowModule Glow { get; private set; }
        public BSPFile Map { get; private set; }
        public ESPModule ESP { get; private set; }
        public ReloadSettingsModule ReloadSettings { get; private set; }

        public bool IsOnServer { get { return Program.Hack.StateMod.ClientState.Value != null && Program.Hack.StateMod.ClientState.Value.State.Value == Modules.SnapshotHelpers.ClientState.SignOnState.Full; } }
        #endregion

        #region CONSTRUCTORS
        public Heke() : base("csgo", 999, true, false)
        { }
        #endregion

        #region METHODS
        private ProcessModule WaitForModule(string moduleName)
        {
            Program.Logger.Log("Waiting for game to load {0}...", moduleName);
            while (!this.Process.Process.Modules.OfType<ProcessModule>().Any(x => x.ModuleName == moduleName))
            {
                Process.Process.Refresh();
                Thread.Sleep(300);
            }

            var mod = this.Process.Process.Modules.OfType<ProcessModule>().First(x => x.ModuleName == moduleName);
            Program.Logger.Log("- {0} loaded at 0x{1}", moduleName, mod.BaseAddress.ToInt32().ToString("X8"));

            return mod;
        }

        protected override void OnFirstTick(TickEventArgs args)
        {
            base.OnFirstTick(args);

            ClientDll = WaitForModule("client_panorama.dll");
            EngineDll = WaitForModule("engine.dll");
#if DEBUG
            Program.Logger.Log("Performing SigScans...");
            Program.Offsets.SigScan();
            Program.Offsets.Save("offsets.json");
#endif
            Program.Logger.Log("Grabbing ClientClasses...");
            do
            {
                ClientClassParser.Parse();
                Thread.Sleep(500);
            } while (ClientClassParser.DataTables.Count == 0);
#if DEBUG
            Program.Logger.Log("Dumping ClientClasses...");
            ClientClassParser.DumpClassIDs();
            ClientClassParser.DumpNetVars(false, "netvars.txt");
            ClientClassParser.DumpNetVars(true, "netvars_full.txt");
            ClientClassParser.DumpNetVarsJson("netvars.json");
            ClientClassParser.DumpCppClasses("clientclasses.cpp");
#endif
            Program.Logger.Info("Helios is ready.");

            //Window window = null;
            //using (var frm = new UI.TestForm())
            //    window = ControlConverter.Convert(frm);

            //Overlay.Controls.Add(window);
        }

        protected override bool ProcessModules()
        {
            return Process.IsInForeground || WinAPI.GetForegroundWindow() == Program.ConfigWindowHandle;
        }

        protected override bool ProcessInput()
        {
            return Process.IsInForeground;
        }

        protected override void AfterRun()
        {
            base.AfterRun();

            Program.Logger.Info("Terminating.");
        }

        public T GetEntityByAddress<T>(IntPtr address, bool addToSnapshot = true) where T : EntityPrototype, new()
        {
            /*if (address <= 0)
                return null;*/

            //try
            //{





            T entity = new T();
            entity.Init(Memory, address);

            if (addToSnapshot && entity != null && entity.IsValid)
            {
                var t = typeof(T);
                if (t == typeof(BaseEntity))
                    StateMod.BaseEntitites[(entity as BaseEntity).m_iID] = entity as BaseEntity;
                else if (t == typeof(BaseCombatWeapon))
                    StateMod.BaseEntitites[(entity as BaseCombatWeapon).m_iID] = entity as BaseCombatWeapon;
                else if (t == typeof(CSPlayer))
                    StateMod.BaseEntitites[(entity as CSPlayer).m_iID] = entity as CSPlayer;
            }

            return entity;
            //}catch { return null; }
        }

        public T GetEntityByID<T>(int id, bool addToSnapshot = true) where T : EntityPrototype, new()
        {
            var address = Program.Hack.Memory.Read<IntPtr>(ClientDll.BaseAddress.ToInt32() + Program.Offsets.EntityList + 16 * (id - 1));
            if ((int)address == 0)
                return null;

            var ent = GetEntityByAddress<T>(address);
            return ent;
        }

        protected override void SetupModules()
        {
            EchoMod = new EchoModule();
            StateMod = new StateModule();
            BunnyHop = new BunnyHop();
            TriggerBot = new TriggerModule();
            View = new ViewModule();
            RCS = new NoRecoilModule();
            AimBot = new AimModule();
            Pistol = new AutoPistolModule();
            Glow = new GlowModule();
            ESP = new ESPModule();
            ReloadSettings = new ReloadSettingsModule();

            AddModule(EchoMod);
            AddModule(StateMod);
            AddModule(BunnyHop);
            AddModule(TriggerBot);
            AddModule(View);
            AddModule(RCS);
            AddModule(AimBot);
            AddModule(Pistol);
            AddModule(Glow);
            AddModule(ESP);
            AddModule(ReloadSettings);
        }

        protected override void AfterPluginsTick(TickEventArgs args)
        {
            dbg = Program.Hack.Overlay.Renderer.Fonts[dbg];

            if (Process.IsInForeground)
            {
                //Input
                Overlay.Menu.DrawString(Color.White, dbg, Vector2.Unit * 20f,
                    Input.MousePos.ToString() + "\n" +
                    Input.MouseMoveDist.ToString() + "\n" +
                    string.Join(", ", Input.KeysDown.Select(x => x.ToString())));

                //Mem
                var str = string.Format(
                    "=============================\n" +
                    "RPM: {0} calls\n" +
                    "     {1} total\n" +
                    "     {2}/s\n" +
                    "WPM: {3} calls\n" +
                    "     {4} total\n" +
                    "     {5}/s\n" +
                    "=============================",
                    Memory.RPMCalls.ToString("N0"), SizeFormatter.GetUnitFromSize(Memory.BytesIn, true), SizeFormatter.GetUnitFromSize(Memory.BytesIn / args.Time.TotalTime.TotalSeconds, true),
                    Memory.WPMCalls.ToString("N0"), SizeFormatter.GetUnitFromSize(Memory.BytesOut, true), SizeFormatter.GetUnitFromSize(Memory.BytesOut / args.Time.TotalTime.TotalSeconds, true));
                var size = dbg.MeasureString(str);
                Overlay.Menu.DrawString(Color.White, dbg, Vector2.UnitY * (Overlay.Size.Y * 0.75f - size.Y * 0.5f), str);

                //Specs
                var lp = StateMod.LocalPlayer.Value;
                Color drawColor = Color.White;
                if (CSLocalPlayer.IsProcessable(lp) && !lp.IsDormant)
                {
                    var players = StateMod.GetPlayersSet(true, false, true);
                    Func<CSPlayer, bool> filter = null;
                    filter = (x) => x.m_iID != lp.m_iID && x.m_hObserverTarget == lp.m_iID;
                    if (players.Any(filter))
                    {
                        var specs = players.Where(filter);
                        if (specs.Any(x => x.m_iObserverMode == ObserverMode.Ego))
                            drawColor = Color.Red;
                        else if (specs.Any(x => x.m_iObserverMode == ObserverMode.ThirdPerson))
                            drawColor = Color.Orange;

                        string text = string.Join("\n",
                            specs.Select(x =>
                                string.Format("▻ {0} ({1})",
                                Program.Hack.StateMod.PlayerResources.Value.m_sNames[x.m_iID],
                                x.m_iObserverMode.ToString())
                            )
                        );
                        Overlay.Menu.DrawString(
                            drawColor,
                            dbg,
                            Vector2.UnitY * (Overlay.Size.Y / 2f),
                            "[Specs]\n" + text);
                    }
                    else
                        Overlay.Menu.DrawString(drawColor, dbg, Vector2.UnitY * (Overlay.Size.Y / 2f), "[Specs]\n<none>");
                }
                else
                {
                    Overlay.Menu.DrawString(drawColor, dbg, Vector2.UnitY * (Overlay.Size.Y / 2f), "[Specs]\n<not ingame>");
                }
            }
            base.AfterPluginsTick(args);
        }
        #endregion
    }
}
