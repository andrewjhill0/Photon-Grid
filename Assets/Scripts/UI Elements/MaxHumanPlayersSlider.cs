﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Global;


namespace UI
{
    public class MaxHumanPlayersSlider : MonoBehaviour
    {
        private Slider mainSlider;
        private Text textComponent;

        // Use this for initialization
        void Start()
        {
            textComponent = GetComponent<Text>();
            mainSlider = GameObject.FindGameObjectWithTag(GlobalTags.MAX_HUMANS_SLIDER).GetComponent<Slider>();
            mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }

        // Update is called once per frame
        void Update()
        {

            
            textComponent.text = GameObject.FindGameObjectWithTag(GlobalTags.MAX_HUMANS_SLIDER).GetComponent<Slider>().value.ToString();
        }

        public void ValueChangeCheck()
        {
            int maxHumans = (int)mainSlider.value;
            int numAI = (int)GameObject.FindGameObjectWithTag(GlobalTags.NUM_AI_SLIDER).GetComponent<Slider>().value;

            while (numAI + maxHumans > 8)
                {
                    GameObject.FindGameObjectWithTag(GlobalTags.NUM_AI_SLIDER).GetComponent<Slider>().value--;
                    numAI = (int) GameObject.FindGameObjectWithTag(GlobalTags.NUM_AI_SLIDER).GetComponent<Slider>().value;
                    Debug.Log("");
                }
        }
    }
}
