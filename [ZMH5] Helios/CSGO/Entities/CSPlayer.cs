using _ZMH5__Helios.CSGO.Enums;
using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class CSPlayer : BasePlayer
    {
        #region VARIABLES
        public static LazyCache<int> CLASSID = new LazyCache<int>(() => ClassIDs.ClientClassParser.ClientClasses.First(x => x.NetworkName == "CCSPlayer").ClassID);
        private static LazyCache<int> memSize = new LazyCache<int>(() => LargestDataTable("DT_CSPlayer"));
        #endregion

        #region PROPERTIES
        public LazyCache<MoveState> m_iMoveState { get; private set; }
        public LazyCache<PlayerState> m_iPlayerState { get; private set; }
        public LazyCache<int> m_bIsScoped { get; private set; }
        public LazyCache<int> m_ArmorValue { get; private set; }
        public override bool IsValid { get { return base.IsValid && CLASSID == this.m_ClientClass.Value.ClassID; } }
        public override int MemSize { get { return memSize.Value; } }
        #endregion

        #region CONSTRUCTORS
        public CSPlayer() : base() { }
        public CSPlayer(BaseEntity other) : base(other, memSize.Value) { }

        public CSPlayer(int address) : this(address, memSize.Value) { }
        public CSPlayer(int address, int size) : base(address, size) { }
        #endregion

        #region METHODS
        public static bool IsPlayer(EntityPrototype other)
        {
            if (other == null || !other.IsValid)
                return false;

            return other.m_ClientClass.Value.ClassID == CLASSID.Value;
        }
        protected override void SetupFields()
        {
            base.SetupFields();
            
            m_iMoveState = new LazyCache<MoveState>(() => (MoveState)this.ReadNetVar<int>("DT_CSPlayer", "m_iMoveState"));
            m_iPlayerState = new LazyCache<PlayerState>(() => (PlayerState)this.ReadNetVar<int>("DT_CSPlayer", "m_iPlayerState"));
            m_bIsScoped = new LazyCache<int>(() => this.ReadNetVar<int>("DT_CSPlayer", "m_bIsScoped"));
            m_ArmorValue = new LazyCache<int>(() => this.ReadNetVar<int>("DT_CSPlayer", "m_ArmorValue"));
        }
        #endregion
    }
}
