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
using _ZMH5__Helios.CSGO.BSP;
using System.IO;

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
        private string lastMap;
        #endregion

        #region PROPERTIES
        public LazyCache<CSLocalPlayer> LocalPlayer { get; private set; }
        public EntityCache<CSPlayer> PlayersOld { get; private set; }
        public EntityCache<CSPlayer> Players { get; private set; }
        public EntityCache<BaseCombatWeapon> Weapons { get; private set; }
        public EntityCache<BaseEntity> BaseEntitites { get; private set; }
        //public LazyCache<Vector3> ViewAngles { get; private set; }
        public LazyCache<Matrix> ViewMatrix { get; private set; }
        public LazyCache<CSGameRulesProxy> GameRules { get; private set; }
        public LazyCache<CSPlayerResource> PlayerResources { get; private set; }
        public string[] Names { get; private set; }
        public LazyCache<RadarEntry[]> RadarEntries { get; private set; }
        public LazyCache<ClientState> ClientState { get; private set; }
        public LazyCache<string> GameDirectory { get; private set; }
        public BSPFile Map { get; private set; }
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
            ClientState = new LazyCache<ClientState>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(pClientState);
                if (address == 0 || address == -1)
                    return null;

                return new ClientState(address);
            });
            GameDirectory = new LazyCache<string>(() =>
            {
                return Program.Hack.Memory.ReadString(Program.Hack.EngineDll.BaseAddress.ToInt32() + Program.Offsets.GameDirectory, 256, Encoding.UTF8);
            });
            GameRules = new LazyCache<CSGameRulesProxy>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(pGameRules);
                if (address == 0 || address == -1)
                    return null;

                //var grp = Program.Hack.GetEntityByAddress<CSGameRulesProxy>(address);
                var grp = Program.Hack.GetEntityByAddress<CSGameRulesProxy>(pGameRules);
                return grp;
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
                //var address = Program.Hack.Memory.Read<int>(pRadarAddress);
                //if (address == 0 || address == -1)
                //    return null;

                //address = Program.Hack.Memory.Read<int>(address + Program.Offsets.RadarOffset);
                //if (address == 0 || address == -1)
                //    return null;

                //Radar r = Program.Hack.Memory.Read<Radar>(address);
                //return r.Entries;
                return new RadarEntry[0];
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
            BaseEntitites = new EntityCache<BaseEntity>();
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
            ViewMatrix.Reset();
            GameRules.Reset();
            PlayerResources.Reset();
            RadarEntries.Reset();
            ClientState.Reset();
            GameDirectory.Reset();

            BaseEntitites.Clear();
            PlayersOld.Clear();
            PlayersOld.CopyFrom(Players);
            Players.Clear();
            Weapons.Clear();

            //Load map
            if(ClientState.Value != null && ClientState.Value.Map.Value != null)
            {
                if (ClientState.Value.Map.Value != lastMap)
                {
                    var path = Path.Combine(GameDirectory.Value, ClientState.Value.Map.Value);
                    //try
                    //{
                        lastMap = ClientState.Value.Map.Value;
                    if (File.Exists(path))
                    {
                        using (var str = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            var bsp = new BSPFile(str);
                            Map = bsp;
                        }
                        //}catch(Exception ex)
                        //{
                        //    Program.Logger.Error("Failed to parse map \"{0}\": {1}", path, ex.Message);
                        //}
                    }
                }
            }
        }
        public void WriteViewAngles(Vector3 angles)
        {
            var address = Program.Hack.Memory.Read<int>(pClientState);
            if (address == 0 || address == -1)
                return;

            Program.Hack.Memory.Write<Vector3>(address + Program.Offsets.ClientStateSetViewAngles, angles);
        }

        public CSPlayer[] GetAllPlayers()
        {
            for (int i = 1; i <= 64; i++)
            {
                var pl = Players[i];
            }

            return Players.Entites.Where(x => x != null).ToArray();
        }

        public CSPlayer[] GetPlayersSet(bool isValid=true, bool isAlive=true, bool isActive=false, Team team = Team.None)
        {
            var players = (IEnumerable<CSPlayer>)GetAllPlayers();

            if (isValid)
                players = players.Where(x => x.IsValid);
            if (isAlive)
                players = players.Where(x => x.m_lifeState.Value == LifeState.Alive);
            if(isActive)
                players = players.Where(x => !x.IsDormant);
            if (team != Team.None)
                players = players.Where(x => x.m_iTeamNum.Value == team);

            return players.ToArray();
        }
        #endregion
    }
}
