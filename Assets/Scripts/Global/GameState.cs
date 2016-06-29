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
        //[SyncVar]
        public static GameState instance = null; 
        //GameObject[] players;
        List<GameObject> players;
        //[SyncVar]
        bool gameOver;
        //[SyncVar]
        public Cooldowns cooldowns;

        public GameObject aiPrefab;
        public bool gameReady = false;

        GameObject[] spawnPositions;


        void Awake()
        {
            if (instance == null)       instance = this;
            else if (instance != this)  Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            players = new List<GameObject>();
        }

        // Use this for initialization
        void Start()
        {
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 60;

            spawnPositions = GameObject.FindGameObjectsWithTag(GlobalTags.SPAWN_POSITION);

            // GameState is now initialized and ready to be used.
            // Let's switch scenes and enable the player controller.

            NetworkManager.singleton.ServerChangeScene(GlobalTags.GAME_SCREEN);

            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().enabled = true;
            
        }



        // Update is called once per frame
        void Update()
        {
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

        public void getGameReady()
        {
            if (!gameReady)
            {
                InstantiateAIPlayers();
                fixPlayerPositions();

                int numPlayers = NetworkManager.singleton.numPlayers + GameObject.FindGameObjectWithTag(GlobalTags.NETWORK_MANAGER).GetComponent<GameSettings>().numAI;


                gameOver = checkIfAllPlayersDead();  // do we need this here?  is the update() one enough?

                assignPlayerNumbers();
                setAIPlayers();

                cooldowns = Cooldowns.Instance; // create an instance of the Cooldowns singleton class after players have been initialized.

                enablePlayerControllers();
                enableCameraController();
                gameReady = true;
            }

        }

        public void updatePlayerList(GameObject playerObject)
        {
            if (!players.Contains(playerObject))
            {
                players.Add(playerObject);
            }
            if (players.Count == NetworkManager.singleton.numPlayers)
            {
                getGameReady();
            }
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
            for (int i = 1; i < players.Count; i++ )
            {
                if (!players[i].GetComponent<Controllers.PlayerController>().isLocalPlayer)
                {
                    players[i].GetComponent<Controllers.PlayerController>().IsAI = true;
                }
            }

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
            int numAI = (int)GameObject.FindGameObjectWithTag(GlobalTags.NETWORK_MANAGER).GetComponent<GameSettings>().numAI;
            for (int i = 1; i <= numAI; i++)
            {
                    GameObject ai = (GameObject)Instantiate(aiPrefab);
            }
        }

        private void enablePlayerControllers()
        {
            foreach (GameObject player in players) // now we need to setup the AIManager class
            {
                player.GetComponent<Controllers.PlayerController>().enabled = true;   
            }
        }

        private void enableCameraController()
        {
            GameObject.FindGameObjectWithTag(GlobalTags.CAMERA).GetComponent<CameraController>().enabled = true;
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