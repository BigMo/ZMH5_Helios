using _ZMH5__Helios.CSGO.Entities.NetVars;
using _ZMH5__Helios.CSGO.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class CSPlayerResource : EntityPrototype
    {
        #region VARIABLES
        public static LazyCache<int> CLASSID = new LazyCache<int>(() => ClassIDs.ClientClassParser.ClientClasses.First(x => x.NetworkName == "CCSPlayerResource").ClassID);
        private static LazyCache<int> memSize = new LazyCache<int>(() => System.Math.Max(LargestDataTable(DT_CSPlayerResource.Instance), Program.Offsets.PlayerResourcesNames + 32 * 64));
        #endregion

        #region PROPERTIES
        public override bool IsValid { get { return base.IsValid && m_ClientClass.ClassID == CLASSID; } }

        public Vector3 m_bombsiteCenterA { get; private set; }
        public Vector3 m_bombsiteCenterB { get; private set; }
        public int[] m_iScore { get; private set; } //TODO: Irgendwie gescheit implementieren... LazyArray wäre Blödsinn, am besten fixed struct.
        public string[] m_sNames { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public CSPlayerResource(IntPtr address, int size) : base(Program.Hack.Memory, address, size) { }
        public CSPlayerResource(IntPtr address) : this(address, memSize) { }
        public CSPlayerResource() : this(memSize) { }
        public CSPlayerResource(int size) : base(size) { }
        #endregion

        #region METHODS
        protected override unsafe void ReadFields(byte* d)
        {
            base.ReadFields(d);

            m_bombsiteCenterA = *(Vector3*)(d + DT_CSPlayerResource.Instance.m_bombsiteCenterA);
            m_bombsiteCenterA = *(Vector3*)(d + DT_CSPlayerResource.Instance.m_bombsiteCenterA);
            m_iScore = new int[65];
            
            for (int i = 0; i < m_iScore.Length; i++)
                m_iScore[i] = *(int*)(d + DT_CSPlayerResource.Instance.m_iScore + SizeCache<int>.Size * i);

            m_sNames = new string[64];
            for(int i=0; i < m_sNames.Length; i++)
            {
                var address = *(IntPtr*)(d + Program.Offsets.PlayerResourcesNames + SizeCache<IntPtr>.Size * i);
                if ((int)address != 0)
                    m_sNames[i] = Program.Hack.Memory.ReadString(address, 32, Encoding.ASCII);
            }
        }
        #endregion
    }
}
