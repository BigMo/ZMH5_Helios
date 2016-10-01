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
using ZatsHackBase.UI.Drawing;
using _ZMH5__Helios.CSGO.Modules;
using _ZMH5__Helios.CSGO.BSP;

namespace _ZMH5__Helios.CSGO
{
    public class Heke : Hack
    {
        #region VARIABLES
        private Font dbg = Font.CreateDummy("Courier New", 10);
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
        #endregion

        #region CONSTRUCTORS
        public Heke() : base("csgo", 60, true)
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

            ClientDll = WaitForModule("client.dll");
            EngineDll = WaitForModule("engine.dll");
#if DEBUG
            Program.Logger.Log("Performing SigScans...");
            Program.Offsets.SigScan();
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
#endif
            Program.Logger.Info("Helios is ready.");
        }

        protected override void AfterRun()
        {
            base.AfterRun();

            Program.Logger.Info("Terminating.");
        }

        public T GetEntityByAddress<T>(int address, bool addToSnapshot = true) where T : EntityPrototype, new()
        {
            if (address <= 0)
                return null;

            try
            {
                T entity = new T();
                entity.Init(address, entity.MemSize);
                return entity;
            }catch { return null; }
        }

        public T GetEntityByID<T>(int id, bool addToSnapshot = true) where T : EntityPrototype, new()
        {
            int address = Program.Hack.Memory.Read<int>(ClientDll.BaseAddress.ToInt32() + Program.Offsets.EntityList + 16 * (id - 1));
            if (address == 0)
                return null;

            return GetEntityByAddress<T>(address);
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
            dbg = Overlay.Renderer.Fonts[dbg];

            Overlay.Renderer.DrawString(Color.White, dbg, Vector2.Unit * 20f, 
                Input.MousePos.ToString() + "\n" +
                Input.MouseMoveDist.ToString() + "\n" + 
                string.Join(", ", Input.KeysDown.Select(x=>x.ToString())));

            base.AfterPluginsTick(args);
        }
        #endregion
    }
}
