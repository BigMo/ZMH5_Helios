using _ZMH5__Helios.CSGO.ClassIDs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities.EntityHelpers
{
    public class VMTtoClassId
    {
        private Dictionary<IntPtr, ManagedClientClass> dict;

        private static VMTtoClassId instance;
        public static VMTtoClassId Instance
        {
            get
            {
                if (instance == null)
                    instance = new VMTtoClassId();
                return instance;
            }
        }

        public ManagedClientClass this[IntPtr address]
        {
            get
            {
                return dict[address];
            }
            set
            {
                dict[address] = value;
            }
        }

        private VMTtoClassId()
        {
            dict = new Dictionary<IntPtr, ManagedClientClass>();
        }

        public bool ContainsVMT(IntPtr address)
        {
            return dict.ContainsKey(address);
        }
    }
}
