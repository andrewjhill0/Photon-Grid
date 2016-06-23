using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Constants;
using Controllers;
using Behaviors;
using Global;

public class TurnRightButton: Button {

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
        turnRight();
    }

    public void turnRight()
    {
        GameObject playerObject;
        playerObject = GetGlobalObjects.getControllablePlayer();
        PlayerBehaviors.turnPlayer(playerObject, InputConstants.INPUT_RIGHT);
    }
}
