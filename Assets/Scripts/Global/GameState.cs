using UnityEngine;
using System.Collections;
using Controllers;
using Behaviors;
using UnityEngine.Networking;
using System.Collections.Generic;
using Controller;
using Constants;

namespace Global {
    /// <summary>
    /// Singleton class.
    /// Maintains the game's state and must be updated as the game progresses.
    /// </summary>
    public class GameState : NetworkBehaviour {
        #region Attributes
        public int maxHumanPlayers = 1;
        public int numAI = 0;
        //[SyncVar]
        protected static GameState instance;  // Singleton instance
        //GameObject[] players;
        //SyncListStruct<List<GameObject>> players;
        public List<GameObject> players = new List<GameObject>();
        [SyncVar]
        public bool gameOver;
        [SyncVar]
        public Cooldowns cooldowns;

        public GameObject aiPrefab;
        [SyncVar]
        public bool gameReady = false;
        [SyncVar]
        public bool playersInserted = false;

        public GameObject[] spawnPositions;

        public GameObject walls;
        #endregion

        /// <summary>
        /// Create a singleton instance if it already hasn't been made.
        /// DontdestroyonLoad this gameObject
        /// </summary>
        void Awake() {
            Debug.Log("GameState Awake called");
            if(instance == null) {
                Debug.Log("Create new GameState instance.");
                instance = this;
            } else if(instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// If this is the server, start a coroutine to start the game state.
        /// </summary>
        void Start() {
            if(isServer) {
                StartCoroutine(gameStateStart());
            }
        }

        /// <summary>
        /// Get the game state ready
        /// </summary>
        /// <returns></returns>
        private IEnumerator gameStateStart() {
            bool isMultiplayerGame = false;

            if(GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().isMultiplayerGame) {
                isMultiplayerGame = true;

                while(!playersInserted) {
                    yield return new WaitForSeconds(1);
                    if(players.Count == NetworkLobbyManager.singleton.numPlayers)
                        playersInserted = true;
                }
            }
            Debug.Log("GameState Started");
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 60;
            if(isServer) {
                Debug.Log("GameState Started Server statement.");
                Debug.Log(spawnPositions.Length);
                spawnPositions = GameObject.FindGameObjectsWithTag(GlobalTags.SPAWN_POSITION);
                Debug.Log(spawnPositions.Length);
                maxHumanPlayers = GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().maxHumanPlayers;
                numAI = GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().numAI;
                int numPlayers = NetworkManager.singleton.numPlayers + numAI;

                Debug.Log("Player Array is size: " + players.Count);

                NetworkServer.SpawnObjects(); // push changes to clients

                if(isMultiplayerGame) {
                    NetworkLobbyManager.singleton.ServerChangeScene(GlobalTags.GAME_SCREEN);
                } else {
                    NetworkManager.singleton.ServerChangeScene(GlobalTags.GAME_SCREEN);
                }
            }
        }

        // Update is called once per frame
        /// <summary>
        /// Checks to see if the game is over.
        /// Tells players to eject walls. ** TODO This should not be here **
        /// </summary>
        void Update() {
            // Do we need this if statement?  It looks like it's handled in the gameStateStart coroutine
            if(spawnPositions.Length == 0) {
                spawnPositions = GameObject.FindGameObjectsWithTag(GlobalTags.SPAWN_POSITION);
            }


            gameOver = checkIfAllPlayersDead();
            if(gameOver) {
                // end the game after so many seconds.
            }

            if(isServer && gameReady) {
                //let's spawn walls for all players
                for(int i = 0; i < players.Count; i++) {
                    if(players[i].GetComponent<Rigidbody>().velocity.magnitude > 0 && players[i].GetComponent<Controllers.PlayerController>().isAlive) {  // TODO only a temporary check.  we need better sync method for after vehicles are ready.
                        StartCoroutine(PlayerBehaviors.ejectWall(players[i], walls));
                    }
                }
            }
        }

        [ClientRpc]
        public void RpcSetWallToPlayer(GameObject playerDataFromServer, GameObject wall) {
            GameObject player = new GameObject();

            foreach(GameObject playerSearch in GameState.Instance.players) {
                if(playerSearch.GetInstanceID() == playerDataFromServer.GetInstanceID()) {
                    player = playerSearch;
                }
            }

            Debug.Log("RPC SET WALL CALLED");

            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 behindVehicle = vehicle.transform.position - vehicle.transform.forward * PlayerConstants.WALL_SPAWN_DISTANCE;
            behindVehicle.y = PlayerConstants.WALL_HEIGHT;

            int playerNum = player.GetComponent<Controllers.PlayerController>().PlayerNum;
            wall.GetComponent<WallController>().PlayerID = playerNum;
            Material mat = (Material) Resources.Load("Walls/" + GlobalTags.PLAYER_COLORS[playerNum], typeof(Material));

            wall.GetComponent<Renderer>().material = mat;
        }

        [Server]
        public void getGameReady() {
            if(!gameReady) {
                if(isServer) {
                    Debug.Log("spawnPosition length is: " + spawnPositions.Length);

                    NetworkServer.SpawnObjects();
                    InstantiateAIPlayers();
                    //fixPlayerPositions();

                    //gameOver = checkIfAllPlayersDead();  // do we need this here?  is the update() one enough?

                    //assignPlayerNumbers();
                    setAIPlayers();

                    cooldowns = Cooldowns.Instance; // create an instance of the Cooldowns singleton class after players have been initialized.

                    /*foreach(GameObject player in players)
                    {
                        player.SetActive(true);
                    }*/

                    enableAIPlayerControllers();

                    gameReady = true; // setting this true will let the Player Initialization script know that it's time to enable the Player Controllers.
                }
            }
        }

        public void updatePlayerList(GameObject playerObject) {
            if(!players.Contains(playerObject)) {
                Debug.Log("updatePlayer List called.  Player added.", playerObject);
                players.Add(playerObject);
            }
            /*if (players.Count == (NetworkManager.singleton.matchSize))
            {
                getGameReady();
            }*/
        }

        private bool checkIfAllPlayersDead() {
            bool allDead = false;
            int aliveCount = 0;
            foreach(GameObject player in players) {
                Controllers.PlayerController playerContr = player.GetComponent<Controllers.PlayerController>();
                bool isAlive = playerContr.isAlive;
                if(isAlive) {
                    aliveCount++;
                }
            }
            if(aliveCount < 1) {
                allDead = true;
            }
            return allDead;
        }

        private void assignPlayerNumbers() {
            for(int i = 0; i < players.Count; i++) {
                players[i].tag = "Player " + i;
                players[i].GetComponent<Controllers.PlayerController>().PlayerNum = i;
            }
        }

        private void setAIPlayers() {
            //foreach (GameObject player in players) // now we need to setup the AIManager class
            for(int i = 0; i < players.Count; i++) {
                if(players[i].GetComponent<Controllers.PlayerController>().IsAI) {
                    players[i].GetComponent<Controllers.PlayerController>().PlayerNum = i;
                    players[i].tag = GlobalTags.PLAYERS[i];

                    Material mat = (Material) Resources.Load("Vehicles/" + GlobalTags.PLAYER_COLORS[i], typeof(Material));
                    players[i].GetComponent<Renderer>().material = mat;

                    players[i].GetComponent<Transform>().position = GameState.Instance.spawnPositions[i].GetComponent<Transform>().position;
                    players[i].GetComponent<Transform>().rotation = GameState.Instance.spawnPositions[i].GetComponent<Transform>().rotation;

                    players[i].GetComponent<BoxCollider>().enabled = true;

                    AIManager.instance.AiPlayerObjects.Add(players[i]);
                    AIManager.instance.AiPlayers.Add(players[i].GetComponent<Controllers.PlayerController>().PlayerNum);
                }
            }
        }

        /// <summary>
        /// Instantiate the AI player game objects.
        /// </summary>
        private void InstantiateAIPlayers() {
            int numAI = (int) GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().numAI;
            for(int i = 0; i < numAI; i++) {
                GameObject ai = (GameObject) Instantiate(aiPrefab);
                ai.GetComponent<Controllers.PlayerController>().IsAI = true;
                NetworkServer.Spawn(ai);
            }
        }

        /// <summary>
        /// Turn on AI Players.
        /// </summary>
        private void enableAIPlayerControllers() {
            foreach(GameObject player in players) {
                if(player.GetComponent<Controllers.PlayerController>().IsAI) {
                    player.GetComponent<Controllers.PlayerController>().enabled = true;
                }
            }
        }



        private void fixPlayerPositions() {
            for(int i = 0; i < players.Count; i++) {
                players[i].GetComponent<Transform>().position = spawnPositions[i].GetComponent<Transform>().position;
                players[i].GetComponent<Transform>().rotation = spawnPositions[i].GetComponent<Transform>().rotation;
            }
        }

        #region GettersAndSetters
        /// <summary>
        /// Returns a singleton instance of this class.
        /// </summary>
        public static GameState Instance {
            get {
                if(instance == null) {
                    return null;
                }
                return instance;
            }
        }

        public bool getGameOver() {
            return gameOver;
        }

        public GameObject getPlayer(int id) {
            return players[id];
        }

        public List<GameObject> getAllPlayers() {
            return players;
        }
        #endregion
    }
}