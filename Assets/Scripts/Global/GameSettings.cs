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
        public float numAI = 0;
        public float colorChoice = 0;
        
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
            numAI = GameObject.FindGameObjectWithTag(GlobalTags.NUM_AI_SLIDER).GetComponent<Slider>().value;
            colorChoice = GameObject.FindGameObjectWithTag(GlobalTags.COLOR_SLIDER).GetComponent<Slider>().value;
        }
    }
}
