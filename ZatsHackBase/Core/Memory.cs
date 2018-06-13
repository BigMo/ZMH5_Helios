using ZatsHackBase.Core.SigScanning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    public class Memory : Stream
    {
        #region PROPERTIES
        public long BytesIn { get; private set; }
        public long BytesOut { get; private set; }
        public long RPMCalls { get; private set; }
        public long WPMCalls { get; private set; }
        public EUCProcess Process { get; private set; }
        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return true; } }

        public override bool CanWrite { get { return true; } }

        public override long Length { get { return IntPtr.Size == 4 ? (long)int.MaxValue : long.MaxValue; } }

        public override long Position { get; set; }

        public IntPtr pPosition { get { return (IntPtr)Position; } set { Position = (long)value; } }
        #endregion

        #region CONSTRUCTORS
        public Memory(EUCProcess process)
        {
            Process = process;
            BytesIn = 0;
            BytesOut = 0;
            Position = 0;
            RPMCalls = 0;
            WPMCalls = 0;
        }
        #endregion

        #region METHODS
        public ScanResult PerformSignatureScan(byte[] pattern, string mask, ProcessModule module, bool codeOnly = true)
        {
            MemoryModule mmod = MemoryModule.FromMemory(this, module);

            byte[] buffer = new byte[4096];
            int idx = 0;

            bool found = false;
            long start = mmod.ImageBase;// codeOnly ? mmod.BaseOfCode : mmod.ImageBase;
            long size = codeOnly ? mmod.SizeOfCode : mmod.MemorySize;

            while (true)
            {
                this.Position = start + idx;
                this.Position -= this.Position % 4;

                    int length = this.Read(buffer, 0, (int)Math.Min(buffer.Length, size - idx));

                    for (int b = 0; b < buffer.Length - mask.Length; b++)
                    {
                        found = true;
                        for (int i = 0; i < mask.Length; i++)
                        {
                            if (mask[i] != '?' && buffer[b + i] != pattern[i])
                            {
                                found = false;
                                break;
                            }
                        }
                        if (found)
                        {
                            byte[] data = new byte[mask.Length];
                            Array.Copy(buffer, b, data, 0, mask.Length);
                            return ScanResult.Succeeded(start + idx + b, data);
                        }
                    }

                if (length - mask.Length == 0)
                    break;
                idx += length - mask.Length;
                if (this.Position >= start + size)
                    break;
            }
            return ScanResult.Failed();
        }



        #region OVERRIDDEN
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch(origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length - offset;
                    break;
            }

            return Position;
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Flush() { }

        public int Read(IntPtr address, byte[] buffer, int offset, int count)
        {
            pPosition = address;
            return Read(buffer, offset, count);
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            IntPtr numBytes = IntPtr.Zero;
            try
            {
              RPMCalls++;
              bool succ = WinAPI.ReadProcessMemory(Process.Handle, pPosition, buffer, count, out numBytes) && numBytes.ToInt32() == count;
              BytesIn += numBytes.ToInt32();
            }
            catch
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return count;
        }

        public void Write(IntPtr address, byte[] buffer, int offset, int count)
        {
            pPosition = address;
            Write(buffer, offset, count);

        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            IntPtr numBytes = IntPtr.Zero;
            WPMCalls++;
            WinAPI.WriteProcessMemory(Process.Handle, pPosition, buffer, count, out numBytes);
            BytesOut += numBytes.ToInt32();
            if (numBytes.ToInt32() != count)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        #endregion
        #endregion
    }
}
