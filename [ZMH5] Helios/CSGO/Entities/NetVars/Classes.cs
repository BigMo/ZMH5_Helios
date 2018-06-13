using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities.NetVars
{
    public class DT_BaseEntity : NetVarResolver
    {
        private static DT_BaseEntity instance;
        public static DT_BaseEntity Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_BaseEntity();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_BaseEntity() { }
        

        public int m_iTeamNum { get; private set; }
        public int m_flSimulationTime { get; private set; }
        public int m_vecOrigin { get; private set; }
        public int m_angRotation { get; private set; }
        public int m_bSpotted { get; private set; }
        public int m_bSpottedBy { get; private set; }
        public int m_bSpottedByMask { get; private set; }
    }

    public class DT_AnimTimeMustBeFirst : NetVarResolver
    {
        private static DT_AnimTimeMustBeFirst instance;
        public static DT_AnimTimeMustBeFirst Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_AnimTimeMustBeFirst();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_AnimTimeMustBeFirst() { }

        public int m_flAnimTime { get; private set; }
    }

    public class DT_BaseCombatWeapon : NetVarResolver
    {
        private static DT_BaseCombatWeapon instance;
        public static DT_BaseCombatWeapon Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_BaseCombatWeapon();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_BaseCombatWeapon() { }

        public int m_hOwner { get; private set; }
        public int m_iClip1 { get; private set; }
        public int m_iClip2 { get; private set; }
    }

    public class DT_WeaponCSBase : NetVarResolver
    {
        private static DT_WeaponCSBase instance;
        public static DT_WeaponCSBase Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_WeaponCSBase();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_WeaponCSBase() { }

        public int m_fAccuracyPenalty { get; private set; }
    }

    public class DT_LocalActiveWeaponData : NetVarResolver
    {
        private static DT_LocalActiveWeaponData instance;
        public static DT_LocalActiveWeaponData Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_LocalActiveWeaponData();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_LocalActiveWeaponData() { }

        public int m_flNextPrimaryAttack { get; private set; }
    }

    public class DT_BasePlayer : NetVarResolver
    {
        private static DT_BasePlayer instance;
        public static DT_BasePlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_BasePlayer();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_BasePlayer() { }

        public int m_iHealth { get; private set; }
        public int m_lifeState { get; private set; }
        public int m_iDefaultFOV { get; private set; }
        public int m_hObserverTarget { get; private set; }
        public int m_fFlags { get; private set; }
        public int m_iObserverMode { get; private set; }
    }

    public class DT_BaseCombatCharacter : NetVarResolver
    {
        private static DT_BaseCombatCharacter instance;
        public static DT_BaseCombatCharacter Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_BaseCombatCharacter();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_BaseCombatCharacter() { }

        public int m_hActiveWeapon { get; private set; }
    }

    public class DT_CSPlayer : NetVarResolver
    {
        private static DT_CSPlayer instance;
        public static DT_CSPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_CSPlayer();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_CSPlayer() { }

        public int m_iMoveState { get; private set; }
        public int m_iPlayerState { get; private set; }
        public int m_bIsScoped { get; private set; }
        public int m_ArmorValue { get; private set; }
    }

    public class DT_CSPlayerResource : NetVarResolver
    {
        private static DT_CSPlayerResource instance;
        public static DT_CSPlayerResource Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_CSPlayerResource();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_CSPlayerResource() { }

        public int m_bombsiteCenterA { get; private set; }
        public int m_bombsiteCenterB { get; private set; }
        [NetVar("m_iScore", 64 * 4)]
        public int m_iScore { get; private set; }
    }

    public class DT_Local : NetVarResolver
    {
        private static DT_Local instance;
        public static DT_Local Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_Local();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_Local() { }

        public int m_aimPunchAngle { get; private set; }
        public int m_viewPunchAngle { get; private set; }
    }

    public class DT_LocalPlayerExclusive : NetVarResolver
    {
        private static DT_LocalPlayerExclusive instance;
        public static DT_LocalPlayerExclusive Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_LocalPlayerExclusive();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_LocalPlayerExclusive() { }

        public int m_Local { get; private set; }
        [NetVar("m_vecViewOffset[0]", 12)]
        public int m_vecViewOffset { get; private set; }
        [NetVar("m_vecVelocity[0]", 12)]
        public int m_vecVelocity { get; private set; }
    }

    public class DT_CSLocalPlayerExclusive : NetVarResolver
    {
        private static DT_CSLocalPlayerExclusive instance;
        public static DT_CSLocalPlayerExclusive Instance
        {
            get
            {
                if (instance == null)
                {
                    var inst = new DT_CSLocalPlayerExclusive();
                    inst.Resolve();
                    if (inst.Resolved)
                        instance = inst;
                }
                return instance;
            }
        }
        private DT_CSLocalPlayerExclusive() { }

        public int m_iShotsFired { get; private set; }
    }
    //DT_CSLocalPlayerExclusive
}
