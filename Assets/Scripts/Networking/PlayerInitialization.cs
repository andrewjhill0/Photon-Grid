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

                    for (int i = 0; i < players.Count; i++)
                    {
                        if(players[i].GetComponent<Controllers.PlayerController>().isLocalPlayer)
                        {
                            gameObject.tag = GlobalTags.PLAYERS[i];
                            GetComponent<Controllers.PlayerController>().PlayerNum = i;

                            Material mat = (Material)Resources.Load("Vehicles/" + GlobalTags.PLAYER_COLORS[i], typeof(Material));
                            GetComponent<Renderer>().material = mat;

                            players[i].GetComponent<Transform>().position = GameState.Instance.spawnPositions[i].GetComponent<Transform>().position;
                            players[i].GetComponent<Transform>().rotation = GameState.Instance.spawnPositions[i].GetComponent<Transform>().rotation;
                        }
                    }


                    GetComponent<BoxCollider>().enabled = true;
                    GetComponent<Controllers.PlayerController>().enabled = true;
                    enabled = false;
                }
            }
        }
    }
}

