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
            players = GetGlobalObjects.getInitialPlayers();
            gameOver = checkIfAllPlayersDead();
            assignPlayerNumbers();
            assignControllablePlayer();
            Debug.Log("breakpoint"); 
            
            cooldowns = Cooldowns.Instance;
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
            players[2].GetComponent<PlayerController>().IsControlledPlayer = true;
        }
    }
}