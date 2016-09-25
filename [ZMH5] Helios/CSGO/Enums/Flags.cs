using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.CSGO.Enums
{
    [Flags]
    public enum Flags : int
    {

        FL_ONGROUND = (1 << 0), // At rest / on the ground
        FL_DUCKING = (1 << 1),  // Player flag -- Player is fully crouched
        FL_ANIMDUCKING = (1 << 2),  // Player flag -- Player is in the process of crouching or uncrouching but could be in transition
                                    // examples:                                   Fully ducked:  FL_DUCKING &  FL_ANIMDUCKING
                                    //           Previously fully ducked, unducking in progress:  FL_DUCKING & !FL_ANIMDUCKING
                                    //                                           Fully unducked: !FL_DUCKING & !FL_ANIMDUCKING
                                    //           Previously fully unducked, ducking in progress: !FL_DUCKING &  FL_ANIMDUCKING
        FL_WATERJUMP = (1 << 3),    // player jumping out of water
        FL_ONTRAIN = (1 << 4), // Player is _controlling_ a train, so movement commands should be ignored on client during prediction.
        FL_INRAIN = (1 << 5),   // Indicates the entity is standing in rain
        FL_FROZEN = (1 << 6), // Player is frozen for 3rd person camera
        FL_ATCONTROLS = (1 << 7), // Player can't move, but keeps key inputs for controlling another entity
        FL_CLIENT = (1 << 8),   // Is a player
        FL_FAKECLIENT = (1 << 9),   // Fake client, simulated server side; don't send network messages to them
                                    // NON-PLAYER SPECIFIC = (i.e., not used by GameMovement or the client .dll ), -- Can still be applied to players, though
        FL_INWATER = (1 << 10), // In water


        FL_FLY = (1 << 11), // Changes the SV_Movestep= (), behavior to not need to be on ground
        FL_SWIM = (1 << 12),    // Changes the SV_Movestep= (), behavior to not need to be on ground = (but stay in water),
        FL_CONVEYOR = (1 << 13),
        FL_NPC = (1 << 14),
        FL_GODMODE = (1 << 15),
        FL_NOTARGET = (1 << 16),
        FL_AIMTARGET = (1 << 17),   // set if the crosshair needs to aim onto the entity
        FL_PARTIALGROUND = (1 << 18),   // not all corners are valid
        FL_STATICPROP = (1 << 19),  // Eetsa static prop!		
        FL_GRAPHED = (1 << 20), // worldgraph has this ent listed as something that blocks a connection
        FL_GRENADE = (1 << 21),
        FL_STEPMOVEMENT = (1 << 22),    // Changes the SV_Movestep= (), behavior to not do any processing
        FL_DONTTOUCH = (1 << 23),   // Doesn't generate touch functions, generates Untouch= (), for anything it was touching when this flag was set
        FL_BASEVELOCITY = (1 << 24),    // Base velocity has been applied this frame = (used to convert base velocity into momentum),
        FL_WORLDBRUSH = (1 << 25),  // Not moveable/removeable brush entity = (really part of the world, but represented as an entity for transparency or something),
        FL_OBJECT = (1 << 26), // Terrible name. This is an object that NPCs should see. Missiles, for example.
        FL_KILLME = (1 << 27),  // This entity is marked for death -- will be freed by game DLL
        FL_ONFIRE = (1 << 28),  // You know...
        FL_DISSOLVING = (1 << 29), // We're dissolving!
        FL_TRANSRAGDOLL = (1 << 30), // In the process of turning into a client side ragdoll.
        FL_UNBLOCKABLE_BY_PLAYER = (1 << 31) // pusher that can't be blocked by the player
    }
}
