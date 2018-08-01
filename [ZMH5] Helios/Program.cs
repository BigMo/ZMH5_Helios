using _ZMH5__Helios.CSGO;
using ZatsHackBase.Core;
using ZatsHackBase.IO;
using ZatsHackBase.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZatsHackBase.UI;
using System.Windows.Forms;
using _ZMH5__Helios.UI;
using System.IO;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios
{
    class Program
    {
        #region PROPERTIES
        public static Heke Hack { get; private set; }
        public static Logger Logger { get; private set; }
        public static ConsoleAnimation Animation { get; private set; }
        public static string Name { get; private set; }
        public static Settings[] AllSettings { get; private set; }
        public static Settings CurrentSettings { get; set; }
        public static Offsets Offsets { get; private set; }
        public static IntPtr ConfigWindowHandle { get; private set; }
        #endregion

        [STAThread]
        static void Main(string[] args)
        {

            Animation = new ConsoleAnimation();
            Animation.Text = Name = string.Format("[ZMH5] Helios v.{0}", Assembly.GetExecutingAssembly().GetName().Version);
            Animation.Delimiter = "          //          ";
            Animation.RunningOutOfNews += (o, e) => Animation.AppendNews(Memes.Captions[new Random().Next(0, Memes.Captions.Length)]);
            Animation.Start();
            Animation.Interval = (int)(1000f / 15f);

            Logger = new Logger();
            Logger.PrintDate = Logger.PrintTime = false;
            Logger.Log("### [Helios] started ###");
            Logger.Info("Loaded {0} memes", Memes.Captions.Length);

            LoadSettings();
            CurrentSettings.Save();

            LoadOffsets();
            Offsets.Save("offsets.json");
            //Wait for game
            Logger.Warning("WAITING FOR CSGO...");
            
            while (!EUCProcess.IsProcessRunning("csgo"))
                Thread.Sleep(500);

            Hack = new Heke();

            var thread = new Thread(()=> {
                var cfg = new ConfigForm();
                ConfigWindowHandle = cfg.Handle;
                Application.Run(cfg);
                });
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();

            Logger.Info("> Running hack!");
            Hack.Run();
            Console.ReadLine();
        }

        public static void LoadSettings()
        {
            try
            {
                if (!Directory.Exists("configs"))
                    Directory.CreateDirectory("configs");

                var files = Directory.GetFiles("configs", "*.json");
                if (files.Length == 0)
                    throw new Exception();

                AllSettings = files.Select(x => Settings.FromFile(x)).ToArray();
                CurrentSettings = AllSettings.FirstOrDefault(x => x.IsActive) ?? AllSettings.First();
            }
            catch (Exception ex)
            {
                CurrentSettings = new Settings("dummySettings");
                CurrentSettings.Save();
                Logger.Error("Failed to read settings!");
                Logger.PrintException(ex, true, false);
            }
        }
        public static void LoadOffsets()
        {
            try
            {
                Offsets = Offsets.FromFile("offsets.json");
            }
            catch (Exception ex)
            {
                Offsets = new Offsets();
                Offsets.Save("offsets.json");
                Logger.Error("Failed to read offsets!");
                Logger.PrintException(ex, true, false);
            }
        }
    }
}
