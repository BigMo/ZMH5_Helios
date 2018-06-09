using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities.NetVars
{
    public class NetVarAttribute : Attribute
    {
        public string Name { get; private set; }
        public int Size { get; private set; }

        public NetVarAttribute(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}
