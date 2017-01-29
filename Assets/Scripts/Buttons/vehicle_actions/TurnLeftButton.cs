using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Controllers;
using Constants;
using Behaviors;
using Global;
using Assets.Scripts.Buttons;

namespace Buttons {
    public class TurnLeftButton : TurnButton {
        public override void turn() {
            GameObject playerObject;
            playerObject = GetGlobalObjects.getControllablePlayer();
            PlayerBehaviors.turnPlayer(playerObject, InputConstants.INPUT_LEFT);
        }
    }
}