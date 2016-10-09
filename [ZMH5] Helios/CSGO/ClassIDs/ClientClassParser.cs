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
        public static void DumpCppClasses(string file)
        {
            var tables = DataTables.Values.OrderBy(x => x.BaseClassDepth.Value);
            using (StreamWriter writer = new StreamWriter(file, false))
                foreach (var table in tables)
                    DumpCppClass(writer, table);
        }
        private static void DumpCppClass(TextWriter o, ManagedRecvTable table)
        {
            const int padding = 32;

            var bc = table.BaseClass.Value;
            int currentOffset = 0;
            int pad = 0;

            if (bc != null)
                currentOffset = bc.Size;

            var props = table.RecvProps.Value.OrderBy(x => x.Offset).ToArray();

            if (bc != null)
                o.WriteLine("class {0} : {1} {{", table.NetTableName.Value, bc.NetTableName.Value);
            else
                o.WriteLine("class {0} {{", table.NetTableName.Value);

            for(int i = 0; i < props.Length;i++)
            {
                var prop = props[i];
                if (prop.VarName.Value == "baseclass")
                    continue;

                //Pad
                if (prop.Offset != currentOffset)
                {
                    o.WriteLine("\t{0} {1}//0x{2}",
                        "char".PadRight(padding, ' '),
                        string.Format("pad{0}[{1}];",
                            (pad++).ToString().PadLeft(2, '0'), 
                            prop.Offset - currentOffset
                        ).PadRight(padding, ' '),
                        currentOffset.ToString("X8"));
                    currentOffset = prop.Offset;
                }

                switch (prop.Type)
                {
                    case RecvProp_t.ePropType.DataTable:
                        o.WriteLine("\t{0} {1}//0x{2}",
                            prop.SubTable.Value.NetTableName.Value.PadRight(padding, ' '),
                            (prop.VarName.Value + ";").PadRight(padding, ' '),
                            prop.Offset.ToString("X8"));
                        break;
                    case RecvProp_t.ePropType.Array:
                        o.WriteLine("\t{0} {1}//0x{2}", 
                            (prop.ArrayProp.Value.Length > 0 ? prop.ArrayProp.Value[0].Type.ToString() : "void*").PadRight(padding, ' '), 
                            string.Format("{0}[{1}];", 
                                prop.VarName.Value, 
                                prop.ElementCount).PadRight(padding, ' '),
                            prop.Offset.ToString("X8"));
                        break;
                    default:
                        o.WriteLine("\t{0} {1}//0x{2}", 
                            prop.Type.ToString().PadRight(padding, ' '), 
                            (prop.VarName.Value + ";").PadRight(padding, ' '), 
                            prop.Offset.ToString("X8"));
                        break;
                }
                if (i < props.Length - 1)
                    currentOffset += Math.Min(prop.Size, props[i + 1].Offset - currentOffset);
                else
                    currentOffset += prop.Size;
            }

            o.WriteLine("}\n");
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
