using _ZMH5__Helios.CSGO.Enums;
using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _ZMH5__Helios.CSGO.Entities.NetVars;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class CSPlayer : BasePlayer
    {
        #region VARIABLES
        public static LazyCache<int> CLASSID = new LazyCache<int>(() => ClassIDs.ClientClassParser.ClientClasses.First(x => x.NetworkName == "CCSPlayer").ClassID);
        private static LazyCache<int> memSize = new LazyCache<int>(() => LargestDataTable(DT_CSPlayer.Instance));
        #endregion

        #region PROPERTIES
        public MoveState m_iMoveState { get; private set; }
        public PlayerState m_iPlayerState { get; private set; }
        public int m_bIsScoped { get; private set; }
        public int m_ArmorValue { get; private set; }
        public override bool IsValid { get { return base.IsValid && CLASSID == this.m_ClientClass.ClassID; } }
        #endregion

        #region CONSTRUCTORS
        public CSPlayer(IntPtr address, int size) : base(address, size) { }
        public CSPlayer(IntPtr address) : this(address, memSize) { }
        public CSPlayer() : this(memSize) { }
        public CSPlayer(int size) : base(System.Math.Max(size, memSize)) { }
        #endregion

        #region METHODS
        public static bool IsPlayer(EntityPrototype other)
        {
            if (other == null || !other.IsValid)
                return false;

            return other.m_ClientClass.ClassID == CLASSID.Value;
        }

        protected override unsafe void ReadFields(byte* d)
        {
            base.ReadFields(d);

            m_iMoveState = *(MoveState*)(d + DT_CSPlayer.Instance.m_iMoveState);
            m_iPlayerState = *(PlayerState*)(d + DT_CSPlayer.Instance.m_iPlayerState);
            m_bIsScoped = *(int*)(d + DT_CSPlayer.Instance.m_bIsScoped);
            m_ArmorValue = *(int*)(d + DT_CSPlayer.Instance.m_ArmorValue);
        }
        #endregion
    }
}
