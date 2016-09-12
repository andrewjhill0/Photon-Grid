using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behaviors;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Global
{
    class GameSettings : MonoBehaviour
    {
        public int numAI = 0;
        public int colorChoice = 0;
        public int maxHumanPlayers = 1;
        public bool isMultiplayerGame = false;
        
        void Awake()
        {
            gameObject.SetActive(true);
        }
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void startHosting()
        {
            NetworkManager.singleton.StartHost();
        }

        public void setSettings()
        { 
            if (GameObject.FindGameObjectWithTag(GlobalTags.MAX_HUMANS_SLIDER) != null) // we are in the Multiplayer host menu
            { 
                maxHumanPlayers = (int)GameObject.FindGameObjectWithTag(GlobalTags.MAX_HUMANS_SLIDER).GetComponent<Slider>().value;
                isMultiplayerGame = true;
            }
            numAI = (int) GameObject.FindGameObjectWithTag(GlobalTags.NUM_AI_SLIDER).GetComponent<Slider>().value;
            colorChoice = (int) GameObject.FindGameObjectWithTag(GlobalTags.COLOR_SLIDER).GetComponent<Slider>().value;
        }
    }
}
