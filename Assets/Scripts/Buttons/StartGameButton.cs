using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Global;
using UnityEngine.UI;

namespace Buttons
{
    public class StartGameButton : MonoBehaviour
    {
        public int numPlayers = 0;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void startSinglePlayerHosting()
        {
            GameObject.FindGameObjectWithTag(GlobalTags.NETWORK_MANAGER).GetComponent<GameSettings>().setSettings();
            NetworkManager.singleton.StartHost();
            //NetworkManager.singleton.ServerChangeScene(GlobalTags.LOADING_SCREEN);
        }

        public void startSinglePlayerGame()
        {

            NetworkManager.singleton.ServerChangeScene(GlobalTags.GAME_SCREEN);
        }

        public void updateNumPlayers()
        {
            numPlayers = GameObject.FindGameObjectsWithTag(GlobalTags.PLAYER).Length;
        }
    }
}
