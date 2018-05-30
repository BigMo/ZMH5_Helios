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
using ZatsHackBase.UI.Drawing.FontHelpers;
using Grapevine.Server;
using System.Windows.Forms;
using _ZMH5__Helios.UI;
using System.IO;

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
            //for (int i = 0; i < 20; i++)
            //{
            //    GlyphAtlas t = new ZatsHackBase.UI.Drawing.FontHelpers.GlyphAtlas(new GlyphAtlas.CharRange[] {
            //        new GlyphAtlas.CharRange((char)32, (char)1000), //Basic
            //        new GlyphAtlas.CharRange((char)0x0400, (char)0x04ff), //Cyrillic
            //        new GlyphAtlas.CharRange((char)0x0500, (char)0x052f), //Cyrillic Supplementary
            //        new GlyphAtlas.CharRange((char)0x02b0, (char)0x02ff), //Block Elements
            //        new GlyphAtlas.CharRange((char)0x2580, (char)0x259f), //Block Elements
            //        new GlyphAtlas.CharRange((char)0x25A0, (char)0x25ff) //Geometric Shapes
            //    }, ZatsHackBase.Maths.Vector2.Unit * 3f);
            //    t.InitDebug("Segoe UI", 10 + i);
            //}
            //return;

            new Test().Run();
            return;
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
            CurrentSettings.Save();

            LoadOffsets();
            Offsets.Save("offsets.json");
            //Wait for game
            Logger.Warning("WAITING FOR CSGO...");
            var server = new RestServer(new ServerSettings()
            {
                 Host = "localhost",
                 Port = "1337",
                 PublicFolder = new PublicFolder("html")
            });
            server.Start();
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
            server.Stop();
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
