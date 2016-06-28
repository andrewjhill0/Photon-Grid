using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Global;

public class NumAISliderLabel : MonoBehaviour {

    private Text textComponent;

	// Use this for initialization
	void Start () {
        textComponent = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        textComponent.text = GameObject.FindGameObjectWithTag(GlobalTags.NUM_AI_SLIDER).GetComponent<Slider>().value.ToString();
	}
}
