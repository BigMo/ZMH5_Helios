using ZatsHackBase.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.ClassIDs
{
    public class ManagedClientClass
    {
        #region PROPERTIES
        [JsonIgnore]
        public int Address { get; private set; }
        [JsonIgnore]
        public LazyCache<ClientClass> ClientClass { get; private set; }
        public LazyCache<string> NetworkName { get; private set; }
        public ManagedRecvTable RecvTable { get; private set; }
        public int ClassID { get { return ClientClass.Value.m_ClassID; } }
        #endregion

        #region CONSTRUCTORS
        public ManagedClientClass(int address)
        {
            Address = address;

            ClientClass = new LazyCache<ClientClass>(() => Program.Hack.Memory.Read<ClientClass>(address));
            var c = ClientClass;

            NetworkName = new LazyCache<string>(() => Program.Hack.Memory.ReadString(c.Value.m_pNetworkName, 32, Encoding.ASCII));
            if (c.Value.m_pRecvTable == 0xffff || c.Value.m_pRecvTable == -1 || c.Value.m_pRecvTable == 0)
                RecvTable = null;
            else
                RecvTable = new ManagedRecvTable(c.Value.m_pRecvTable);
        }
        #endregion

        #region METHODS
        
        #endregion
    }
}
