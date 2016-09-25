using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using Newtonsoft.Json;

namespace _ZMH5__Helios.CSGO.ClassIDs
{
    public static class ClientClassParser
    {
        public static ManagedClientClass[] ClientClasses { get; private set; }
        public static Dictionary<string, ManagedRecvTable> DataTables { get; private set; }
        public static void Parse()
        {
            DataTables = new Dictionary<string, ManagedRecvTable>();
            int dwClassIDBase = Program.Hack.Memory.Read<int>(Program.Hack.EngineDll.BaseAddress.ToInt32() + Program.Offsets.ClassIDBase);
            dwClassIDBase += Program.Offsets.ClassIDBaseOffset;
            int dwClassIDManager = dwClassIDBase + Program.Offsets.ClassIDManager;

            ClientClassManager pManager = Program.Hack.Memory.Read<ClientClassManager>(dwClassIDManager);

            byte[] pData = new byte[pManager.m_iCount * 0x10];
            if (pManager.m_pCIDArray == 0)
                return;
            Program.Hack.Memory.Position = pManager.m_pCIDArray;
            Program.Hack.Memory.Read(pData, 0, pData.Length);

            List<ManagedClientClass> classes = new List<ManagedClientClass>();
            for (int i = 0; i < pManager.m_iCount; i++)
            {
                int dwAddress = BitConverter.ToInt32(pData, i * 0x10);
                classes.Add(new ManagedClientClass(dwAddress));
            }

            ClientClasses = classes.OrderBy(x => x.ClassID).ToArray();
            foreach (var cls in ClientClasses)
                CrawlDataTables(cls.RecvTable);
        }
        private static void CrawlDataTables(ManagedRecvTable table)
        {
            if (table == null)
                return;
            if (!DataTables.ContainsKey(table.NetTableName))
                DataTables[table.NetTableName] = table;

            foreach (var prop in table.RecvProps.Value)
                if (prop.SubTable.Value != null)
                    CrawlDataTables(prop.SubTable.Value);
        }
        public static void DumpClassIDs()
        {
            using (StreamWriter writer = new StreamWriter("classids.txt"))
            {
                writer.WriteLine("public enum ClassID : uint");
                writer.WriteLine("{");
                foreach (ManagedClientClass id in ClientClasses)
                {
                    writer.WriteLine("\t{0} = {1},", id.NetworkName.Value, id.ClassID.ToString());
                }
                writer.WriteLine("}");
            }
        }
        public static void DumpNetVars(bool full, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine("Netvars");
                foreach (var id in DataTables.OrderBy(x=>x.Key))
                {
                    DumpTable(id.Value, writer, full);
                    writer.WriteLine();
                }
            }
        }
        public static void DumpNetVarsJson(string file)
        {
            JsonSerializer s = new JsonSerializer();
            s.Formatting = Formatting.Indented;

            using (StreamWriter writer = new StreamWriter(file))
            {
                s.Serialize(writer, ClientClasses);
            }
        }
        private static void DumpTable(ManagedRecvTable table, StreamWriter writer, bool full = false, string depth = "")
        {
            if (table == null)
                return;
            writer.WriteLine("{0}{1}:", depth, table.NetTableName.Value);

            foreach (var prop in table.RecvProps.Value.OrderBy(x=>x.Offset))
            {
                writer.WriteLine("{0} {1}",
                    string.Format("{0}{1} ({2}) -> ",
                        depth + " ",
                        prop.VarName.Value,
                        Enum.GetName(typeof(RecvProp_t.ePropType), prop.Type)
                    ).PadRight(60, '_'),
                    prop.Offset.ToString("X8"));
                if (prop.SubTable.Value != null)
                    if (!full)
                        writer.WriteLine("{0}->{1}",
                            depth + " ",
                            prop.SubTable.Value.NetTableName.Value);
                    else
                        DumpTable(prop.SubTable.Value, writer, full, depth + "  ");
            }
            writer.WriteLine("{0}//{1}", depth, table.NetTableName.Value);
        }
    }
}
