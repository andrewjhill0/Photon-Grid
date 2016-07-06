using Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

namespace Networking
{
    class GameInitializationManager : LobbyManager // normal lobby manager functions with some custom overrides specific to this game.
    {
        void OnLevelWasLoaded(int level)
        {
            if(level == GlobalTags.LOBBY_SCREEN_NUM)
            {
                if (GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT) != null)
                {
                    int numPlayers = GameObject.FindGameObjectWithTag(GlobalTags.GAME_SETTINGS_OBJECT).GetComponent<GameSettings>().maxHumanPlayers;
                    minPlayers = numPlayers;
                    maxPlayers = numPlayers;
                }
            }
        }

        public override void OnLobbyServerSceneChanged(string sceneName)
        {
            base.OnLobbyServerSceneChanged(sceneName);

            if(sceneName == GlobalTags.GAME_SCREEN)
            {
                Debug.Log("Does GameState Exist? ");
                if(GameObject.FindGameObjectWithTag(GlobalTags.GAME_STATE) != null)
                {
                    Debug.Log("Yes");
                }
                else
                {
                    Debug.Log("No");
                }
                NetworkServer.SpawnObjects();

                GameObject.FindGameObjectWithTag(GlobalTags.GAME_STATE).GetComponent<GameState>().getGameReady();

            }

        }

        
    }
}
