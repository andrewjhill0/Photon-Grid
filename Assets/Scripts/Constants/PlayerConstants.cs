using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public static class PlayerConstants
    {


        public static readonly int TURNING_SPEED = 200; // bigger means faster turns
        public static readonly int BOOST_DURATION = 2; // how many seconds for the boost
        public static readonly int BOOST_COOLDOWN = 2;  // cooldown starts after the boost duration ends.
        public static readonly float BOOST_AMOUNT = 5.0f; // bigger means faster boost
        public static readonly float BASE_VEHICLE_SPEED = 45; // bigger means faster vehicle
        public static readonly int WALL_SPAWN_RESPAWN_TIME = 10; //bigger means more time between walls
        public static readonly float WALL_HEIGHT = 0.5f; // it's the wall height for all vehicle-spawned walls.  should be updated if you change the prefab's setting in Unity.
    }
}
