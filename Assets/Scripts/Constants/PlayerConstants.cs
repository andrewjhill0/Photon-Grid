using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public static class PlayerConstants
    {


        public static readonly int TURNING_SPEED = 75;
        public static readonly int BOOST_DURATION = 2; // why is this static?
        public static readonly int BOOST_COOLDOWN = 2;  // cooldown starts after the boost duration ends.
        public static readonly float BOOST_AMOUNT = 5.0f;
        public static readonly float BASE_VEHICLE_SPEED = 15;
    }
}
