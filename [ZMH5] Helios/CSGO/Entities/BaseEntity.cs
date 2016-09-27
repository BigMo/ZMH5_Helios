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

namespace _ZMH5__Helios.CSGO.Entities
{
    public class BaseEntity : EntityPrototype
    {
        #region VARIABLES
        private static LazyCache<int> memSize = new LazyCache<int>(() => System.Math.Max(Program.Offsets.m_iGlowIndex, LargestDataTable("DT_BaseEntity")));
        #endregion

        #region PROPERTIES
        public int Size { get { return Data.Length; } }
        public LazyCache<int> m_iTeamNum { get; private set; }
        public LazyCache<int> m_iID { get; private set; }
        public LazyCache<Vector3> m_vecOrigin { get; private set; }
        public LazyCache<Vector3> m_angRotation { get; private set; }
        public LazyCache<int> m_bSpotted { get; private set; }
        public LazyCache<long> m_bSpottedBy { get; private set; }
        public LazyCache<long> m_bSpottedByMask { get; private set; }
        public LazyCache<int> m_iGlowIndex { get; private set; }
        public LazyCache<float> m_flSimulationTime { get; private set; }
        public LazyCache<int> m_pBoneMatrix { get; private set; }
        public LazyCache<RPMLazyArray<Bone>> m_Skeleton { get; private set; }
        public LazyCache<byte> m_bDormant { get; private set; }

        public override int MemSize { get { return memSize.Value; } }
        public override bool IsValid { get { return base.IsValid && m_iID.Value > 0 && m_ClientClass.Value.ClassID > 0 && m_bDormant.Value != 1; } }
        #endregion


        #region CONSTRUCTORS
        public BaseEntity() : base()
        { }
        public BaseEntity(BaseEntity other, int newSize) : base()
        {
            if (other.Size < newSize)
            {
                int size = newSize - other.Size + 64;
                byte[] plusData = new byte[size];
                Program.Hack.Memory.Position = other.Address + other.Size;
                Program.Hack.Memory.Read(plusData, 0, size);
                this.Data = other.Data.Concat(plusData).ToArray();
            }
            else
            {
                this.Data = new byte[other.Data.Length];
                Array.Copy(other.Data, this.Data, this.Data.Length);
            }
            this.stream = new System.IO.MemoryStream(Data);

        }

        public BaseEntity(long address) : this(address, memSize.Value) { }
        public BaseEntity(long address, int size) : base()
        {
            Init(address, size);
        }
        #endregion

        #region METHODS
        protected override void SetupFields()
        {
            base.SetupFields();

            m_bDormant = new LazyCache<byte>(() => this.ReadAt<byte>(0xE9));
            m_iGlowIndex = new LazyCache<int>(() => ReadAt<int>(Program.Offsets.m_iGlowIndex));
            m_iTeamNum = new LazyCache<int>(() => ReadNetVar<int>("m_iTeamNum"));
            m_flSimulationTime = new LazyCache<float>(() => ReadNetVar<float>("m_flSimulationTime"));
            m_iID = new LazyCache<int>(() => ReadAt<int>(Program.Offsets.m_iID));
            m_vecOrigin = new LazyCache<Vector3>(() => ReadNetVar<Vector3>("m_vecOrigin"));
            m_angRotation = new LazyCache<Vector3>(() => ReadNetVar<Vector3>("m_angRotation"));
            m_bSpotted = new LazyCache<int>(() => ReadNetVar<int>("m_bSpotted"));
            m_bSpottedBy = new LazyCache<long>(() => ReadNetVar<long>("m_bSpottedBy"));
            m_bSpottedByMask = new LazyCache<long>(() => ReadNetVar<long>("m_bSpottedByMask"));
            m_pBoneMatrix = new LazyCache<int>(() => ReadAt<int>(Program.Offsets.m_pBoneMatrix));
            m_Skeleton = new LazyCache<RPMLazyArray<Bone>>(() => {
                if (m_pBoneMatrix.Value < 0x1000000 || (uint)m_pBoneMatrix.Value > 0x80000000)
                    return null;

                try
                {
                    return new RPMLazyArray<Bone>(m_pBoneMatrix.Value, 80);
                }catch { return null; }
                }
            );
        }

        public bool SeenById(int id)
        {
            return (m_bSpottedByMask.Value & (0x1 << (id - 1))) != 0;
        }
        public bool SeenBy(BaseEntity other)
        {
            return SeenById(other.m_iID);
        }
        public CSPlayer[] GetPlayersSeeingMe()
        {
            var mask = m_bSpottedByMask.Value;
            List<CSPlayer> players = new List<CSPlayer>();
            for (int i = 0; i < 64; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    var player = Program.Hack.StateMod.Players[i + 1];
                    if (player == null || !player.IsValid || player.m_lifeState.Value != Enums.LifeState.Alive)
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
                if (player == null || !player.IsValid || player.m_lifeState.Value != Enums.LifeState.Alive || !player.SeenBy(this))
                    continue;

                players.Add(player);
            }
            return players.ToArray();
        }
        public float DistanceTo(BaseEntity other)
        {
            if (other == null)
                return 0f;

            return (other.m_vecOrigin.Value = this.m_vecOrigin.Value).Length;
        }
        #endregion
    }
}
