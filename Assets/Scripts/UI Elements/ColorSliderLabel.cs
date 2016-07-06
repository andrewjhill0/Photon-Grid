using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Global;

namespace UI
{
    public class ColorSliderLabel : MonoBehaviour
    {

        private Text textComponent;

        // Use this for initialization
        void Start()
        {
            textComponent = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            int colorValue = int.Parse(GameObject.FindGameObjectWithTag(GlobalTags.COLOR_SLIDER).GetComponent<Slider>().value.ToString());
            textComponent.text = GlobalTags.PLAYER_COLORS[colorValue];
        }
    }
}
