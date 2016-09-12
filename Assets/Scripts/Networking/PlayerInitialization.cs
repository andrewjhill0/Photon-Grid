using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Global;
using System.Collections.Generic;

namespace Networking
{
    public class PlayerInitialization : NetworkBehaviour
    {
        void Update()
        {
            
            if (GameState.Instance != null)
            {
                if (GameState.Instance.gameReady)
                {
                    List<GameObject> players = GameState.Instance.getAllPlayers();
                    Debug.Log("Player 0 is localplayer? " + players[0].GetComponent<Controllers.PlayerController>().isLocalPlayer, players[0]);
                    Debug.Log("Player 1 is localplayer? " + players[1].GetComponent<Controllers.PlayerController>().isLocalPlayer, players[1]);

                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].gameObject.GetInstanceID() == gameObject.GetInstanceID())
                        {
                            Debug.Log("Player #" + i + "is local player.  Setting tag, materials, and spawn position/rotation.");
                            gameObject.tag = GlobalTags.PLAYERS[i];
                            GetComponent<Controllers.PlayerController>().PlayerNum = i;

                            Material mat = (Material)Resources.Load("Vehicles/" + GlobalTags.PLAYER_COLORS[i], typeof(Material));
                            GetComponent<Renderer>().material = mat;

                            if (isClient)
                            {
                                players[i].GetComponent<Transform>().position = GameState.Instance.spawnPositions[i].GetComponent<Transform>().position;
                                players[i].GetComponent<Transform>().rotation = GameState.Instance.spawnPositions[i].GetComponent<Transform>().rotation;

                            }
                            GetComponent<BoxCollider>().enabled = true;
                            GetComponent<Controllers.PlayerController>().enabled = true;
                            enabled = false;  // we're finished with this behavior object
                        }
                    }


                    
                }
            }
            
        }
    }
}

