using ZatsHackBase.Core;
using ZatsHackBase.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _ZMH5__Helios.CSGO.Entities.NetVars;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class CSLocalPlayer : CSPlayer
    {
        #region VARIABLES
        private static LazyCache<int> memSize = new LazyCache<int>(() => System.Math.Max(LargestDataTable(DT_Local.Instance, DT_LocalPlayerExclusive.Instance, DT_CSLocalPlayerExclusive.Instance), Program.Offsets.m_iCrosshairID + 4));
        #endregion

        #region PROPERTIES
        public int m_iCrosshairIdx { get; private set; }
        public Vector3 m_aimPunchAngle { get; private set; }
        public Vector3 m_viewPunchAngle { get; private set; }
        public Vector3 m_vecViewOffset { get; private set; }
        public Vector3 m_vecVelocity { get; private set; }
        public int m_iShotsFired { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public CSLocalPlayer(IntPtr address, int size) : base(address, size) { }
        public CSLocalPlayer(IntPtr address) : this(address, memSize) { }
        public CSLocalPlayer() : this(memSize) { }
        public CSLocalPlayer(int size) : base(System.Math.Max(size, memSize)) { }
        #endregion

        #region METHODS
        public static bool IsProcessable(CSLocalPlayer lp)
        {
            return !(lp == null || !lp.IsValid);
        }

        protected override unsafe void ReadFields(byte* d)
        {
            base.ReadFields(d);

            m_iCrosshairIdx = *(int*)(d + Program.Offsets.m_iCrosshairID);
            m_aimPunchAngle = *(Vector3*)(d + DT_LocalPlayerExclusive.Instance.m_Local + DT_Local.Instance.m_aimPunchAngle);
            m_viewPunchAngle = *(Vector3*)(d + DT_LocalPlayerExclusive.Instance.m_Local + DT_Local.Instance.m_viewPunchAngle);
            m_vecViewOffset = *(Vector3*)(d + DT_LocalPlayerExclusive.Instance.m_vecViewOffset);
            m_vecVelocity = *(Vector3*)(d + DT_LocalPlayerExclusive.Instance.m_vecVelocity);
            m_iShotsFired = *(int*)(d + DT_CSLocalPlayerExclusive.Instance.m_iShotsFired);
        }

        public bool CanSee(CSPlayer player, int boneIndex = 6)
        {
            return false;
        }
        #endregion
    }
}
