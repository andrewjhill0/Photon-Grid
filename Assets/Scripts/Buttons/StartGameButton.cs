using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Global;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startSinglePlayerHosting()
    {
        GameObject.FindGameObjectWithTag(GlobalTags.NETWORK_MANAGER).GetComponent<GameSettings>().setSettings();
        NetworkManager.singleton.StartHost();
        NetworkManager.singleton.ServerChangeScene(GlobalTags.LOADING_SCREEN);
    }
}
