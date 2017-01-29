using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.Buttons {
    public abstract class TurnButton : VehicleActionButton {

        public override void WhilePressed() {
            turn();
        }

        public abstract void turn();
    }
}
