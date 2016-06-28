using UnityEngine;
using System.Collections;
using Controllers;
using Behaviors;
using UnityEngine.Networking;

namespace Global
{
    public class GameState : NetworkBehaviour
    {
        //[SyncVar]
        public static GameState instance = null; 
        GameObject[] players;
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
        }

        // Use this for initialization
        void Start()
        {
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 60;

            spawnPositions = GameObject.FindGameObjectsWithTag(GlobalTags.SPAWN_POSITION);

            InstantiateAIPlayers(spawnPositions);

            int numPlayers = GameObject.FindGameObjectsWithTag(GlobalTags.PLAYER).Length;
            players = GetGlobalObjects.getInitialPlayers();

            // Game is now ready to play.
            // Let's switch scenes and enable the player controller.

            NetworkManager.singleton.ServerChangeScene(GlobalTags.GAME_SCREEN);

            //there is an OnSceneWasLoaded for the scene change below.
            
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().enabled = true;


            
         

            
        }



        // Update is called once per frame
        void Update()
        {

            GameObject[] all = (GameObject[])Resources.FindObjectsOfTypeAll(gameObject.GetType());
            //gameOver = checkIfAllPlayersDead();
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

        public GameObject[] getAllPlayers()
        {
            return players;
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
            for(int i = 0; i < players.Length; i++)
            {
                players[i].tag = "Player " + i;
                players[i].GetComponent<Controllers.PlayerController>().PlayerNum = i;
            }
        }

        private void assignControllablePlayer()
        {
            players[0].GetComponent<Controllers.PlayerController>().IsControlledPlayer = true;
        }

        private void setAIPlayers()
        {
            for (int i = 1; i < players.Length; i++ )
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

        private void InstantiateAIPlayers(GameObject[] spawnPositions)
        {
            int numAI = (int)GameObject.FindGameObjectWithTag(GlobalTags.NETWORK_MANAGER).GetComponent<GameSettings>().numAI;
            //GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag(GlobalTags.SPAWN_POSITION);
            Vector3 humanPlayerPosition = GameObject.FindGameObjectWithTag(GlobalTags.PLAYER).GetComponent<Transform>().position;

            for (int i = 1; i <= numAI; i++)
            {
                if (spawnPositions[i].GetComponent<Transform>().position == humanPlayerPosition)
                {
                    numAI++; // let's skip this spawn Position
                }
                else
                {
                    GameObject ai = (GameObject)Instantiate(aiPrefab);
                    ai.GetComponent<Transform>().position = spawnPositions[i].GetComponent<Transform>().position;
                    ai.GetComponent<Transform>().rotation = spawnPositions[i].GetComponent<Transform>().rotation;
                }

            }
        }

        private void enablePlayerControllers()
        {
            foreach (GameObject player in players) // now we need to setup the AIManager class
            {
                player.GetComponent<Controllers.PlayerController>().enabled = true;   
            }
        }

        public override void OnStartServer()
        {
                //NetworkManager.singleton.OnServerSceneChanged()
                //NetworkClient.Instance.Ready();
                //NetworkManager.singleton.SpawnObjects();
                //NetworkServer.SpawnObjects();

                GameObject[] go = (GameObject[])GameObject.FindObjectsOfType(gameObject.GetType());

                GameObject[] all = (GameObject[])GameObject.FindObjectsOfTypeAll(gameObject.GetType());


                //updatePlayerList();
                players = GetGlobalObjects.getInitialPlayers();

                /*fixPlayerPosition(spawnPositions[0]);

                gameOver = checkIfAllPlayersDead();  // do we need this here?  is the update() one enough?

                assignPlayerNumbers();
                setAIPlayers();

                cooldowns = Cooldowns.Instance; // create an instance of the Cooldowns singleton class after players have been initialized.

                enablePlayerControllers();
                gameReady = true;*/
            
        }

        private void updatePlayerList()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = GameObject.FindGameObjectWithTag(GlobalTags.PLAYERS[i]);
            }
        }
        private void fixPlayerPosition(GameObject spawnPosition)
        {
            GetGlobalObjects.getControllablePlayer().GetComponent<Transform>().position = spawnPosition.GetComponent<Transform>().position;
            GetGlobalObjects.getControllablePlayer().GetComponent<Transform>().rotation = spawnPosition.GetComponent<Transform>().rotation;
        }
    }
}