using Global;
using Prototype.NetworkLobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Networking
{
    class GameInitializationHook : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {

            //base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
            Debug.Log("Does GameState Exist? ");
            if (GameObject.FindGameObjectWithTag(GlobalTags.GAME_STATE) != null)
            {
                Debug.Log("Yes");
            }
            else
            {
                Debug.Log("No");
            }
            if (SceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {
                CmdSpawnObjects();
            }
            Debug.Log("Does GameState Exist? ");
            if (GameObject.FindGameObjectWithTag(GlobalTags.GAME_STATE) != null)
            {
                Debug.Log("Yes");
            }
            else
            {
                Debug.Log("No");
            }


        }

        private void CmdSpawnObjects()
        {
            NetworkServer.SpawnObjects();
        }
    }
}
