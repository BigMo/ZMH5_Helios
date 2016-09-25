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

namespace _ZMH5__Helios
{
    class Program
    {
        #region PROPERTIES
        public static Heke Hack { get; private set; }
        public static Logger Logger { get; private set; }
        public static ConsoleAnimation Animation { get; private set; }
        public static string Name { get; private set; }
        public static Settings Settings { get; private set; }
        public static Offsets Offsets { get; private set; }
        #endregion

        static void Main(string[] args)
        {
            //Setup
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
            LoadOffsets();

            //Wait for game
            Logger.Warning("WAITING FOR CSGO...");
            while (!EUCProcess.IsProcessRunning("spotify"))
                Thread.Sleep(500);

            Hack = new Heke();

            Logger.Info("> Running hack!");
            Hack.Run();
            Console.ReadLine();
        }

        private static void LoadSettings()
        {
            try
            {
                Settings = Settings.FromFile("settings.json");
            }
            catch (Exception ex)
            {
                Settings = new Settings();
                Settings.Save("settings.json");
                Logger.Error("Failed to read settings!");
                Logger.PrintException(ex, true, false);
            }
        }
        private static void LoadOffsets()
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
