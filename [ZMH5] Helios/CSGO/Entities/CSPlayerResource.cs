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
    class CSPlayerResource : EntityPrototype
    {
        #region VARIABLES
        public static LazyCache<int> CLASSID = new LazyCache<int>(() => ClassIDs.ClientClassParser.ClientClasses.First(x => x.NetworkName == "CCSPlayerResource").ClassID);
        private static LazyCache<int> memSize = new LazyCache<int>(() => LargestDataTable("DT_CSPlayerResource", "DT_PlayerResource"));
        #endregion

        #region PROPERTIES
        public override int MemSize { get { return memSize.Value; } }
        public override bool IsValid { get { return base.IsValid && m_ClientClass.Value.ClassID == CLASSID; } }

        public LazyCache<Vector3> m_bombsiteCenterA { get; private set; }
        public LazyCache<Vector3> m_bombsiteCenterB { get; private set; }
        public LazyCache<byte[]> m_iScore { get; private set; } //TODO: Irgendwie gescheit implementieren... LazyArray wäre Blödsinn, am besten fixed struct.

        #endregion

        #region CONSTRUCTORS
        public CSPlayerResource()
        { }
        #endregion

        #region METHODS
        protected override void SetupFields()
        {
            base.SetupFields();

            m_bombsiteCenterA = new LazyCache<Vector3>(() => ReadNetVar<Vector3>("DT_CSPlayerResource", "m_bombsiteCenterA"));
            m_bombsiteCenterB = new LazyCache<Vector3>(() => ReadNetVar<Vector3>("DT_CSPlayerResource", "m_bombsiteCenterB"));
            m_iScore = new LazyCache<byte[]>(() => new byte[65 * sizeof(int)]);
        }
        #endregion
    }
}
