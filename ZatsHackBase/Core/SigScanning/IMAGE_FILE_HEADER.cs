using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core.SigScanning
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct IMAGE_FILE_HEADER
    {
        public ushort Machine;
        public ushort NumberOfSections;
        public int TimeDateStamp;
        public int PointerToSymbolTable;
        public int NumberOfSymbols;
        public ushort SizeOfOptionalHeader;
        public FileCharacteristics Characteristics;
    }

    public enum FileCharacteristics : ushort
    {
        IMAGE_FILE_RELOCS_STRIPPED = 0x0001,
        IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002,
        IMAGE_FILE_LINE_NUMS_STRIPPED = 0x0004,
        IMAGE_FILE_LOCAL_SYMS_STRIPPED = 0x0008,
        IMAGE_FILE_AGGRESIVE_WS_TRIM = 0x0010,
        IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020,
        IMAGE_FILE_BYTES_REVERSED_LO = 0x0080,
        IMAGE_FILE_32BIT_MACHINE = 0x0100,
        IMAGE_FILE_DEBUG_STRIPPED = 0x0200,
        IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = 0x0400,
        IMAGE_FILE_NET_RUN_FROM_SWAP = 0x0800,
        IMAGE_FILE_SYSTEM = 0x1000,
        IMAGE_FILE_DLL = 0x2000,
        IMAGE_FILE_UP_SYSTEM_ONLY = 0x4000,
        IMAGE_FILE_BYTES_REVERSED_HI = 0x8000
    }
}
