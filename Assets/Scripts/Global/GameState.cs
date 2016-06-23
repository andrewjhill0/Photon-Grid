using UnityEngine;
using System.Collections;
using Controllers;

namespace Global
{
    public class GameState : MonoBehaviour
    {
        GameObject[] players;
        bool gameOver;


        // Use this for initialization
        void Start()
        {
            players = GetGlobalObjects.getPlayerArray();
            gameOver = checkIfAllPlayersDead();
            assignPlayerNumbers();
            assignControllablePlayer();
            Debug.Log("breakpoint");
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
            if (aliveCount == 0)
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
            }
        }

        private void assignControllablePlayer()
        {
            players[1].GetComponent<PlayerController>().IsControlledPlayer = true;
        }
    }
}