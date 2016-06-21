using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Constants;
using Controllers;
using Behaviors;

public class BoostButton: Button {

    private GameObject playerObject;

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
        //if (PlayerBehaviors.isBoostReady)
        //{
            //PlayerBehaviors.isBoostReady = false;
            playerObject = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(PlayerBehaviors.activateSpeedBoost(playerObject));
        //}
    }
}
