using _ZMH5__Helios.CSGO.ClassIDs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities.NetVars
{
    public abstract class NetVarResolver
    {
        public bool Resolved { get; private set; }
        public int NumBytes { get; private set; }

        protected NetVarResolver() { }


        public void Resolve()
        {
            if (Resolved)
                return;

            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x=>x.Name.StartsWith("m_")).ToArray();
            var typeName = GetType().Name;

            if(!ClientClassParser.DataTables.Any(x => x.Key == typeName))
                throw new Exception($"Unknown data table \"{typeName}\"");

            Program.Logger.Log("[NVR] Resolving data table \"{0}\"", typeName);

            var dataTable = ClientClassParser.DataTables.FirstOrDefault(x => x.Key == typeName).Value;
                
            foreach (var prop in props)
            {
                var propName = prop.Name;
                var attribute = (NetVarAttribute)prop.GetCustomAttributes(true).FirstOrDefault(x => x is NetVarAttribute);
                if (attribute != null)
                    propName = attribute.Name;

                Program.Logger.Log("[NVR] -> Property \"{0}\"", propName);

                var recvProp = dataTable.RecvProps.Value.FirstOrDefault(x => x.VarName == propName);
                if (recvProp == null)
                    throw new Exception($"Unknown property \"{propName}\"");

                prop.SetValue(this, recvProp.Offset);

                if (recvProp.Offset + recvProp.Size > NumBytes)
                    NumBytes = recvProp.Offset + (attribute != null ? attribute.Size : recvProp.Size);
            }
            Resolved = true;
        }
    }
}
