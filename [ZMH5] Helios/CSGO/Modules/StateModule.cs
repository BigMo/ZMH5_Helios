using _ZMH5__Helios.CSGO.Entities;
using _ZMH5__Helios.CSGO.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.Core;
using _ZMH5__Helios.CSGO.Modules.SnapshotHelpers;
using ZatsHackBase.Maths;
using _ZMH5__Helios.CSGO.Enums;

namespace _ZMH5__Helios.CSGO.Modules
{
    public class StateModule : HackModule
    {
        #region VARIABLES
        private int pEntityList;
        private int pClientState;
        private int pLocalPlayer;
        private int pViewMatrix;
        private int pGameRules;
        private int pPlayerResources;
        private int pRadarAddress;

        #endregion

        #region PROPERTIES
        public LazyCache<CSLocalPlayer> LocalPlayer { get; private set; }
        public EntityCache<CSPlayer> PlayersOld { get; private set; }
        public EntityCache<CSPlayer> Players { get; private set; }
        public EntityCache<BaseCombatWeapon> Weapons { get; private set; }
        public LazyCache<Vector3> ViewAngles { get; private set; }
        public LazyCache<Matrix> ViewMatrix { get; private set; }
        public LazyCache<CSGameRulesProxy> GameRules { get; private set; }
        public LazyCache<CSPlayerResource> PlayerResources { get; private set; }
        public string[] Names { get; private set; }
        public LazyCache<RadarEntry[]> RadarEntries { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public StateModule() : base(Program.Hack, ModulePriority.Highest)
        {
            LocalPlayer = new LazyCache<CSLocalPlayer>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(pLocalPlayer);
                if (address == 0 || address == -1)
                    return null;

                return new CSLocalPlayer(address);
            });
            ViewAngles = new LazyCache<Vector3>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(pClientState);
                if (address == 0 || address == -1)
                    return Vector3.Zero;

                return Program.Hack.Memory.Read<Vector3>(address + Program.Offsets.SetViewAngles);
            });
            GameRules = new LazyCache<CSGameRulesProxy>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(pGameRules);
                if (address == 0 || address == -1)
                    return null;

                return Program.Hack.GetEntityByAddress<CSGameRulesProxy>(address);
            });
            PlayerResources = new LazyCache<CSPlayerResource>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(pPlayerResources);
                if (address == 0 || address == -1)
                    return null;

                return Program.Hack.GetEntityByAddress<CSPlayerResource>(address);
            });
            RadarEntries = new LazyCache<RadarEntry[]>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(pRadarAddress);
                if (address == 0 || address == -1)
                    return null;

                address = Program.Hack.Memory.Read<int>(address + Program.Offsets.RadarOffset);
                if (address == 0 || address == -1)
                    return null;

                Radar r = Program.Hack.Memory.Read<Radar>(address);
                return r.Entries;
            });
            ViewMatrix = new LazyCache<Matrix>(() => Program.Hack.Memory.Read<Matrix>(pViewMatrix));
        }
        #endregion

        #region METHODS
        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            Players = new EntityCache<CSPlayer>();
            PlayersOld = new EntityCache<CSPlayer>();
            PlayersOld.RequestMissingEntities = false;
            Weapons = new EntityCache<BaseCombatWeapon>();
            pEntityList = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.EntityList;
            pClientState = Program.Hack.EngineDll.BaseAddress.ToInt32() + Program.Offsets.ClientState;
            pLocalPlayer = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.LocalPlayer;
            pViewMatrix = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.m_mViewMatrix;
            pGameRules = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.GameRulesProxy;
            pPlayerResources = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.PlayerResources;
            pRadarAddress = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.RadarBase;
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            //Grab local player
            LocalPlayer.Reset();
            ViewAngles.Reset();
            ViewMatrix.Reset();
            GameRules.Reset();
            PlayerResources.Reset();
            RadarEntries.Reset();

            PlayersOld.Clear();
            PlayersOld.CopyFrom(Players);
            Players.Clear();
            Weapons.Clear();
        }
        public void WriteViewAngles(Vector3 angles)
        {
            var address = Program.Hack.Memory.Read<int>(pClientState);
            if (address == 0 || address == -1)
                return;

            Program.Hack.Memory.Write<Vector3>(address + Program.Offsets.SetViewAngles, angles);
        }

        public CSPlayer[] GetAllPlayers()
        {
            for (int i = 1; i <= 64; i++)
            {
                var pl = Players[i];
            }

            return Players.Entites.Where(x => x != null).ToArray();
        }

        public CSPlayer[] GetPlayersSet(bool isValid=true, bool isAlive=true, Team team = Team.None)
        {
            var players = (IEnumerable<CSPlayer>)GetAllPlayers();

            if (isValid)
                players = players.Where(x => x.IsValid);
            if (isAlive)
                players = players.Where(x => x.m_lifeState.Value == LifeState.Alive);
            if (team != Team.None)
                players = players.Where(x => x.m_iTeamNum.Value == team);

            return players.ToArray();
        }
        #endregion
    }
}
