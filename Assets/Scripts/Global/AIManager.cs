using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Behaviors;
using Constants;
using UnityEngine.SceneManagement;       //Allows us to use Lists. 

namespace Global {
    /// <summary>
    /// Singleton class used to manage AI behavior and AI vehicle housekeeping.
    /// </summary>
    public class AIManager : MonoBehaviour {
        #region Attributes
        public static AIManager instance = null;              //Static instance of AIManager which allows it to be accessed by any other script.
        //private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
        //private int level = 3;                                  //Current level number, expressed in game as "Day 1".
        private List<int> aiPlayers = new List<int>();
        private List<GameObject> aiPlayerObjects = new List<GameObject>();
        #endregion

        //Awake is always called before any Start functions
        /// <summary>
        /// Initialize the singleton instance.
        /// Set DontDestroyOnLoad for this gameObject
        /// </summary>
        void Awake() {
            //Check if instance already exists
            if(instance == null)
                instance = this;

            //If instance already exists and it's not this:
            else if(instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            Debug.Log("AIManager Awake.");
        }

        //Update is called every frame.
        /// <summary>
        /// Waits until the active scene is the game screen.
        /// Updates the AI player's behaviors.
        /// </summary>
        void Update() {
            if(SceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN) {
                //Debug.Log("AIManager Update");
                if(GameState.Instance.gameReady) {
                    foreach(GameObject player in AiPlayerObjects) {
                        System.Random rnd = new System.Random();
                        double turnDirection = rnd.NextDouble();

                        if(turnDirection <= 0.5) // turn left
                        {
                            PlayerBehaviors.turnPlayer(player, InputConstants.INPUT_LEFT);
                            if(turnDirection < AIConstants.BOOST_FREQ) {
                                boostAIPlayer(player);
                            }
                        } else // turn right
                        {
                            PlayerBehaviors.turnPlayer(player, InputConstants.INPUT_RIGHT);
                            if(turnDirection > 1 - AIConstants.BOOST_FREQ) {
                                boostAIPlayer(player);
                            }
                        }
                    }
                }
            }
        }

        #region GettersAndSetters
        public List<int> AiPlayers {
            get {
                return this.aiPlayers;
            }
            set {
                this.aiPlayers = value;
            }
        }
        public List<GameObject> AiPlayerObjects {
            get {
                return this.aiPlayerObjects;
            }
            set {
                this.aiPlayerObjects = value;
            }
        }
        #endregion

        /// <summary>
        /// Starts a coroutine to speed boost an AI player.
        /// </summary>
        /// <param name="player">The player to be boosted</param>
        public void boostAIPlayer(GameObject player) {
            StartCoroutine(PlayerBehaviors.activateSpeedBoost(player));
        }
    }
}
