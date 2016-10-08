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
using System.Windows.Forms;
using ZatsHackBase.UI;
using ZatsHackBase.UI.Drawing.FontHelpers;

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
            Application.SetCompatibleTextRenderingDefault(false);
            new Test();
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
            Settings.Save("settings.json");

            LoadOffsets();
            Offsets.Save("offsets.json");
            //Wait for game
            Logger.Warning("WAITING FOR CSGO...");
            while (!EUCProcess.IsProcessRunning("spotify"))
                Thread.Sleep(500);

            Hack = new Heke();

            Logger.Info("> Running hack!");
            Hack.Run();
            Console.ReadLine();
        }

        public static void LoadSettings()
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
