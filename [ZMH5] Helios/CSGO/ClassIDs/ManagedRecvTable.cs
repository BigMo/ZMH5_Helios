using ZatsHackBase.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.ClassIDs
{
    public class ManagedRecvTable
    {
        #region PROPERTIES
        [JsonIgnore]
        public int Address { get; private set; }
        [JsonIgnore]
        public LazyCache<RecvTable_t> RecvTable { get; private set; }
        public LazyCache<string> NetTableName { get; private set; }
        public LazyCache<ManagedRecvProp[]> RecvProps { get; private set; }
        [JsonIgnore]
        public LazyCache<int> HighestOffset { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public ManagedRecvTable(int address)
        {
            Address = address;

            RecvTable = new LazyCache<RecvTable_t>(() => Program.Hack.Memory.Read<RecvTable_t>(address));
            var r = RecvTable;

            NetTableName = new LazyCache<string>(() => Program.Hack.Memory.ReadString(r.Value.m_pNetTableName, 32, Encoding.ASCII));
            RecvProps = new LazyCache<ManagedRecvProp[]>(() => {
                ManagedRecvProp[] props = new ManagedRecvProp[r.Value.m_nProps];
                for (int i = 0; i < props.Length; i++)
                    props[i] = new ManagedRecvProp(r.Value.m_pProps + i * 0x3C);

                return props;
            });
            HighestOffset = new LazyCache<int>(() =>
            {
                return RecvProps.Value.Max(x => x == null ? 0 : x.Offset);// GetHighestOffset(this);
            });
        }
        #endregion

        #region METHODS
        private int GetHighestOffset(ManagedRecvTable table)
        {
            if (table == null)
                return 0;

            int highest = table.RecvProps.Value.Max(x => x == null ? 0 : x.Offset);
            if (table.RecvProps.Value.Any(x => x.SubTable != null)) {
                int subHighest = table.RecvProps.Value.Where(x => x != null && x.SubTable != null).Max(x => GetHighestOffset(x.SubTable));
                highest = Math.Max(highest, subHighest);
            }
            return highest;
        }
        public ManagedRecvProp GetProperty(string propertyName)
        {
            return RecvProps.Value.First(x => x.VarName == propertyName);
        }
        public ManagedRecvProp this[string propertyName]
        {
            get
            {
                return RecvProps.Value.First(x => x.VarName == propertyName);
            }
        }
        public override string ToString()
        {
            return string.Format("{0}: {1} offsets, size: {2}bytes", NetTableName.Value, RecvProps.Value.Length, HighestOffset.Value);
        }
        #endregion
    }
}
