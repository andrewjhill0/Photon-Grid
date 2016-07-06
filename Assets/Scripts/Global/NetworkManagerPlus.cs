using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Global;

public class NetworkManagerPlus : NetworkManager {

	public GameObject gameStatePrefab;

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == GlobalTags.GAME_SCREEN)
        {

            /*GameState.instance.getGameReady();

            //NetworkManager.singleton.OnServerSceneChanged()
            //NetworkClient.Instance.Ready();
            //NetworkManager.singleton.SpawnObjects();
            //NetworkServer.SpawnObjects();
            GameObject[] numPlayers = GameObject.FindGameObjectsWithTag(GlobalTags.PLAYER); 
            GameObject[] go = (GameObject[])GameObject.FindObjectsOfType(gameObject.GetType());

            GameObject[] all = (GameObject[])GameObject.FindObjectsOfTypeAll(gameObject.GetType());

            Debug.Log(" ");*/
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        bool ready = conn.isReady;
        NetworkServer.Spawn(gameStatePrefab);

    }
}
