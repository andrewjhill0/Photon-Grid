using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Constants;
using Controllers;
using Behaviors;
using Global;
using Assets.Scripts.Buttons;


namespace Buttons
{
    public class BoostButton : VehicleActionButton
    {
        public override void WhilePressed()
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