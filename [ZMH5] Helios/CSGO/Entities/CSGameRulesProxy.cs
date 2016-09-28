using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;

namespace _ZMH5__Helios.CSGO.Entities
{
    public class CSGameRulesProxy : EntityPrototype
    {
        #region VARIABLES
        public static LazyCache<int> CLASSID = new LazyCache<int>(() => ClassIDs.ClientClassParser.ClientClasses.First(x => x.NetworkName == "CCSGameRulesProxy").ClassID);
        private static LazyCache<int> memSize = new LazyCache<int>(() => LargestDataTable("DT_CSGameRulesProxy", "DT_GameRulesProxy", "DT_CSGameRules"));
        #endregion

        #region PROPERTIES
        public override int MemSize { get { return memSize.Value; } }
        public override bool IsValid { get { return base.IsValid && m_ClientClass.Value.ClassID == CLASSID; } }

        public LazyCache<int> m_iRoundTime { get; private set; }
        public LazyCache<float> m_fMatchStartTime { get; private set; }
        public LazyCache<float> m_fRoundStartTime { get; private set; }
        public LazyCache<int> m_totalRoundsPlayed { get; private set; }
        public LazyCache<int> m_iHostagesRemaining { get; private set; }
        public LazyCache<byte> m_bAnyHostageReached { get; private set; }
        public LazyCache<byte> m_bMapHasBombTarget { get; private set; }
        public LazyCache<byte> m_bMapHasRescueZone { get; private set; }
        public LazyCache<byte> m_bIsQueuedMatchmaking { get; private set; }
        public LazyCache<byte> m_bIsValveDS { get; private set; }
        public LazyCache<byte> m_bHasMatchStarted { get; private set; }
        public LazyCache<byte> m_bBombDropped { get; private set; }
        public LazyCache<byte> m_bBombPlanted { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public CSGameRulesProxy()
        { }
        #endregion

        #region METHODS
        protected override void SetupFields()
        {
            base.SetupFields();

            m_iRoundTime = new LazyCache<int>(() => ReadNetVar<int>("DT_CSGameRules", "m_iRoundTime"));
            m_fMatchStartTime = new LazyCache<float>(() => ReadNetVar<float>("DT_CSGameRules", "m_fMatchStartTime"));
            m_fRoundStartTime = new LazyCache<float>(() => ReadNetVar<float>("DT_CSGameRules", "m_fRoundStartTime"));
            m_totalRoundsPlayed = new LazyCache<int>(() => ReadNetVar<int>("DT_CSGameRules", "m_totalRoundsPlayed"));
            m_iHostagesRemaining = new LazyCache<int>(() => ReadNetVar<int>("DT_CSGameRules", "m_iHostagesRemaining"));
            m_bAnyHostageReached = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bAnyHostageReached"));
            m_bMapHasBombTarget = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bMapHasBombTarget"));
            m_bMapHasRescueZone = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bMapHasRescueZone"));
            m_bIsQueuedMatchmaking = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bIsQueuedMatchmaking"));
            m_bIsValveDS = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bIsValveDS"));
            m_bHasMatchStarted = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bHasMatchStarted"));
            m_bBombDropped = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bBombDropped"));
            m_bBombPlanted = new LazyCache<byte>(() => ReadNetVar<byte>("DT_CSGameRules", "m_bBombPlanted"));
        }
        #endregion
    }
}
