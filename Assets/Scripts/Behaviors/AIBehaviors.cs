using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Constants;
using Global;

namespace Behaviors
{
    public static class AIBehaviors
    {
        public static void update(GameObject player) //this will be run every frame
        {
            System.Random rnd = new System.Random();
            double turnDirection = rnd.NextDouble();


            if(turnDirection <= 0.5) // turn left
            {
                PlayerBehaviors.turnPlayer(player, InputConstants.INPUT_LEFT);
                if(turnDirection < AIConstants.BOOST_FREQ)
                {
                    AIManager.instance.boostAIPlayer(player);
                }
            }
            else // turn right
            {
                PlayerBehaviors.turnPlayer(player, InputConstants.INPUT_RIGHT);
                if (turnDirection > 1 - AIConstants.BOOST_FREQ)
                {
                    AIManager.instance.boostAIPlayer(player);
                }
            }
        }
    }
}
