using ZatsHackBase.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.ClassIDs
{
    public class ManagedRecvProp
    {
        #region PROPERTIES
        [JsonIgnore]
        public int Address { get; private set; }
        [JsonIgnore]
        public LazyCache<RecvProp_t> RecvProp { get; private set; }
        public LazyCache<string> VarName { get; private set; }
        public int Offset { get { return RecvProp.Value.m_Offset; } }
        [JsonConverter(typeof(StringEnumConverter))]
        public RecvProp_t.ePropType Type { get { return (RecvProp_t.ePropType)RecvProp.Value.m_RecvType; } }
        [JsonIgnore]
        public LazyCache<ManagedRecvTable> SubTable { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public ManagedRecvProp(int address)
        {
            Address = address;

            RecvProp = new LazyCache<RecvProp_t>(() => Program.Hack.Memory.Read<RecvProp_t>(address));
            var r = RecvProp;
            VarName = new LazyCache<string>(() => Program.Hack.Memory.ReadString(r.Value.m_pVarName, 32, Encoding.ASCII));
            SubTable = new LazyCache<ManagedRecvTable>(() =>
            {
                if (r.Value.m_pDataTable == 0)
                    return null;

                return new ManagedRecvTable(r.Value.m_pDataTable);
            });
        }
        #endregion

        #region METHODS
        public override string ToString()
        {
            return string.Format("{0} ({1}): {2}", VarName.Value, Enum.GetName(typeof(RecvProp_t.ePropType), Type), Offset.ToString("X8"));
        }
        #endregion
    }
}
