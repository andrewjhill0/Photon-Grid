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


        void Awake()
        {
            if (instance == null)       instance = this;
            else if (instance != this)  Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            //DontDestroyOnLoad(gameObject);
        }

        public override void OnStartServer()
        {
            
        }
        // Use this for initialization
        void Start()
        {
            //StartHost();
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 60;
            //NetworkManager.singleton.numPlayers

            int numPlayers = GameObject.FindGameObjectsWithTag(GlobalTags.PLAYER).Length;
            players = GetGlobalObjects.getInitialPlayers();
            gameOver = checkIfAllPlayersDead();
            assignPlayerNumbers();
            assignControllablePlayer();
            setAIPlayers();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().enabled = true;
            cooldowns = Cooldowns.Instance; // create an instance of the Cooldowns singleton class after players have been initialized.

            foreach (GameObject player in players) // now we need to setup the AIManager class
            {
                if (player.GetComponent<Controllers.PlayerController>().IsAI)
                {
                    AIManager.instance.AiPlayerObjects.Add(player);
                    AIManager.instance.AiPlayers.Add(player.GetComponent<Controllers.PlayerController>().PlayerNum);
                }
            }
         

            
        }

        // Update is called once per frame
        void Update()
        {
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
                players[i].GetComponent<Controllers.PlayerController>().IsAI = true;
            }
        }
    }
}