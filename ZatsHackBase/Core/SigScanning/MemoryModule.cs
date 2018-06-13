using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core.SigScanning
{
    public class MemoryModule
    {
        #region PROPERTIES
        public long ImageBase { get; private set; }
        public long MemorySize { get; private set; }
        public long BaseOfCode { get; private set; }
        public long SizeOfCode { get; private set; }
        public bool Is64Bit { get { return !Is32Bit; } }
        public bool Is32Bit { get; private set; }
        #endregion

        #region CONSTRUCTORS
        private MemoryModule(long imageBase, long memSize, long baseOfCode, long sizeOfCode, bool is32Bit)
        {
            ImageBase = imageBase;
            MemorySize = memSize;
            BaseOfCode = baseOfCode;
            SizeOfCode = sizeOfCode;
            Is32Bit = is32Bit;
        }
        public static MemoryModule FromMemory(Memory mem, ProcessModule module)
        {
            var dos = mem.Read<IMAGE_DOS_HEADER>(module.BaseAddress.ToInt64());
            mem.Position = module.BaseAddress.ToInt64() + dos.e_lfanew + 4;
            var file = mem.Read<IMAGE_FILE_HEADER>();
            bool is32bit = file.Characteristics.HasFlag(FileCharacteristics.IMAGE_FILE_32BIT_MACHINE);

            long offset = module.BaseAddress.ToInt64() + dos.e_lfanew + 4 + SizeCache<IMAGE_FILE_HEADER>.Size;

            if (is32bit)
            {
                var opt = mem.Read<IMAGE_OPTIONAL_HEADER32>(offset);
                return new MemoryModule(
                    module.BaseAddress.ToInt64(), module.ModuleMemorySize,
                    module.BaseAddress.ToInt64() + offset + opt.BaseOfCode, opt.SizeOfCode,
                    is32bit);
            }
            else
            {
                var opt = mem.Read<IMAGE_OPTIONAL_HEADER64>(offset);
                return new MemoryModule(
                    module.BaseAddress.ToInt64(), module.ModuleMemorySize, 
                    module.BaseAddress.ToInt64() + offset + opt.BaseOfCode, opt.SizeOfCode, 
                    is32bit);
            }
        }
        #endregion
    }
}
