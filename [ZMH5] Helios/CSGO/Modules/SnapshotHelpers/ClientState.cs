using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.Modules.SnapshotHelpers
{
    public class ClientState
    {
        private static int MAX_SIZE = System.Math.Max(Program.Offsets.ClientStateMapDirectory, Program.Offsets.ClientStateSetViewAngles) + 32;

        public enum SignOnState
        {
            None = 0,
            Challange = 1,
            Connected = 2,
            New = 3,
            Prespawn = 4,
            Spawn = 5,
            Full = 6,
            ChangeLevel = 7
        }

        private int address;
        private byte[] data;
        protected MemoryStream stream;

        public LazyCache<Vector3> ViewAngles { get; private set; }
        public LazyCache<string> Map { get; private set; }
        public LazyCache<SignOnState> State { get; private set; }

        public ClientState(int address)
        {
            this.address = address;
            data = new byte[MAX_SIZE];
            Program.Hack.Memory.Position = address;
            Program.Hack.Memory.Read(data, 0, data.Length);
            stream = new MemoryStream(data);

            ViewAngles = new LazyCache<Vector3>(() => ReadAt<Vector3>(Program.Offsets.ClientStateSetViewAngles));
            Map = new LazyCache<string>(() => {
                var str = Encoding.UTF8.GetString(data, Program.Offsets.ClientStateMapDirectory, 256);
                if (str.Contains("\0"))
                    str = str.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries)[0];
                return str;
                });
            State = new LazyCache<SignOnState>(() => ReadAt<SignOnState>(Program.Offsets.ClientStateState));
        }

        protected T ReadAt<T>(long offset) where T : struct
        {
            return stream.Read<T>(offset);
        }
    }
}
