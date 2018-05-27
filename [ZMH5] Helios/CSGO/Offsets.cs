using ZatsHackBase.Core.SigScanning;
using ZatsHackBase.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _ZMH5__Helios.CSGO
{
    public class Offsets
    {
        #region PROPERTIES
        public int EntityList { get; set; }
        public int LocalPlayer { get; set; }
        public int ClientState { get; set; }
        public int ClientStateSetViewAngles { get; set; }
        public int ClientStateMapDirectory { get; set; }
        public int ClientStateState { get; set; }
        public int State { get; set; }
        public int ForceAttack { get; set; }
        public int ForceJump { get; set; }
        public int ClassIDBase { get; set; }
        public int GlowManager { get; set; }
        public int ClassIDBaseOffset { get; set; }
        public int ClassIDManager { get; set; }
        public int m_iID { get; set; }
        public int m_iCrosshairID { get; set; }
        public int m_iGlowIndex { get; set; }
        public int m_pBoneMatrix { get; set; }
        public int m_mViewMatrix { get; set; }
        public int GameRulesProxy { get; set; }
        public int PlayerResources { get; set; }
        public int RadarBase { get; set; }
        public int RadarOffset { get; set; }
        public int PlayerResourcesNames { get; set; }
        public int m_iItemDefinitionIndex { get; set; }
        public int GameDirectory { get; set; }
        #endregion

        #region CONSTRUCTORS
        public Offsets()
        { }

        public static Offsets FromFile(string file)
        {
            JsonSerializer s = new JsonSerializer();
            s.Converters.Add(new HexToIntConverter());
            using (StreamReader str = new StreamReader(file))
                return (Offsets)s.Deserialize(str, typeof(Offsets));
        }
        #endregion

        #region METHODS
        public void Save(string file)
        {
            JsonSerializer s = new JsonSerializer();
            s.Formatting = Formatting.Indented;
            s.Converters.Add(new HexToIntConverter());
            using (StreamWriter writer = new StreamWriter(file, false))
                s.Serialize(writer, this);
        }

#if DEBUG
        public void SigScan()
        {
            SigRadarBase();
            SigViewMatrix();
            SigBoneMatrix();
            SigForceAttack();
            SigForceJump();
            SigClientState();
            SigLocalPlayer();
            SigEntityList();
            SigClassIDBase();
            SigClassIDManager();
            SigEntityID();
            SigCrosshairIdx();
            SigSetViewAngles();
            SigGlowManager();
            SigGlowIndex();
            SigGameRulesProxy();
            SigPlayerResources();
            this.m_mViewMatrix = 0x4A76154;
            this.GlowManager = 0x4FB14E8;
            this.ForceAttack = 0x2EC6AC8;
            this.ForceJump = 0x4F1B9E0;
            this.RadarBase = 0x4EB9424;
            PlayerResourcesNames = 0x9E0;
            m_iItemDefinitionIndex = 0x2F88;
            ClientStateMapDirectory = 0x188;
            ClientStateState = 0x108;
            GameDirectory = 0x61D870;
        }
        private void SigRadarBase()
        {
            var scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0x6A, 0x00, 0xFF, 0x90, 0x98, 0x00
            }, "xxxxxx", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var address = Program.Hack.Memory.Read<int>(scan.Address + 0x13);
                var val = address - Program.Hack.ClientDll.BaseAddress.ToInt32();
                RadarBase = val;
            } else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigViewMatrix()
        {
            var scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0xE8, 0x00, 0x00, 0x00, 0x00, 0x8D, 0x95, 0xE0
            }, "x????xxx", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var address = Program.Hack.Memory.Read<int>(scan.Address - 0x1A);
                var val = address + 0x90 - Program.Hack.ClientDll.BaseAddress.ToInt32();
                m_mViewMatrix = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigBoneMatrix()
        {
            //75 15 8B 87 98 26 00 00 8B CF 8B 17 03 44 24 14 50
            var scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0x75, 0x15, 0x8B, 0x87, 0x00, 0x00, 0x00, 0x00, 0x8B, 0xCF, 0x8B, 0x17, 0x03, 0x44, 0x24, 0x14, 0x50
            }, "xxxx????xxxxxxxxx", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var val = scan.Stream.Read<int>(4);
                m_pBoneMatrix = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigForceAttack()
        {
            var scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0x89, 0x15, 0x00, 0x00, 0x00, 0x00, //mov [client.dll+xxxx],edx
                0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, //mov edx, [client.dll+????]
                0xF6, 0xC2, 0x03,                   //test dl, 03
                0x74, 0x03,                         //je client.dll+???? 
                0x83, 0xCE, 0x04,                   //or esi,04
                0xA8, 0x04
            }, "xx????xx????xxxxxxxxxx", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var val = scan.Stream.Read<int>(2) - Program.Hack.ClientDll.BaseAddress.ToInt32();
                ForceAttack = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigForceJump()
        {
            var scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0x89, 0x15, 0x00, 0x00, 0x00, 0x00, //mov [client.dll+xxxx],edx
                0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, //mov edx,[client.dll+xxxx]
                0xF6, 0xC2, 0x03,                   //test dl, 03
                0x74, 0x03,                         //je client.dll+???? 
                0x83, 0xCE, 0x08, //or esi,08
                0xA8, 0x08
            }, "xx????xx????xxxxxxxxxx", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var val = scan.Stream.Read<int>(2) - Program.Hack.ClientDll.BaseAddress.ToInt32();
                ForceJump = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigClientState()
        {
            var scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0xA1, 0x00, 0x00, 0x00, 0x00,
                0x80, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x75, 0x0C,
                0x83, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x7F, 0x03
            }, "x????xx????xxxxx????xxx", Program.Hack.EngineDll);
            if (scan.Success)
            {
                var val = scan.Stream.Read<int>(1) - Program.Hack.EngineDll.BaseAddress.ToInt32();
                ClientState = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigLocalPlayer()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(new byte[]{
                0x8D, 0x34, 0x85, 0x00, 0x00, 0x00, 0x00,       //lea esi, [eax*4+client.dll+xxxx]
                0x89, 0x15, 0x00, 0x00, 0x00, 0x00,             //mov [client.dll+xxxx],edx
                0x8B, 0x41, 0x08,                               //mov eax,[ecx+08]
                0x8B, 0x48, 0x00                                //mov ecx,[eax+04]
                }, "xxx????xx????xxxxx?", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var val1 = scan.Stream.Read<int>(3);
                var val2 = scan.Stream.Read<byte>(18);
                var val = val1 + val2 - Program.Hack.ClientDll.BaseAddress.ToInt32();
                LocalPlayer = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigEntityList()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                //{&HBB, 0, 0, 0, 0, &H83, &HFF, &H1, &HF, &H8C, 0, 0, 0, 0, &H3B, &HF8}
                0xBB, 0x00, 0x00, 0x00, 0x00,
                0x83, 0xFF, 0x01, 0x0f, 0x8C, 0x00, 0x00, 0x00, 0x00, 0x3B, 0xF8
            }, "x????xxxxx????xx", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var val = scan.Stream.Read<int>(1) - Program.Hack.ClientDll.BaseAddress.ToInt32();
                EntityList = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigPlayerResources()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0x8B, 0xF0,
                0x57,
                0x89, 0x75, 0xBC,
                0x8B, 0x01,
                0xFF, 0x90, 0x00, 0x00, 0x00, 0x00,
                0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00,
                0x33, 0xC0
            },
            "xxxxxxxxxx????xx????xx", Program.Hack.ClientDll, true);
            if (scan.Success)
            {
                var val = scan.Stream.Read<int>(16) - Program.Hack.ClientDll.BaseAddress.ToInt32();
                PlayerResources = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigGameRulesProxy()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                0x56,
                0x8B, 0xCF,
                0xE8, 0x00, 0x00, 0x00, 0x00,
                0xFF, 0x75, 0x0C,
                0x8B, 0xCF,
                0x89, 0x3D, 0x00, 0x00, 0x00, 0x00,
                0xFF, 0x75, 0x08
            },
            "xxxx????xxxxxxx????xxx", Program.Hack.ClientDll, true);
            if (scan.Success)
            {
                var val = scan.Stream.Read<int>(15) - Program.Hack.ClientDll.BaseAddress.ToInt32();
                GameRulesProxy = val;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigGlowIndex()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(
                new byte[]
                {
                    0x83, 0xC4, 0x04,
                    0x8B, 0x83, 0x00, 0x00, 0x00, 0x00,
                    0x8D, 0x0C, 0xC5, 0x00, 0x00, 0x00, 0x00,
                    0x2B, 0xC8,
                    0xA1, 0x00, 0x00, 0x00, 0x00,
                    0x89, 0x74, 0xC8, 0x2C,
                    0xF6, 0xC2, 0x01
                }, "xxxxx????xxxxxxxxxx????xxxxxxx", Program.Hack.ClientDll, true);
            if (scan.Success)
            {
                int tmp = scan.Stream.Read<int>(5);
                m_iGlowIndex = tmp;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigGlowManager()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(
                    new byte[] {
                        0x8A, 0x5D, 0xFF,
                        0x8D, 0x0C, 0xC5, 0x00, 0x00, 0x00, 0x00,
                        0x2B, 0xC8,
                        0xA1, 0x00, 0x00, 0x00, 0x00,
                        0x88, 0x5C, 0xC8, 0x24,
                        0xA1, 0x00, 0x00, 0x00, 0x00,
                        0x8B, 0x5D, 0xF8
                    },
                    "xxxxxxxxxxxxx????xxxxx????xxx", Program.Hack.ClientDll, true);
            if (scan.Success)
            {
                int tmp = scan.Stream.Read<int>(13);
                GlowManager = tmp - Program.Hack.ClientDll.BaseAddress.ToInt32();
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigSetViewAngles()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(
                    new byte[] {
                        //0xF3, 0x0F, 0x5C, 0xC1,//            - subss xmm0,xmm1
                        //0xF3, 0x0F, 0x10, 0x15, 0x00, 0x00, 0x00, 0x00,//   - movss xmm2,[engine.dll + 509EB8]
                        //0x0F, 0x2F, 0xD0,// - comiss xmm2, xmm0
                        //0x76, 0x04,// - jna engine.dll + BD492
                        //0xF3, 0x0F, 0x58, 0xC1,// - addss xmm0, xmm1
                        //0xA1, 0x00, 0x00, 0x00, 0x00,// - mov eax,[engine.dll + 5BB2D4] <<<<
                        //0xF3, 0x0F, 0x11, 0x80, 0x00, 0x00, 0x00, 0x00,// - movss[eax + 00004D0C], xmm0
                        //0xD9, 0x46, 0x04// - fld dword ptr[esi + 04]
                       0xF3, 0x0F, 0x11, 0x80, 0x00, 0x00, 0x00, 0x00, 0xD9, 0x46, 0x04, 0xD9, 0x05

                    },
                    "xxxx????xxxxx", Program.Hack.EngineDll);
            if (scan.Success) {
                var value = scan.Stream.Read<int>(4);
                ClientStateSetViewAngles = value;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigClassIDBase()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
                {
                    0x8D, 0x49, 0x08,
                    0xE8, 0x00, 0x00, 0x00, 0x00,
                    0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, //<<!
                    0x83, 0xC1, 0x00, //<<!
                    0x8B, 0x01,
                    0xFF, 0x90, 0x00, 0x00, 0x00, 0x00,
                    0xE8
                }, "xxxx????xx????xx?xxxx????x", Program.Hack.EngineDll, false);
            if (scan.Success)
            {
                int tmp = scan.Stream.Read<int>(10);
                byte tmp2 = scan.Stream.Read<byte>(16);
                ClassIDBase = tmp - Program.Hack.EngineDll.BaseAddress.ToInt32();
                ClassIDBaseOffset = tmp2;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigClassIDManager()
        {
            /*D3 EF 89 44 24 50 89 7C 24 4C 89 54 24 10 8B 44 24 24 8B 88 C4 43 00 00 85 C9*/
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(new byte[]
            {
                    0x2B, 0xC6,
                    0xD3, 0xEF,
                    0x89, 0x00, 0x24, 0x00,
                    0x8B, 0x00, 0x24, 0x00,
                    0x89, 0x00, 0x24, 0x00,
                    0x8B, 0x00, 0x24, 0x00,
                    0x8B, 0x88, 0x00, 0x00, 0x00, 0x00, //<<!
                    0x85, 0xC9
            }, 
            "xx" +
            "xx" +
            "x?x?" +
            "x?x?" +
            "x?x?" +
            "x?x?" +
            "xx????" +
            "xx", Program.Hack.EngineDll, true);
            if (scan.Success)
            {
                int tmp = scan.Stream.Read<int>(22);
                ClassIDManager = tmp;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigEntityID()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(
                    new byte[] { 0x74, 0x72, 0x80, 0x79, 0x00, 0x00, 0x8B, 0x56, 0x00, 0x89, 0x55, 0x00, 0x74, 0x17 },
                    "xxxx??xx?xx?xx", Program.Hack.ClientDll);
            if (scan.Success)
            {
                byte tmp = scan.Stream.Read<byte>(8);
                m_iID = tmp;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
        private void SigCrosshairIdx()
        {
            ScanResult scan = Program.Hack.Memory.PerformSignatureScan(
                    new byte[] {
                    0x57,//                    - push edi
                    0x8B, 0xF9,//                 - mov edi,ecx
                    0xC7, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,// - mov [edi+0000AA54],00000000
                    0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00//        - mov ecx,[client.dll + A4F634]
                    },
                    "xxxxx????????xx????", Program.Hack.ClientDll);
            if (scan.Success)
            {
                var off = scan.Stream.Read<int>(5);
                m_iCrosshairID = off;
            }
            else
            {
                Console.WriteLine("[SIG] Failed: " + new StackTrace().GetFrame(0).GetMethod().Name);
            }
        }
#endif
        #endregion

        #region CLASSES
        public class HexToIntConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(int);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.String)
                {
                    var hex = serializer.Deserialize<string>(reader);
                    if (!string.IsNullOrEmpty(hex))
                    {
                        hex = hex.Replace("0x", "");
                        try
                        {
                            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                        }
                        catch { }
                    }
                }
                return 0;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                int val = (int)value;
                writer.WriteValue("0x" + val.ToString("X4"));
            }
        }
        #endregion
    }
}
