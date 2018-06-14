using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _ZMH5__Helios.CSGO.Entities.NetVars;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class BaseCombatWeapon : BaseEntity
    {
        #region VARIABLES
        private static LazyCache<int> memSize = new LazyCache<int>(() => System.Math.Max(LargestDataTable(DT_BaseCombatWeapon.Instance, DT_WeaponCSBase.Instance, DT_LocalActiveWeaponData.Instance), Program.Offsets.m_iItemDefinitionIndex + 4));
        private static LazyCache<int[]> idsPistol = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsPistol.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsSniper = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsSniper.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsRifle = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsRifle.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsPumpgun = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsPumpgun.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsMG = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsMG.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsMP = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsMP.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsGrenade = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsGrenade.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsGrenadeProjectile = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsGrenadeProjectile.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsC4 = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsC4.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsKnife = new LazyCache<int[]>(() => {
            return ClassIDs.ClientClassParser.ClientClasses.Where(x => clsKnife.Contains(x.NetworkName)).Select(x => x.ClassID).ToArray();
        });
        private static LazyCache<int[]> idsAll = new LazyCache<int[]>(() =>
        {
            return idsC4.Value.Concat(idsGrenade.Value).
                Concat(idsGrenadeProjectile.Value).
                Concat(idsKnife.Value).
                Concat(idsMG.Value).
                Concat(idsMP.Value).
                Concat(idsPistol.Value).
                Concat(idsPumpgun.Value).
                Concat(idsRifle.Value).
                Concat(idsSniper.Value).ToArray();
        });

        private static string[] clsPistol = new string[] {
            "CDEagle",
            "CWeaponTec9",
            "CWeaponElite",
            "CWeaponFiveSeven",
            "CWeaponGlock",
            "CWeaponHKP2000",
            "CWeaponP228",
            "CWeaponP250",
            "CWeaponUSP"
        };
        private static string[] clsSniper = new string[] {
            "CSCAR17",
            "CWeaponAWP",
            "CWeaponG3SG1",
            "CWeaponSCAR20",
            "CWeaponScout",
            "CWeaponSSG08",
        };
        private static string[] clsRifle = new string[] {
            "CAK47",
            "CWeaponAug",
            "CWeaponFamas",
            "CWeaponGalil",
            "CWeaponGalilAR",
            "CWeaponM4A1",
            "CWeaponSG550",
            "CWeaponSG552",
            "CWeaponSG556",
        };
        private static string[] clsPumpgun = new string[] {
            "CWeaponM3",
            "CWeaponMag7",
            "CWeaponXM1014",
            "CWeaponNOVA",
            "CWeaponSawedoff"
        };
        private static string[] clsMG = new string[] {
            "CWeaponM249",
            "CWeaponNegev",
        };
        private static string[] clsMP = new string[] {
            "CWeaponBizon",
            "CWeaponMAC10",
            "CWeaponMP5Navy",
            "CWeaponP90",
            "CWeaponMP7",
            "CWeaponMP9",
            "CWeaponTMP",
            "CWeaponUMP45",
        };
        private static string[] clsGrenade = new string[] {
            "CFlashbang",
            "CHEGrenade",
            "CSmokeGrenade",
            "CMolotovGrenade",
            "CIncendiaryGrenade",
            "ParticleSmokeGrenade",
            "CDecoyGrenade",
            "CSensorGrenade",
            "CBaseCSGrenade",
            "CBaseGrenade"
        };
        private static string[] clsGrenadeProjectile = new string[] {
            "CSmokeGrenadeProjectile",
            "CMolotovProjectile",
            "CDecoyProjectile",
            "CSensorGrenadeProjectile",
            "CBaseCSGrenadeProjectile"
        };
        private static string[] clsC4 = new string[] {
            "CC4",
            "CPlantedC4"
        };
        private static string[] clsKnife = new string[] {
            "CKnife",
            "CKnifeGG"
        };
        #endregion

        #region PROPERTIES
        public int m_hOwner { get; private set; }
        public int m_iClip1 { get; private set; }
        public int m_iClip2 { get; private set; }
        public float m_fAccuracyPenalty { get; private set; }
        public float m_flNextPrimaryAttack { get; private set; }
        public bool IsPistol { get; private set; }
        public bool IsSniper { get; private set; }
        public bool IsRifle { get; private set; }
        public bool IsMP { get; private set; }
        public bool IsMG { get; private set; }
        public bool IsGrenade { get; private set; }
        public bool IsGrenadeProjectile { get; private set; }
        public bool IsC4 { get; private set; }
        public bool IsKnife { get; private set; }
        public bool IsPumpgun { get; private set; }
        public int WeaponId { get; private set; }
        public override bool IsValid
        {
            get
            {
                return base.IsValid &&
                    (IsC4 || IsPistol || IsSniper || IsRifle || IsMP || IsMG || IsGrenade || IsGrenadeProjectile || IsKnife || IsPumpgun);
            }
        }
        #endregion

        #region CONSTRUCTORS
        public BaseCombatWeapon(IntPtr address) : base(address, memSize.Value) { }
        public BaseCombatWeapon() : this(memSize) { }
        public BaseCombatWeapon(int size) : base(System.Math.Max(size, memSize)) { }
        #endregion

        #region METHODS
        protected override unsafe void ReadFields(byte* d)
        {
            base.ReadFields(d);

            if (m_ClientClass == null)
                return;

            m_hOwner = *(int*)(d + DT_BaseCombatWeapon.Instance.m_hOwner);
            m_iClip1 = *(int*)(d + DT_BaseCombatWeapon.Instance.m_iClip1);
            m_iClip2 = *(int*)(d + DT_BaseCombatWeapon.Instance.m_iClip2);
            m_fAccuracyPenalty = *(float*)(d + DT_WeaponCSBase.Instance.m_fAccuracyPenalty);
            m_flNextPrimaryAttack = *(float*)(d + DT_LocalActiveWeaponData.Instance.m_flNextPrimaryAttack);
            WeaponId = *(int*)(d + Program.Offsets.m_iItemDefinitionIndex);

            IsPistol = idsPistol.Value.Contains(m_ClientClass.ClassID);
            IsSniper = idsSniper.Value.Contains(m_ClientClass.ClassID);
            IsRifle = idsRifle.Value.Contains(m_ClientClass.ClassID);
            IsMP = idsMP.Value.Contains(m_ClientClass.ClassID);
            IsMG = idsMG.Value.Contains(m_ClientClass.ClassID);
            IsGrenade = idsGrenade.Value.Contains(m_ClientClass.ClassID);
            IsGrenadeProjectile = idsGrenadeProjectile.Value.Contains(m_ClientClass.ClassID);
            IsC4 = idsC4.Value.Contains(m_ClientClass.ClassID);
            IsKnife = idsKnife.Value.Contains(m_ClientClass.ClassID);
            IsPumpgun = idsPumpgun.Value.Contains(m_ClientClass.ClassID);
        }

        public static bool IsWeapon(EntityPrototype other)
        {
            if (other == null || !other.IsValid)
                return false;

            return idsAll.Value.Any(x => x == other.m_ClientClass.ClassID);
        }
        #endregion
    }
}
