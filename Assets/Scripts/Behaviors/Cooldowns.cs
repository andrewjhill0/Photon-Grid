using UnityEngine;
using System.Collections;

namespace Behaviors
{
    public class Cooldowns 
    {
        protected static Cooldowns instance;
        public bool isBoostReady;
        /**
           Returns the instance of this singleton.
        */
        public static Cooldowns Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Create new cooldown instance.");
                    instance = new Cooldowns();
                }
                return instance;
            }
        }

        private Cooldowns()
        {
            isBoostReady = true;
        }
    }
}

