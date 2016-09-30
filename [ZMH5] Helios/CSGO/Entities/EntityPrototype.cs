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
                try
                {
                    if (m_iVMT.Value == 0 || m_iVMT.Value % 4 != 0)
                        return null;

                    uint fn = Program.Hack.Memory.Read<uint>(m_iVMT + 2 * 0x04);
                    if (fn == 0xffffffff || fn == 0 || fn % 4 != 0)
                        return null;

                    int address = (int)Program.Hack.Memory.Read<uint>(fn + 1);

                    if (ClientClassParser.ClientClasses.Any(x => x.Address == address))
                        return ClientClassParser.ClientClasses.First(x => x.Address == address);
                    return null;
                } catch { return null; }
            });
        }
        protected T ReadAt<T>(long offset) where T : struct
        {
            if (!Initialized)
                throw new InvalidOperationException();

            return stream.Read<T>(offset);
        }
        protected static int LargestDataTable(params string[] tables)
        {
            var filtered = ClientClassParser.DataTables.Values.Where(x => tables.Contains(x.NetTableName));
            return filtered.Max(x => x.HighestOffset.Value);
        }
        public void WriteNetVar<T>(string className, string fieldName, T value) where T : struct
        {
            if (!Initialized)
                throw new InvalidOperationException();

            Program.Hack.Memory.Write<T>(Address + ClientClassParser.DataTables[className][fieldName].Offset, value);
        }
        public void WriteAt<T>(int offset, T value) where T : struct
        {
            if (!Initialized)
                throw new InvalidOperationException();

            Program.Hack.Memory.Write<T>(Address + offset, value);
        }
        protected T ReadNetVar<T>(string fieldName) where T : struct
        {
            return ReadNetVar<T>("DT_BaseEntity", fieldName);
        }
        protected T ReadNetVar<T>(string className, string fieldName) where T : struct
        {
            int offset = ClientClassParser.DataTables[className][fieldName].Offset;
            return ReadAt<T>(offset);
        }
        protected T ReadNetVar<T>(string className, string fieldName, string className2, string fieldName2) where T : struct
        {
            int offset1 = ClientClassParser.DataTables[className][fieldName].Offset;
            int offset2 = ClientClassParser.DataTables[className2][fieldName2].Offset;
            return ReadAt<T>(offset1 + offset2);
        }
        #endregion
    }
}
