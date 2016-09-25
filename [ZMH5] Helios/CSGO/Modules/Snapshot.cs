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

namespace _ZMH5__Helios.CSGO.Modules
{
    public class Snapshot : HackModule
    {
        #region VARIABLES

        #endregion

        #region PROPERTIES
        public int EntityList { get; private set; }
        public int ClientStateAddress { get; private set; }
        public int LocalPlayerAddress { get; private set; }
        public LazyCache<CSLocalPlayer> LocalPlayer { get; private set; }
        public EntityCache<CSPlayer> PlayersOld { get; private set; }
        public EntityCache<CSPlayer> Players { get; private set; }
        public EntityCache<BaseCombatWeapon> Weapons { get; private set; }
        public LazyCache<Vector3> ViewAngles { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public Snapshot() : base(ModulePriority.Highest)
        {
            LocalPlayer = new LazyCache<CSLocalPlayer>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(LocalPlayerAddress);
                if (address == 0 || address == -1)
                    return null;

                return new CSLocalPlayer(address);
            });
            ViewAngles = new LazyCache<Vector3>(() =>
            {
                var address = Program.Hack.Memory.Read<int>(ClientStateAddress);
                if (address == 0 || address == -1)
                    return Vector3.Zero;

                return Program.Hack.Memory.Read<Vector3>(address + Program.Offsets.SetViewAngles);
            });
        }
        #endregion

        #region METHODS
        protected override void OnFirstRun(TickEventArgs args)
        {
            base.OnFirstRun(args);

            Players = new EntityCache<CSPlayer>();
            PlayersOld = new EntityCache<CSPlayer>();
            Weapons = new EntityCache<BaseCombatWeapon>();
            EntityList = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.EntityList;
            ClientStateAddress = Program.Hack.EngineDll.BaseAddress.ToInt32() + Program.Offsets.ClientState;
            LocalPlayerAddress = Program.Hack.ClientDll.BaseAddress.ToInt32() + Program.Offsets.LocalPlayer;
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            //Grab local player
            LocalPlayer.Reset();
            ViewAngles.Reset();
            PlayersOld.Clear();
            PlayersOld.CopyFrom(Players);
            Players.Clear();
            Weapons.Clear();
        }
        public void WriteViewAngles(Vector3 angles)
        {
            var address = Program.Hack.Memory.Read<int>(ClientStateAddress);
            if (address == 0 || address == -1)
                return;

            Program.Hack.Memory.Write<Vector3>(address + Program.Offsets.SetViewAngles, angles);
        }

        public CSPlayer[] ReadAllPlayers()
        {
            for (int i = 1; i <= 32; i++)
            {
                var pl = Players[i];
            }

            return Players.Entites.Where(x => x != null).ToArray();
        }
        #endregion
    }
}
