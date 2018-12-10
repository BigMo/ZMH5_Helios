using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Maths;
using _ZMH5__Helios.CSGO.ClassIDs;
using _ZMH5__Helios.CSGO.Misc;
using _ZMH5__Helios.CSGO.Entities.EntityHelpers;
using _ZMH5__Helios.CSGO.Enums;
using _ZMH5__Helios.CSGO.Entities.NetVars;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class BaseEntity : EntityPrototype
    {
        #region VARIABLES
        private static LazyCache<int> memSize = new LazyCache<int>(() => System.Math.Max(Program.Offsets.m_iGlowIndex + SizeCache<int>.Size, LargestDataTable(DT_BaseEntity.Instance, DT_AnimTimeMustBeFirst.Instance)));
        #endregion

        #region PROPERTIES
        public float m_flAnimTime { get; private set; }
        public Team m_iTeamNum { get; private set; }
        public int m_iID { get; private set; }
        public Vector3 m_vecOrigin { get; private set; }
        public Vector3 m_angRotation { get; private set; }
        public int m_bSpotted { get; private set; }
        public long m_bSpottedBy { get; private set; }
        public long m_bSpottedByMask { get; private set; }
        public int m_iGlowIndex { get; private set; }
        public float m_flSimulationTime { get; private set; }
        public IntPtr m_pBoneMatrix { get; private set; }
        public Skeleton m_Skeleton { get; private set; }
        public byte m_bDormant { get; private set; }

        public bool IsDormant { get { return m_bDormant == 1; } }
        public override bool IsValid { get { return base.IsValid && m_iID > 0 && m_ClientClass.ClassID > 0; } }
        #endregion


        #region CONSTRUCTORS
        public BaseEntity(IntPtr address, int size) : base(Program.Hack.Memory, address, System.Math.Max(size, memSize)) { }
        public BaseEntity(IntPtr address) : this(address, memSize) { }
        public BaseEntity() : this(memSize) { }
        public BaseEntity(int size) : base(System.Math.Max(size, memSize)) { }

        protected override unsafe void ReadFields(byte* d)
        {
            base.ReadFields(d);

            m_bDormant = *(byte*)(d + 0xED);
            m_iGlowIndex = *(int*)(d + Program.Offsets.m_iGlowIndex);
            m_iID = *(int*)(d + Program.Offsets.m_iID);
            m_pBoneMatrix = *(IntPtr*)(d + Program.Offsets.m_pBoneMatrix);
            m_Skeleton = Program.Hack.Memory.Read<Skeleton>(m_pBoneMatrix);

            m_flAnimTime = *(float*)(d + DT_AnimTimeMustBeFirst.Instance.m_flAnimTime);
            m_iTeamNum = *(Team*)(d + DT_BaseEntity.Instance.m_iTeamNum);
            m_flSimulationTime = *(float*)(d + DT_BaseEntity.Instance.m_flSimulationTime);
            m_vecOrigin = *(Vector3*)(d + DT_BaseEntity.Instance.m_vecOrigin);
            m_angRotation = *(Vector3*)(d + DT_BaseEntity.Instance.m_angRotation);
            m_bSpotted = *(int*)(d + DT_BaseEntity.Instance.m_bSpotted);
            m_bSpottedBy = *(long*)(d + DT_BaseEntity.Instance.m_bSpottedBy);
            m_bSpottedByMask = *(long*)(d + DT_BaseEntity.Instance.m_bSpottedByMask);
        }
        #endregion

        #region METHODS

        public bool SeenById(int id)
        {
            return (m_bSpottedByMask & (0x1 << (id - 1))) != 0;
        }
        public bool SeenBy(BaseEntity other)
        {
            return SeenById(other.m_iID);
        }
        public CSPlayer[] GetPlayersSeeingMe()
        {
            var mask = m_bSpottedByMask;
            List<CSPlayer> players = new List<CSPlayer>();
            for (int i = 0; i < 64; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    var player = Program.Hack.StateMod.Players[i + 1];
                    if (player == null || !player.IsValid || player.m_lifeState != Enums.LifeState.Alive)
                        continue;

                    players.Add(player);
                }
            }
            return players.ToArray();
        }
        public CSPlayer[] GetPlayersISee()
        {
            List<CSPlayer> players = new List<CSPlayer>();
            for (int i = 0; i < 64; i++)
            {
                var player = Program.Hack.StateMod.Players[i + 1];
                if (player == null || !player.IsValid || player.m_lifeState != Enums.LifeState.Alive || !player.SeenBy(this))
                    continue;

                players.Add(player);
            }
            return players.ToArray();
        }
        public float DistanceTo(BaseEntity other)
        {
            if (other == null)
                return 0f;

            return (other.m_vecOrigin - m_vecOrigin).Length;
        }
        #endregion
    }
}
