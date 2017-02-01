using UnityEngine;
using System.Collections;
using Global;
using UnityEngine.Networking;

namespace Global {
    /// <summary>
    /// Manages the cooldowns for all players.
    /// </summary>
    public class Cooldowns {
        #region Attributes
        //[SyncVar]
        protected static Cooldowns instance;
        private bool[] isBoostReady;
        private bool[] isWallReady;
        #endregion

        /// <summary>
        /// Constructor.
        /// Gets the arrays initialized.
        /// </summary>
        private Cooldowns() {
            int numOfPlayers = GetGlobalObjects.getPlayerList().Count;

            isBoostReady = new bool[numOfPlayers];
            isWallReady = new bool[numOfPlayers];

            for(int i = 0; i < numOfPlayers; i++) {
                isBoostReady[i] = true;
                isWallReady[i] = true;
            }
        }

        #region GettersAndSetters
        /// <summary>
        /// Returns the instance of this singleton.
        /// </summary>
        public static Cooldowns Instance {
            get {
                if(instance == null) {
                    Debug.Log("Create new cooldown instance.");
                    instance = new Cooldowns();
                }
                return instance;
            }
        }
        public bool[] IsBoostReady {
            get {
                return this.isBoostReady;
            }
            set {
                this.isBoostReady = value;
            }
        }
        public bool[] IsWallReady {
            get {
                return this.isWallReady;
            }
            set {
                this.isWallReady = value;
            }
        }
        #endregion
    }
}

