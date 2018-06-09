using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    public static class SizeCache<T>
    {
        //private static SizeCache<T> instance;
        //public static SizeCache<T> Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance = new SizeCache<T>();
        //        return instance;
        //    }
        //}

        public static int Size { get; private set; }

        static SizeCache()
        {
            Size = Marshal.SizeOf(typeof(T));
        }
    }
}
