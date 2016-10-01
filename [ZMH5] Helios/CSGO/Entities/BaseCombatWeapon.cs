using ZatsHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class BaseCombatWeapon : BaseEntity
    {
        #region VARIABLES
        private static LazyCache<int> memSize = new LazyCache<int>(() => LargestDataTable("DT_BaseCombatWeapon", "DT_WeaponCSBase", "DT_LocalActiveWeaponData"));
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
        public LazyCache<int> m_hOwner { get; private set; }
        public LazyCache<int> m_iClip1 { get; private set; }
        public LazyCache<int> m_iClip2 { get; private set; }
        public LazyCache<float> m_fAccuracyPenalty { get; private set; }
        public LazyCache<float> m_flNextPrimaryAttack { get; private set; }
        public override int MemSize { get { return memSize.Value; } }
        public LazyCache<bool> IsPistol { get; private set; }
        public LazyCache<bool> IsSniper { get; private set; }
        public LazyCache<bool> IsRifle { get; private set; }
        public LazyCache<bool> IsMP { get; private set; }
        public LazyCache<bool> IsMG { get; private set; }
        public LazyCache<bool> IsGrenade { get; private set; }
        public LazyCache<bool> IsGrenadeProjectile { get; private set; }
        public LazyCache<bool> IsC4 { get; private set; }
        public LazyCache<bool> IsKnife { get; private set; }
        public LazyCache<bool> IsPumpgun { get; private set; }
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
        public BaseCombatWeapon() : base() { }
        public BaseCombatWeapon(BaseEntity other) : this(other, memSize.Value) { }
        public BaseCombatWeapon(BaseEntity other, int newSize) : base(other, newSize) { }

        public BaseCombatWeapon(long address, int size) : base(address, size) { }
        public BaseCombatWeapon(long address) : base(address, memSize.Value) { }
        #endregion

        #region METHODS
        public static bool IsWeapon(EntityPrototype other)
        {
            if (other == null || !other.IsValid)
                return false;

            return idsAll.Value.Any(x => x == other.m_ClientClass.Value.ClassID);
        }
        protected override void SetupFields()
        {
            base.SetupFields();

            m_hOwner = new LazyCache<int>(() => ReadNetVar<int>("DT_BaseCombatWeapon", "m_hOwner"));
            m_iClip1 = new LazyCache<int>(() => ReadNetVar<int>("DT_BaseCombatWeapon", "m_iClip1"));
            m_iClip2 = new LazyCache<int>(() => ReadNetVar<int>("DT_BaseCombatWeapon", "m_iClip2"));
            m_fAccuracyPenalty = new LazyCache<float>(() => ReadNetVar<float>("DT_WeaponCSBase", "m_fAccuracyPenalty"));
            m_flNextPrimaryAttack = new LazyCache<float>(() => ReadNetVar<float>("DT_LocalActiveWeaponData", "m_flNextPrimaryAttack"));
            IsPistol = new LazyCache<bool>(() => m_ClientClass != null && idsPistol.Value.Contains(m_ClientClass.Value.ClassID));
            IsSniper = new LazyCache<bool>(() => m_ClientClass != null && idsSniper.Value.Contains(m_ClientClass.Value.ClassID));
            IsRifle = new LazyCache<bool>(() => m_ClientClass != null && idsRifle.Value.Contains(m_ClientClass.Value.ClassID));
            IsMP = new LazyCache<bool>(() => m_ClientClass != null && idsMP.Value.Contains(m_ClientClass.Value.ClassID));
            IsMG = new LazyCache<bool>(() => m_ClientClass != null && idsMG.Value.Contains(m_ClientClass.Value.ClassID));
            IsGrenade = new LazyCache<bool>(() => m_ClientClass != null && idsGrenade.Value.Contains(m_ClientClass.Value.ClassID));
            IsGrenadeProjectile = new LazyCache<bool>(() => m_ClientClass != null && idsGrenadeProjectile.Value.Contains(m_ClientClass.Value.ClassID));
            IsC4 = new LazyCache<bool>(() => m_ClientClass != null && idsC4.Value.Contains(m_ClientClass.Value.ClassID));
            IsKnife = new LazyCache<bool>(() => m_ClientClass != null && idsKnife.Value.Contains(m_ClientClass.Value.ClassID));
            IsPumpgun = new LazyCache<bool>(() => m_ClientClass != null && idsPumpgun.Value.Contains(m_ClientClass.Value.ClassID));
        }
        #endregion
    }
}
