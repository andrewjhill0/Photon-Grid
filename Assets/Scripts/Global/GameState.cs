using UnityEngine;
using System.Collections;
using Controllers;
using Behaviors;

namespace Global
{
    public class GameState : MonoBehaviour
    {
        GameObject[] players;
        bool gameOver; 
        public Cooldowns cooldowns;


        // Use this for initialization
        void Start()
        {
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 60;
            players = GetGlobalObjects.getInitialPlayers();
            gameOver = checkIfAllPlayersDead();
            assignPlayerNumbers();
            assignControllablePlayer();
            Debug.Log("breakpoint"); 
            
            cooldowns = Cooldowns.Instance; // create an instance of the Cooldowns singleton class after players have been initialized.

            foreach(GameObject player in players) // now we need to setup the AIManager class
            {
                if(player.GetComponent<PlayerController>().isAI)
                {
                    AIManager.instance.AiPlayerObjects.Add(player);
                    AIManager.instance.AiPlayers.Add(player.GetComponent<PlayerController>().PlayerNum);
                }
            }
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
                PlayerController playerContr = player.GetComponent<PlayerController>();
                bool isAlive = playerContr.isAlive;
                if(isAlive)
                {
                    aliveCount++;
                }
            }
            if (aliveCount <= 1)
            {
                Debug.Log("End game");
                allDead = true;
            }
            return allDead;
        }

        private void assignPlayerNumbers()
        {
            for(int i = 0; i < players.Length; i++)
            {
                players[i].tag = "Player " + i;
                players[i].GetComponent<PlayerController>().PlayerNum = i;
            }
        }

        private void assignControllablePlayer()
        {
            players[6].GetComponent<PlayerController>().IsControlledPlayer = true;
        }
    }
}