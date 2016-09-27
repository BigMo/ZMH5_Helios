using _ZMH5__Helios.CSGO.ClassIDs;
using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class EntityPrototype
    {
        #region VARIABLES
        protected MemoryStream stream;
        #endregion

        #region PROPERTIES
        public long Address { get; private set; }
        public byte[] Data { get; protected set; }
        public LazyCache<int> m_iVMT { get; private set; }
        public LazyCache<ManagedClientClass> m_ClientClass { get; private set; }
        public virtual bool IsValid { get { return m_ClientClass != null && m_ClientClass.Value != null; } }

        public virtual int MemSize { get { return 0x0C; } }
        public bool Initialized { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public EntityPrototype()
        { }
        #endregion

        #region METHODS
        public void Init(long address, int size)
        {
            Address = address;
            Data = new byte[size + 64];
            Program.Hack.Memory.Position = address;
            Program.Hack.Memory.Read(Data, 0, Data.Length);
            stream = new MemoryStream(Data);

            SetupFields();

            Initialized = true;
        }
        protected virtual void SetupFields()
        {
            m_iVMT = new LazyCache<int>(() => ReadAt<int>(0x08));

            m_ClientClass = new LazyCache<ManagedClientClass>(() =>
            {
                if (m_iVMT.Value == 0 || m_iVMT.Value < 0x01000000 || m_iVMT.Value % 4 != 0)
                    return null;

                uint fn = Program.Hack.Memory.Read<uint>(m_iVMT + 2 * 0x04);
                if (fn == 0xffffffff || fn == 0 || fn < 0x01000000 || fn % 4 != 0)
                    return null;

                int address = (int)Program.Hack.Memory.Read<uint>(fn + 1);

                if (ClientClassParser.ClientClasses.Any(x => x.Address == address))
                    return ClientClassParser.ClientClasses.First(x => x.Address == address);
                return null; 
            });
        }
        protected T ReadAt<T>(long offset) where T : struct
        {
            if (!Initialized)
                throw new InvalidOperationException();

            return stream.Read<T>(offset);
        }
        #endregion
    }
}
