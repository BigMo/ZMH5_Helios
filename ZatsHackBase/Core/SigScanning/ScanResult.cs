using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core.SigScanning
{
    public class ScanResult
    {
        #region PROPERTIES
        public bool Success { get; private set; }
        public long Address { get; private set; }
        public byte[] Data { get; private set; }
        public Stream Stream { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public static ScanResult Failed()
        {
            return new ScanResult(false, 0, null);
        }
        public static ScanResult Succeeded(long address, byte[] data)
        {
            return new ScanResult(true, address, data);
        }

        public ScanResult(bool success, long address, byte[] data)
        {
            Success = success;
            Address = address;
            Data = data;
            if (data != null)
                Stream = new MemoryStream(Data);
        }
        #endregion
    }
}
