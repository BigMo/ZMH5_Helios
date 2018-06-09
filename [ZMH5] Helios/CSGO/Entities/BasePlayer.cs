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
    public class BasePlayer : BaseEntity
    {
        #region VARIABLES
        private static LazyCache<int> memSize = new LazyCache<int>(() => LargestDataTable(DT_BasePlayer.Instance, DT_BaseCombatCharacter.Instance));
        #endregion

        #region PROPERTIES
        public int m_iHealth { get; private set; }
        public int m_hObserverTarget { get; private set; }
        public int m_hActiveWeapon { get; private set; }
        public int m_iDefaultFOV { get; private set; }
        public Flags m_fFlags { get; private set; }
        public ObserverMode m_iObserverMode { get; private set; }
        public LifeState m_lifeState { get; private set; }
        public LazyCache<BaseCombatWeapon> m_ActiveWeapon { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public BasePlayer(IntPtr address, int size) : base(address, size) { }
        public BasePlayer(IntPtr address) : this(address, memSize.Value) { }
        public BasePlayer() : this(memSize) { }
        public BasePlayer(int size) : base(System.Math.Max(size, memSize)) { }
        #endregion

        #region METHODS
        protected override unsafe void ReadFields(byte* d)
        {
            base.ReadFields(d);
            m_iHealth = *(int*)(d + DT_BasePlayer.Instance.m_iHealth);
            m_lifeState = *(LifeState*)(d + DT_BasePlayer.Instance.m_lifeState);
            m_iDefaultFOV = *(int*)(d + DT_BasePlayer.Instance.m_iDefaultFOV);
            m_hObserverTarget = *(int*)(d + DT_BasePlayer.Instance.m_hObserverTarget);
            m_fFlags = *(Flags*)(d + DT_BasePlayer.Instance.m_fFlags);
            m_iObserverMode = *(ObserverMode*)(d + DT_BasePlayer.Instance.m_iObserverMode);
            m_hActiveWeapon = *(int*)(d + DT_BaseCombatCharacter.Instance.m_hActiveWeapon) & 0xFFF;
            m_ActiveWeapon = new LazyCache<BaseCombatWeapon>(GetWeapon);
        }
        private BaseCombatWeapon GetWeapon()
        {
            return Program.Hack.StateMod.Weapons[m_hActiveWeapon];
        }
        #endregion
    }
}
