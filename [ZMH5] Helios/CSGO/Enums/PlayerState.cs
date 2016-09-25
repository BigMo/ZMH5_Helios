using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Enums
{
    /// <summary>
    /// https://github.com/LestaD/SourceEngine2007/blob/master/se2007/game/shared/tfc/tfc_shareddefs.h
    /// </summary>
    public enum PlayerState : int
    {
        // Happily running around in the game.
        // You can't move though if CSGameRules()->IsFreezePeriod() returns true.
        // This state can jump to a bunch of other states like STATE_PICKINGCLASS or STATE_DEATH_ANIM.
        STATE_ACTIVE = 0,

        // This is the state you're in when you first enter the server.
        // It's switching between intro cameras every few seconds, and there's a level info 
        // screen up.
        STATE_WELCOME,          // Show the level intro screen.

        // During these states, you can either be a new player waiting to join, or
        // you can be a live player in the game who wants to change teams.
        // Either way, you can't move while choosing team or class (or while any menu is up).
        STATE_PICKINGTEAM,          // Choosing team.
        STATE_PICKINGCLASS,         // Choosing class.

        STATE_OBSERVER_MODE,

        STATE_DYING,

        TFC_NUM_PLAYER_STATES
    };

}
