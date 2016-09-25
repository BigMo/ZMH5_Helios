using _ZMH5__Helios.CSGO.Enums;
using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class BasePlayer : BaseEntity
    {
        #region VARIABLES
        private static LazyCache<int> memSize = new LazyCache<int>(() => LargestDataTable("DT_BasePlayer", "DT_BaseCombatCharacter"));
        #endregion

        #region PROPERTIES
        public LazyCache<int> m_iHealth { get; private set; }
        public LazyCache<int> m_hObserverTarget { get; private set; }
        public LazyCache<int> m_hActiveWeapon { get; private set; }
        public LazyCache<BaseCombatWeapon> m_ActiveWeapon { get; private set; }
        public LazyCache<int> m_iDefaultFOV { get; private set; }
        public LazyCache<Flags> m_fFlags { get; private set; }
        public LazyCache<ObserverMode> m_hObserverMode { get; private set; }
        public LazyCache<LifeState> m_lifeState { get; private set; }
        public override int MemSize { get { return memSize.Value; } }
        #endregion

        #region CONSTRUCTORS
        public BasePlayer() : base() { }
        public BasePlayer(BaseEntity other) : this(other, memSize.Value) { }
        public BasePlayer(BaseEntity other, int newSize) : base(other, newSize) { }

        public BasePlayer(long address, int size) : base(address, size) { }
        public BasePlayer(long address) : base(address, memSize.Value) { }
        #endregion

        #region METHODS
        protected override void SetupFields()
        {
            base.SetupFields();

            m_iHealth = new LazyCache<int>(() => ReadNetVar<int>("DT_BasePlayer", "m_iHealth"));
            m_lifeState = new LazyCache<LifeState>(() => (LifeState)ReadNetVar<int>("DT_BasePlayer", "m_lifeState"));
            m_iDefaultFOV = new LazyCache<int>(() => ReadNetVar<int>("DT_BasePlayer", "m_iDefaultFOV"));
            m_hObserverTarget = new LazyCache<int>(() => ReadNetVar<int>("DT_BasePlayer", "m_hObserverTarget") & 0xFFF);
            m_fFlags = new LazyCache<Flags>(() => (Flags)ReadNetVar<int>("DT_BasePlayer", "m_fFlags"));
            m_hObserverMode = new LazyCache<ObserverMode>(() => (ObserverMode)ReadNetVar<int>("DT_BasePlayer", "m_hObserverMode"));
            m_hActiveWeapon = new LazyCache<int>(() => ReadNetVar<int>("DT_BaseCombatCharacter", "m_hActiveWeapon") & 0xFFF);
            m_ActiveWeapon = new LazyCache<BaseCombatWeapon>(() => Program.Hack.StateMod.Weapons[m_hActiveWeapon.Value]);
        }
        #endregion
    }
}
