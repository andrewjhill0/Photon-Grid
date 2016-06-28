using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Constants;
using Controllers;
using Behaviors;
using Global;


namespace Buttons
{
    public class BoostButton : Button
    {


        // Use this for initialization
        protected override void Start()
        {

        }

        // Update is called once per frame
        public void Update()
        {
            //A public function in the selectable class which button inherits from.
            if (IsPressed())
            {
                WhilePressed();
            }
        }

        public void WhilePressed()
        {
            boost();
        }

        public void boost()
        {
            GameObject playerObject;
            playerObject = GetGlobalObjects.getControllablePlayer();
            StartCoroutine(PlayerBehaviors.activateSpeedBoost(playerObject));
        }
    }
}