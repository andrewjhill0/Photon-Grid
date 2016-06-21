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

        }

        // Update is called once per frame
        void Update()
        {
            gameOver = checkIfAllPlayersDead();
        }

        public bool getGameOver() 
        {
            return gameOver;
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
    }
}