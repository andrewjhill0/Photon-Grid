using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Constants;
using Controllers;
using Behaviors;
using Global;
using Assets.Scripts.Buttons;

namespace Buttons {
    public class TurnRightButton : TurnButton {
        public override void turn() {
            GameObject playerObject;
            playerObject = GetGlobalObjects.getControllablePlayer();
            PlayerBehaviors.turnPlayer(playerObject, InputConstants.INPUT_RIGHT);
        }
    }
}