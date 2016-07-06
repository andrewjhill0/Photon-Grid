using UnityEngine;
using System.Collections;
using Controllers;
using Behaviors;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Global
{
    public class GameState : NetworkBehaviour
    {
        public int maxHumanPlayers = 1;
        public int numAI = 0;
        //[SyncVar]
        protected static GameState instance; 
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


        public static GameState Instance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                return instance;
            }
        }

        void Awake()
        {
            Debug.Log("GameState Awake called");
            if (instance == null)
            {
                Debug.Log("Create new GameState instance.");
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);


        }

        // Use this for initialization
        void Start()
        {
            if(isServer)
            {
                StartCoroutine(gameStateStart());
            }
        }

        private IEnumerator gameStateStart()
        {
            while (!playersInserted)
            {
                yield return new WaitForSeconds(1);
                if (players.Count == NetworkLobbyManager.singleton.numPlayers) playersInserted = true;
            }
            Debug.Log("GameState Started");
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 60;
            if (isServer)
            {
                Debug.Log("GameState Started Server statement.");
                Debug.Log(spawnPositions.Length);
                spawnPositions = GameObject.FindGameObjectsWithTag(GlobalTags.SPAWN_POSITION);
                Debug.Log(spawnPositions.Length);
                maxHumanPlayers = GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().maxHumanPlayers;
                numAI = GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().numAI;
                int numPlayers = NetworkManager.singleton.numPlayers + numAI;

                Debug.Log("Player Array is size: " + players.Count);


                NetworkServer.SpawnObjects();

                NetworkLobbyManager.singleton.ServerChangeScene(GlobalTags.GAME_SCREEN);
            }
        }



        // Update is called once per frame
        void Update()
        {
            if(spawnPositions.Length == 0)
            {
                spawnPositions = GameObject.FindGameObjectsWithTag(GlobalTags.SPAWN_POSITION);
            }


            gameOver = checkIfAllPlayersDead();
            if(gameOver)
            {
                // end the game after so many seconds.
            }
        }

        public bool getGameOver() 
        {
            return gameOver;
        }

        public GameObject getPlayer(int id)
        {
            return players[id];
        }

        public List<GameObject> getAllPlayers()
        {
            return players;
        }

        [Server]
        public void getGameReady()
        {
            if (!gameReady)
            {
                if (isServer)
                {
                    Debug.Log(spawnPositions.Length);

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


        public void updatePlayerList(GameObject playerObject)
        {
            if (!players.Contains(playerObject))
            {
                players.Add(playerObject);
            }
            /*if (players.Count == (NetworkManager.singleton.matchSize))
            {
                getGameReady();
            }*/
        }

        private bool checkIfAllPlayersDead()
        {
            bool allDead = false;
            int aliveCount = 0;
            foreach (GameObject player in players)
            {
                Controllers.PlayerController playerContr = player.GetComponent<Controllers.PlayerController>();
                bool isAlive = playerContr.isAlive;
                if(isAlive)
                {
                    aliveCount++;
                }
            }
            if (aliveCount <= 1)
            {
                allDead = true;
            }
            return allDead;
        }

        private void assignPlayerNumbers()
        {
            for(int i = 0; i < players.Count; i++)
            {
                players[i].tag = "Player " + i;
                players[i].GetComponent<Controllers.PlayerController>().PlayerNum = i;
            }
        }

        private void setAIPlayers()
        {
            foreach (GameObject player in players) // now we need to setup the AIManager class
            {
                if (player.GetComponent<Controllers.PlayerController>().IsAI)
                {
                    AIManager.instance.AiPlayerObjects.Add(player);
                    AIManager.instance.AiPlayers.Add(player.GetComponent<Controllers.PlayerController>().PlayerNum);
                }
            }
        }

        private void InstantiateAIPlayers()
        {
            int numAI = (int)GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().numAI;
            for (int i = 0; i < numAI; i++)
            {
                GameObject ai = (GameObject)Instantiate(aiPrefab);
                ai.GetComponent<Controllers.PlayerController>().IsAI = true;
                NetworkServer.Spawn(ai);
            }
        }

        private void enableAIPlayerControllers() 
        {
            foreach (GameObject player in players) 
            {
                if (player.GetComponent<Controllers.PlayerController>().IsAI)
                {
                    player.GetComponent<Controllers.PlayerController>().enabled = true;
                }
            }
        }


        
        private void fixPlayerPositions()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<Transform>().position = spawnPositions[i].GetComponent<Transform>().position;
                players[i].GetComponent<Transform>().rotation = spawnPositions[i].GetComponent<Transform>().rotation;
            }
        }

    }
}