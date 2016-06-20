using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Constants;
using Controllers;

public class BoostButton: Button {

    private GameObject playerObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        //A public function in the selectable class which button inherits from.
        if (IsPressed())
        {
			if(PlayerBehaviors.IS_BOOST_READY)
			{
				PlayerBehaviors.IS_BOOST_READY = false;
            	playerObject = GameObject.FindGameObjectWithTag("Player");
            	StartCoroutine (PlayerBehaviors.activateSpeedBoost(playerObject));
			}
        	
        }
    }

    public void WhilePressed()
    {
        
    }

    public void turnRight()
    {

    }
}
