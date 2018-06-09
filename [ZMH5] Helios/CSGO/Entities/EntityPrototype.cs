using _ZMH5__Helios.CSGO.ClassIDs;
using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _ZMH5__Helios.CSGO.Entities.EntityHelpers;
using _ZMH5__Helios.CSGO.Entities.NetVars;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class EntityPrototype : RemoteObject
    {
        public ManagedClientClass m_ClientClass { get; private set; }
        public virtual bool IsValid { get { return m_ClientClass != null; } }

        protected EntityPrototype(Memory mem, IntPtr address, int size) : base(mem, address, size) { }
        public EntityPrototype(Memory mem, IntPtr address) : base(mem, address, 16) { }
        public EntityPrototype() : this(16) { }
        public EntityPrototype(int size) : base(System.Math.Max(size, 16)) { }

        private static unsafe ManagedClientClass ParseClientClass(Memory mem, byte* data)
        {
            IntPtr vmtAddress = *(IntPtr*)(data + 0x8);
            if (VMTtoClassId.Instance.ContainsVMT(vmtAddress))
                return VMTtoClassId.Instance[vmtAddress];
            else
            {
                IntPtr pFn = Program.Hack.Memory.Read<IntPtr>(vmtAddress + 2 * 0x04);
                if ((long)pFn == 0xffffffffL || (long)pFn == 0L)
                    return null;

                IntPtr address = Program.Hack.Memory.Read<IntPtr>(pFn + 1);
                var clientClass = ClientClassParser.ClientClasses.FirstOrDefault(x => x.Address == address);

                if (clientClass != null)
                    VMTtoClassId.Instance[vmtAddress] = clientClass;

                return clientClass;
            }
        }

        protected override unsafe void ReadFields(byte* data)
        {
            m_ClientClass = ParseClientClass(Program.Hack.Memory, data);
        }

        protected static int LargestDataTable(params NetVarResolver[] tables)
        {
            return tables.Max(x => x.NumBytes);
        }

        protected int GetOffset(string fieldName)
        {
            return GetOffset("DT_BaseEntity", fieldName);
        }
        protected int GetOffset(string className, string fieldName)
        {
            return ClientClassParser.DataTables[className][fieldName].Offset;
        }
        protected int GetOffset(string className, string fieldName, string className2, string fieldName2)
        {
            int offset1 = ClientClassParser.DataTables[className][fieldName].Offset;
            int offset2 = ClientClassParser.DataTables[className2][fieldName2].Offset;
            return offset1 + offset2;
        }
    }

    /*#region VARIABLES
    protected MemoryStream stream;
    #endregion

    #region PROPERTIES
    public int Address { get; private set; }
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

    ~EntityPrototype()
    {
        if (stream != null)
            stream.Dispose();
    }
    #endregion

    #region METHODS
    public void Init(int address, int size)
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

                if (ClientClassParser.ClientClasses.Any(x => (int)x.Address == address))
                    return ClientClassParser.ClientClasses.First(x => (int)x.Address == address);
                return null;
            }
            catch { return null; }
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
        var ordered = ClientClassParser.DataTables.Values.OrderBy(x => x.NetTableName.Value).ToArray();
        var filtered = ordered.Where(x => tables.Contains(x.NetTableName.Value));
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
}*/
}
