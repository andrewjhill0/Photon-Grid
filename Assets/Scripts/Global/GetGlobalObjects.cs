using Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Controllers;

namespace Global
{
    public static class  GetGlobalObjects
    {
        public static Cooldowns getCooldownsInstance()
        {
            //GameState gameState = getGameState();
            //Cooldowns cooldowns = gameState.cooldowns;
            //return cooldowns;
            return Cooldowns.Instance;
        }

        public static GameState getGameState()
        {
            GameObject globalGameState = GameObject.FindGameObjectsWithTag(GlobalTags.GAME_STATE)[0];
            GameState gameState = globalGameState.GetComponent<GameState>();
            return gameState;
        }

        public static GameObject[] getInitialPlayers()
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag(GlobalTags.PLAYER);
            return GameObject.FindGameObjectsWithTag(GlobalTags.PLAYER);
        }

        public static GameObject[] getPlayerArray()
        {
            return getGameState().getAllPlayers();
        }

        public static GameObject getPlayer(int playerNum)
        {
            return getGameState().getPlayer(playerNum);
        }

        public static GameObject getControllablePlayer()
        {
            foreach(GameObject player in getGameState().getAllPlayers())
            {
                if (player.GetComponent<PlayerController>().isLocalPlayer)
                {
                    return player;
                }
            }
            return null;
        }

        public static int getNumberOfPlayers()
        {
            return getGameState().getAllPlayers().Length;
        }

        public static AIManager getAIManager()
        {
            return AIManager.instance;
        }
    }
}
