using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Controller {
    /// <summary>
    /// Controls behavior for walls and assigns them to the player who created them.
    /// </summary>
    public class WallController : NetworkBehaviour {
        private int playerID; //who does this wall belong to

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        #region GettersAndSetters
        public int PlayerID {
            get {
                return this.playerID;
            }
            set {
                this.playerID = value;
            }
        }
        #endregion
    }
}