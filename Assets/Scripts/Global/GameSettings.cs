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
    class GameSettings : NetworkBehaviour
    {
        public int numAI = 0;
        public int colorChoice = 0;
        public int maxHumanPlayers = 1;
        
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
            maxHumanPlayers = (int) GameObject.FindGameObjectWithTag(GlobalTags.MAX_HUMANS_SLIDER).GetComponent<Slider>().value;
            numAI = (int) GameObject.FindGameObjectWithTag(GlobalTags.NUM_AI_SLIDER).GetComponent<Slider>().value;
            colorChoice = (int) GameObject.FindGameObjectWithTag(GlobalTags.COLOR_SLIDER).GetComponent<Slider>().value;
        }
    }
}
