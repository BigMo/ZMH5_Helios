using ZatsHackBase.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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
        public int StringBufferSize { get { return RecvProp.Value.m_StringBufferSize; } }
        [JsonConverter(typeof(StringEnumConverter))]
        public RecvProp_t.ePropType Type { get { return RecvProp.Value.m_RecvType; } }
        [JsonIgnore]
        public LazyCache<ManagedRecvTable> SubTable { get; private set; }
        public LazyCache<int> BaseClassDepth { get; private set; }
        public LazyCache<int> Size { get; private set; }
        [JsonIgnore]
        public ManagedRecvTable Table { get; private set; }
        [JsonIgnore]
        public LazyCache<ManagedRecvProp[]> ArrayProp { get; private set; }
        public int ElementCount { get { return RecvProp.Value.m_nElements; } }
        #endregion

        #region CONSTRUCTORS
        public ManagedRecvProp(int address, ManagedRecvTable table)
        {
            Address = address;
            Table = table;

            RecvProp = new LazyCache<RecvProp_t>(() => Program.Hack.Memory.Read<RecvProp_t>(address));
            var r = RecvProp;
            VarName = new LazyCache<string>(() => Program.Hack.Memory.ReadString(r.Value.m_pVarName, 32, Encoding.ASCII));
            SubTable = new LazyCache<ManagedRecvTable>(() =>
            {
                if (r.Value.m_pDataTable == 0)
                    return null;

                return new ManagedRecvTable(r.Value.m_pDataTable);
            });
            BaseClassDepth = new LazyCache<int>(() => SubTable.Value != null ? 1 + SubTable.Value.BaseClassDepth : 0);
            Size = new LazyCache<int>(() =>
            {
                if (SubTable.Value != null)
                    return SubTable.Value.Size;

                int size = RecvProp_t.GetPropTypeSize(Type);
                if (size >= 0)
                    return size;

                switch (Type)
                {
                    case RecvProp_t.ePropType.String:
                        return StringBufferSize;
                    case RecvProp_t.ePropType.Array:
                        if (ArrayProp.Value.Length > 0)
                            return ElementCount * ArrayProp.Value[0].Size;
                        return 0;
                }

                return 0;
            });
            ArrayProp = new LazyCache<ManagedRecvProp[]>(() =>
            {
                if (this.Type != RecvProp_t.ePropType.Array || this.RecvProp.Value.m_pArrayProp == 0 || this.ElementCount == 0)
                    return new ManagedRecvProp[0];

                var props = new ManagedRecvProp[this.ElementCount];
                for (int i = 0; i < props.Length; i++)
                {
                    int a = this.RecvProp.Value.m_pArrayProp + SizeCache<RecvProp_t>.Size * i;
                    if (address == a)
                        props[i] = this;
                    else
                        props[i] = new ManagedRecvProp(a, table);
                }
                return props;
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
